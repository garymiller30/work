using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
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
                if (_coverParams.Command == HardCoverParams.CreateCommand.Back) { CreateHardCoverBack(file.FullName); }
                else { CreateHardCover(file.FullName); }

            }
        }

        public void CreateHardCover(string file)
        {
            string output_file = null;

            if (_coverParams.Command == HardCoverParams.CreateCommand.CreateSchema)
            {
                output_file = Path.Combine(_coverParams.FolderOutput, $"{Path.GetFileNameWithoutExtension(file)}_schema.pdf");
            }
            else
            {
                output_file = Path.Combine(_coverParams.FolderOutput, $"{Path.GetFileNameWithoutExtension(file)}_+_schema.pdf");
            }

            var p = new PDFlib();

            try
            {
                p.begin_document(output_file, "optimize=true");

                p.begin_page_ext(_coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn, "");

                int l_print = p.define_layer("print", "");
                int v_layer = p.define_layer("visual", "");

                if (_coverParams.Command == HardCoverParams.CreateCommand.CreateCover)
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
                Logger.Log.Error(null, "PdfSpliter", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
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
                //int spot = p.makespotcolor(c.Name);
                // p.setdash(4, 2);
                //p.set_graphics_option("dasharray={4 2}");
                p.setlinewidth(0.2);
                //p.setcolor("fillstroke", "spot", spot, 1.0, 0, 0);

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
                Logger.Log.Error(null, "PdfSpliter", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
