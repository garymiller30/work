using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
using System;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Create.Falc
{
    [PdfTool("Візуалізація", "Фальцовка в намотку", Icon = "visual_falc", Order = 20)]
    public class FalcSchema : IPdfTool
    {
        const double COEF_DIMENSION = 0.3;
        const double DISTANCE_FROM_TRIM = 2;
        const double MARK_LENGTH = 2;

        FalcSchemaParams _param;

        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file == null) { return false; }

            using (var form = new FormVisualFalc(file))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _param = form.SchemaParams;
                    return true;
                }

            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                if (_param.CreateSchema)
                {
                    CreateFalcSchema(file.FullName, false);
                }
                if (_param.CreateFileAndSchema)
                {
                    CreateFalcSchema(file.FullName,true);
                }
                
            }
        }

        public void CreateFalcSchema(string filePath,bool fileAndSchema = false)
        {
            var boxes = PdfHelper.GetPagesInfo(filePath);

            // Логіка створення схеми для Falc на основі boxes та _param
            using (PDFlib p = new PDFlib())
            {
                try
                {
                    string sufix = fileAndSchema ? "_+_schema" : "_schema";

                    string targetfile = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(filePath), $"{System.IO.Path.GetFileNameWithoutExtension(filePath)}{sufix}.pdf");

                    MarkColor markColor = MarkColor.ProofColor;

                    p.begin_document(targetfile, "optimize=true");

                    int doc = -1;
                    if (fileAndSchema)
                    {
                        doc = p.open_pdi_document(filePath, "");
                    }
                    for (int idx = 0; idx < boxes.Count; idx++)
                    {
                        PdfPageInfo pageInfo = boxes[idx];

                        p.begin_page_ext(pageInfo.Mediabox.width, pageInfo.Mediabox.height, "");

                        int l_print = p.define_layer("print", "");
                        int v_layer = p.define_layer("visual", "");

                        if (fileAndSchema)
                        {
                            p.begin_layer(l_print);
                            var page_handle = p.open_pdi_page(doc, idx + 1, "");
                            p.fit_pdi_page(page_handle, 0, 0, "");
                            p.close_pdi_page(page_handle);

                            DrawFalcMarks(p, pageInfo, idx);
                        }

                        p.begin_layer(v_layer);
                        int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                        p.set_gstate(gstate);

                        p.setcolor("fillstroke", "cmyk", markColor.C / 100, markColor.M / 100, markColor.Y / 100, markColor.K / 100);
                        int spot = p.makespotcolor(markColor.Name);
                        p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
                        p.setlinewidth(1.0);
                        p.rect(pageInfo.Trimbox.left, pageInfo.Trimbox.bottom, pageInfo.Trimbox.width, pageInfo.Trimbox.height);
                        p.stroke();

                        double xOfs = pageInfo.Trimbox.left;
                        double yOfs = pageInfo.Trimbox.bottom;

                        decimal[] widths = _param.PartsWidth;

                        if (_param.Mirrored && idx % 2 == 0)
                        {
                            widths = _param.PartsWidth.Reverse().ToArray();
                        }
                        DrawPage(p, pageInfo, xOfs, yOfs, widths);
                        DrawDimension(p, spot, pageInfo, widths);

                        // Тут має бути логіка створення схеми Falc
                        // Використовуйте _param.Mirrored та _param.PartsWidth для налаштування схеми
                        p.end_page_ext($"trimbox {{{pageInfo.Trimbox.left} {pageInfo.Trimbox.bottom} {pageInfo.Trimbox.width + pageInfo.Trimbox.left} {pageInfo.Trimbox.bottom + pageInfo.Trimbox.height}}}");
                    }

                    if (doc != -1)
                    {
                        p.close_pdi_document(doc);
                    }

                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "FalcSchema");
                }
            }
        }

        private void DrawDimension(PDFlib p, int spot, PdfPageInfo pageInfo, decimal[] widths)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double x = pageInfo.Trimbox.leftMM();
            double y = pageInfo.Mediabox.hMM() * COEF_DIMENSION;

            for (int i = 0; i < widths.Length; i++)
            {
                if (widths[i] > 0)
                {
                    double w = (double)widths[i];

                    PdfHelper.DrawDimensionsX(p, spot, x, y, w);
                    x += w;
                }
            }
        }

        void DrawPage(PDFlib p, PdfPageInfo pageInfo, double xOfs, double yOfs, decimal[] widths)
        {
            for (int i = 0; i < widths.Length - 1; i++)
            {
                xOfs += (double)widths[i] * PdfHelper.mn;
                p.moveto(xOfs, yOfs);
                p.lineto(xOfs, yOfs + pageInfo.Trimbox.height);
                p.stroke();
            }
        }

        private void DrawFalcMarks(PDFlib p, PdfPageInfo pageInfo, int pageIdx)
        {
            var trimbox = pageInfo.Trimbox;

            p.setcolor("fillstroke", "cmyk", 1, 0, 1, 0);
            p.setlinewidth(1.5);

            double x = trimbox.left;
            double y = trimbox.bottom;

            y -= (DISTANCE_FROM_TRIM + MARK_LENGTH) * PdfHelper.mn;

            if (pageIdx % 2 == 0)
            {
                x = pageInfo.Mediabox.width - trimbox.right;
                double xOfs = x;// + boxes.Media.left;

                for (int i = 0; i < _param.PartsWidth.Length - 1; i++)
                {
                    xOfs -= (double)_param.PartsWidth[i] * PdfHelper.mn;

                    DrawHorLines(p, pageInfo, trimbox, xOfs, MARK_LENGTH, DISTANCE_FROM_TRIM);
                }
            }
            else
            {
                double xOfs = x; //+ pageInfo.Mediabox.left;
                for (int i = 0; i < _param.PartsWidth.Length - 1; i++)
                {
                    xOfs += (double)_param.PartsWidth[i] * PdfHelper.mn;

                    DrawHorLines(p, pageInfo, trimbox, xOfs, MARK_LENGTH, DISTANCE_FROM_TRIM);
                }
            }
        }

        private void DrawHorLines(PDFlib p, PdfPageInfo pageInfo, Box box, double xOfs, double markLength, double distanceFromTrim)
        {
            var yOfs = box.bottom - (distanceFromTrim + markLength) * PdfHelper.mn;

            p.moveto(xOfs, yOfs);
            p.lineto(xOfs, yOfs + markLength * PdfHelper.mn);
            p.stroke();

            p.moveto(xOfs, box.bottom + box.height + distanceFromTrim * PdfHelper.mn);
            p.lineto(xOfs, box.bottom + box.height + (distanceFromTrim + markLength) * PdfHelper.mn);
            p.stroke();
        }
    }
}
