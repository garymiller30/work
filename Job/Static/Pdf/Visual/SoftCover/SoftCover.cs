using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Visual.SoftCover
{
    [PdfTool("Візуалізація", "М'яка обкладинка", Icon = "visual_soft_cover", Order = 20)]
    public class SoftCover : IPdfTool
    {
        const double COEF_DIMENSION = 0.3;

        SoftCoverParams _coverParams;

        double totalWidth;
        double totalHeight;
        double width;
        double height;
        double bleed;
        double leftKlapan;
        double rightKlapan;
        double root;


        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                using (var form = new FormVisualSoftCover(file))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _coverParams = form.CoverParams;

                        totalHeight = mn(_coverParams.TotalHeight);
                        totalWidth = mn(_coverParams.TotalWidth);
                        width = mn(_coverParams.Width);
                        height = mn(_coverParams.Height);
                        bleed = mn(_coverParams.Bleed);
                        leftKlapan = mn(_coverParams.LeftKlapan);
                        rightKlapan = mn(_coverParams.RightKlapan);
                        root = mn(_coverParams.Root);

                        return true;
                    }
                }
            }
            return false;
        }

        double mn(double value) => value * PdfHelper.mn;

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                if (_coverParams.CreateSchema)
                {
                    CreateSoftCover(file.FullName,false);
                }
                if (_coverParams.CreateFileAndSchema)
                {
                    CreateSoftCover(file.FullName,true);
                }
            }
        }

        public void CreateSoftCover(string file, bool placeFile = false)
        {
            string suffix = placeFile ? "_+_schema.pdf" : "_schema.pdf";
            string output_file = Path.Combine(_coverParams.FolderOutput,$"{Path.GetFileNameWithoutExtension(file)}{suffix}");

            using (var p = new PDFlib())
            {
                try
                {
                    p.begin_document(output_file, "optimize=true");
                    p.begin_page_ext(totalWidth, totalHeight, "");
                    int l_print = p.define_layer("print", "");
                    int v_layer = p.define_layer("visual", "");

                    if (placeFile)
                    {
                        p.begin_layer(l_print);
                        var doc = p.open_pdi_document(file, "");
                        var page_handle = p.open_pdi_page(doc, 1, "");
                        p.fit_pdi_page(page_handle, totalWidth / 2, totalHeight / 2, "position={center center}");
                        p.close_pdi_page(page_handle);
                        p.end_layer();
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

                    // обрізний прямокутник
                    double x = bleed;
                    double y = bleed;
                    double w = totalWidth - 2 * bleed;
                    double h = totalHeight - 2 * bleed;

                    p.rect(x, y, w, h);

                    if (leftKlapan > 0)
                    {
                        x += leftKlapan;
                        DrawLine(p, x, y, h);
                    }

                    x += width;
                    DrawLine(p, x, y, h);

                    x += root;
                    DrawLine(p, x, y, h);

                    x += width;
                    DrawLine(p, x, y, h);
                    if (rightKlapan > 0)
                    {
                        x += rightKlapan;
                        DrawLine(p, x, y, h);
                    }

                    p.stroke();

                    DrawDimensions(p, spot);

                    x = bleed;
                    y = bleed;
                    w = totalWidth - x;
                    h = totalHeight - y;

                    p.end_page_ext($"trimbox {{{x} {y} {w} {h}}}");
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "SoftCoverGenerator");
                }
            }
        }
        void DrawLine(PDFlib p, double x, double y, double h)
        {
            p.moveto(x, y);
            p.lineto(x, y + h);
        }

        private void DrawDimensions(PDFlib p, int spot)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double[] widths = new double[7] { _coverParams.Bleed, _coverParams.LeftKlapan, _coverParams.Width, _coverParams.Root, _coverParams.Width, _coverParams.RightKlapan, _coverParams.Bleed };
            double[] heigts = new double[3] { _coverParams.Bleed, _coverParams.Height, _coverParams.Bleed };

            double x = 0;
            double y = _coverParams.Bleed + _coverParams.Height * COEF_DIMENSION;

            for (int i = 0; i < widths.Count(); i++)
            {
                if (widths[i] == 0) continue;
                PdfHelper.DrawDimensionsX(p, spot, x, y, widths[i]);
                x += widths[i];
            }

            x = _coverParams.Bleed + _coverParams.Width * COEF_DIMENSION;
            y = 0;

            for (int i = 0; i < heigts.Count(); i++)
            {
                PdfHelper.DrawDimensionsY(p, spot, x, y, heigts[i]);
                y += heigts[i];
            }
        }
    }
}
