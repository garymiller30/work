using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UserForms.PDF.Visual;
using Krypton.Toolkit;
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
        const double COEF_DISTANCE = 0.3;


        HardCoverParams _coverParams;

        double zagyn;
        double width;
        double height;
        double root;
        double rastav;
        double totalWidth;
        double totalHeight;


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
                        
                        zagyn = mn(_coverParams.Zagyn);
                        width = mn(_coverParams.Width);
                        height = mn(_coverParams.Height);
                        root = mn(_coverParams.Root);
                        rastav = mn(_coverParams.Rastav);
                        totalWidth = mn(_coverParams.TotalWidth);
                        totalHeight = mn(_coverParams.TotalHeight);

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
                    CreateHardCoverBack(file.FullName, _coverParams.BackAnglesCut);
                }
                if (_coverParams.CreateFilePlusSchema)
                {
                    CreateHardCover(file.FullName, true);
                }
                if (_coverParams.CreateSchema)
                {
                    CreateHardCover(file.FullName, false);
                }

            }
        }

        public void CreateHardCover(string file, bool filePlusSchema)
        {
            string suffix = filePlusSchema ? "_+_schema.pdf" : "_schema.pdf";
            string output_file = Path.Combine(_coverParams.FolderOutput, $"{Path.GetFileNameWithoutExtension(file)}{suffix}");

            using (var p = new PDFlib())
            {
                try
                {
                    p.begin_document(output_file, "optimize=true");

                    p.begin_page_ext(totalWidth, totalHeight, "");

                    int l_print = p.define_layer("print", "");
                    int v_layer = p.define_layer("visual", "");

                    if (filePlusSchema)
                    {
                        p.begin_layer(l_print);
                        var doc = p.open_pdi_document(file, "");
                        var page_handle = p.open_pdi_page(doc, 1, "");
                        p.fit_pdi_page(page_handle, totalWidth / 2, totalHeight / 2, "position={center center}");
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
                    p.rect(0, 0, totalWidth, totalHeight);

                    DrawCoverLayout(p);

                    DrawDimensions(p, spot);

                    p.end_page_ext("");
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    Logger.Log.Error(null, "CreateHardCover", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
                }
            }
        }

        private void DrawDimensions(PDFlib p, int spot)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double[] widths = new double[7] { _coverParams.Zagyn, _coverParams.Width, _coverParams.Rastav, _coverParams.Root, _coverParams.Rastav, _coverParams.Width, _coverParams.Zagyn };
            double[] heigts = new double[3] { _coverParams.Zagyn, _coverParams.Height, _coverParams.Zagyn };

            double x = 0;
            double y = _coverParams.Zagyn + _coverParams.Height * COEF_DISTANCE;

            for (int i = 0; i < widths.Count(); i++)
            {
                PdfHelper.DrawDimensionsX(p, spot, x, y, widths[i]);
                x += widths[i];
            }

            x = _coverParams.Zagyn + _coverParams.Width * COEF_DISTANCE;
            y = 0;

            for (int i = 0; i < heigts.Count(); i++)
            {
                PdfHelper.DrawDimensionsY(p, spot, x, y, heigts[i]);
                y += heigts[i];
            }
        }

        double mn(double value) => value * PdfHelper.mn;

        void DrawCoverLayout(PDFlib p)
        {
            // ліва сторінка
            p.rect(zagyn, zagyn, width, height);
            // корінець
            p.rect(zagyn + width + rastav, zagyn, root, height);
            // права сторінка
            p.rect(zagyn + width + rastav * 2 + root,zagyn,width,height);
            p.stroke();
        }

        void DrawCoverAngleCuts(PDFlib p)
        {
            double pointOffset = mn(_coverParams.Zagyn - _coverParams.DistanceAngleCut / Math.Sqrt(2)) * 2;
            // тут малюємо прямокутник зі зрізаними кутами, відстань від сторінки = distance
            p.moveto(pointOffset, 0);
            p.lineto(totalWidth - pointOffset, 0); // низ
            p.lineto(totalWidth, pointOffset);
            p.lineto(totalWidth, totalHeight - pointOffset);
            p.lineto(totalWidth - pointOffset, totalHeight);
            p.lineto(pointOffset, totalHeight);
            p.lineto(0, totalHeight - pointOffset);
            p.lineto(0, pointOffset);
            p.lineto(pointOffset, 0);
            p.stroke();
        }

        void CreateHardCoverBack(string file,bool angleCuts = false)
        {
            string target = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}_back.pdf");
            
            using (var p = new PDFlib())
            {
                try
                {
                    p.begin_document(target, "optimize=true");

                    p.begin_page_ext(totalWidth, totalHeight, "");

                    // Draw cover schema here
                    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                    p.set_gstate(gstate);

                    MarkColor c = MarkColor.Black;
                    p.setcolor("fillstroke", "cmyk", c.C / 100, c.M / 100, c.Y / 100, c.K / 100);
                    p.setlinewidth(0.2);

                    if (angleCuts)
                    {
                        DrawCoverAngleCuts(p);
                    }
                    else
                    {
                        // розмір документу
                        p.rect(0, 0, totalWidth, totalHeight);

                    }

                    DrawCoverLayout(p);

                    p.end_page_ext("");
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    Logger.Log.Error(null, "CreateHardCoverBack", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
                }
            }
        }
    }
}
