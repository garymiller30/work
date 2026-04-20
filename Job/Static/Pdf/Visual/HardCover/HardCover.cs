using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Visual.HardCover
{
    [PdfTool("Візуалізація", "Тверда обкладинка", Icon = "visual_hard_cover", Order = 20)]
    public class HardCover : IPdfTool
    {
        HardCoverParams _coverParams;

        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                using (var form = new FormVisualHardCover(file))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _coverParams = form.CoverParams;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                if (_coverParams.CreateBack)
                {
                    if (_coverParams.BackAnglesCut)
                    {
                        CreateHardCoverBackAnglesCut(file.FullName);
                    }
                    else
                    {
                        CreateHardCoverBack(file.FullName);
                    }
                    
                }
                if (_coverParams.CreateFilePlusSchema)
                {
                    CreateHardCover(file.FullName,true);
                }
                if (_coverParams.CreateSchema)
                {
                    CreateHardCover(file.FullName,false);
                }

            }
        }

        private void CreateHardCoverBackAnglesCut(string file)
        {

            double pointOffset =(_coverParams.Zagyn - _coverParams.DistanceAngleCut/Math.Sqrt(2)) * 2;
            string target = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}_back.pdf");
            var p = new PDFlib();

            try
            {
                p.begin_document(target, "optimize=true");

                p.begin_page_ext(_coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn, "");

                // Draw cover schema here
                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                p.set_gstate(gstate);

                MarkColor c = MarkColor.Black;
                p.setcolor("fillstroke", "cmyk", c.C / 100, c.M / 100, c.Y / 100, c.K / 100);
                p.setlinewidth(0.2);

                // тут малюємо прямокутник зі зрізаними кутами, відстань від сторінки = distance
                p.moveto(pointOffset * PdfHelper.mn, 0);
                p.lineto( (_coverParams.TotalWidth - pointOffset) * PdfHelper.mn, 0); // низ
                p.lineto(_coverParams.TotalWidth * PdfHelper.mn, pointOffset * PdfHelper.mn);
                p.lineto(_coverParams.TotalWidth * PdfHelper.mn, (_coverParams.TotalHeight - pointOffset) * PdfHelper.mn);
                p.lineto((_coverParams.TotalWidth - pointOffset) * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn);
                p.lineto(pointOffset * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn);
                p.lineto(0, (_coverParams.TotalHeight - pointOffset) * PdfHelper.mn);
                p.lineto(0, pointOffset * PdfHelper.mn);
                p.lineto(pointOffset * PdfHelper.mn, 0);
                p.stroke();

                // ліва сторінка
                p.rect(_coverParams.Zagyn * PdfHelper.mn, _coverParams.Zagyn * PdfHelper.mn, _coverParams.Width * PdfHelper.mn, _coverParams.Height * PdfHelper.mn);
                // корінець
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav) * PdfHelper.mn,
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Root * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                // права сторінка
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav * 2 + _coverParams.Root) * PdfHelper.mn,
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Width * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                p.stroke();

                p.end_page_ext("");
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "CreateHardCoverBack", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }

        public void CreateHardCover(string file, bool filePlusSchema)
        {
            string output_file = null;

            if (filePlusSchema)
            {
                output_file = Path.Combine(_coverParams.FolderOutput, $"{Path.GetFileNameWithoutExtension(file)}_+_schema.pdf");
            }
            else
            {
                output_file = Path.Combine(_coverParams.FolderOutput, $"{Path.GetFileNameWithoutExtension(file)}_schema.pdf");
            }

            var p = new PDFlib();

            try
            {
                p.begin_document(output_file, "optimize=true");

                p.begin_page_ext(_coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn, "");

                int l_print = p.define_layer("print", "");
                int v_layer = p.define_layer("visual", "");

                if (filePlusSchema)
                {
                    p.begin_layer(l_print);
                    var doc = p.open_pdi_document(file, "");
                    var page_handle = p.open_pdi_page(doc, 1, "");
                    p.fit_pdi_page(page_handle, (_coverParams.TotalWidth / 2) * PdfHelper.mn, (_coverParams.TotalHeight / 2) * PdfHelper.mn, "position={center center}");
                    p.close_pdi_page(page_handle);

                }
                p.begin_layer(v_layer);
                // Draw cover schema here
                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                p.set_gstate(gstate);

                MarkColor c = MarkColor.ProofColor;
                p.setcolor("fillstroke", "cmyk", c.C / 100, c.M / 100, c.Y / 100, c.K / 100);
                int spot = p.makespotcolor(c.Name);
                // p.setdash(4, 2);
                p.set_graphics_option("dasharray={4 2}");
                p.setlinewidth(0.2);
                p.setcolor("fillstroke", "spot", spot, 1.0, 0, 0);

                // розмір документу
                p.rect(0, 0, _coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn);
                // ліва сторінка
                p.rect(_coverParams.Zagyn * PdfHelper.mn, _coverParams.Zagyn * PdfHelper.mn, _coverParams.Width * PdfHelper.mn, _coverParams.Height * PdfHelper.mn);
                // корінець
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav) * PdfHelper.mn,
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Root * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                // права сторінка
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav * 2 + _coverParams.Root) * PdfHelper.mn,
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Width * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                p.stroke();

                DrawDimensions(p, spot);

                p.end_page_ext("");
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "CreateHardCover", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }

        private void DrawDimensions(PDFlib p, int spot)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double[] widths = new double[7] { _coverParams.Zagyn, _coverParams.Width, _coverParams.Rastav, _coverParams.Root, _coverParams.Rastav, _coverParams.Width, _coverParams.Zagyn };
            double[] heigts = new double[3] { _coverParams.Zagyn, _coverParams.Height, _coverParams.Zagyn };

            double x = 0;
            double y = _coverParams.Zagyn + _coverParams.Height * 0.3;

            for (int i = 0; i < widths.Count(); i++)
            {
                PdfHelper.DrawDimensionsX(p, spot, x, y, widths[i]);
                x += widths[i];
            }

            x = _coverParams.Zagyn + _coverParams.Width * 0.3;
            y = 0;

            for (int i = 0; i < heigts.Count(); i++)
            {
                PdfHelper.DrawDimensionsY(p, spot, x, y, heigts[i]);
                y += heigts[i];
            }
        }

        void CreateHardCoverBack(string file)
        {
            string target = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}_back.pdf");
            var p = new PDFlib();

            try
            {
                p.begin_document(target, "optimize=true");

                p.begin_page_ext(_coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn, "");

                // Draw cover schema here
                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                p.set_gstate(gstate);

                MarkColor c = MarkColor.Black;
                p.setcolor("fillstroke", "cmyk", c.C / 100, c.M / 100, c.Y / 100, c.K / 100);
                p.setlinewidth(0.2);

                // розмір документу
                p.rect(0, 0, _coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn);
                // ліва сторінка
                p.rect(_coverParams.Zagyn * PdfHelper.mn, _coverParams.Zagyn * PdfHelper.mn, _coverParams.Width * PdfHelper.mn, _coverParams.Height * PdfHelper.mn);
                // корінець
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav) * PdfHelper.mn,
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Root * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                // права сторінка
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav * 2 + _coverParams.Root) * PdfHelper.mn,
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Width * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                p.stroke();

                p.end_page_ext("");
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "CreateHardCoverBack", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
