using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using Org.BouncyCastle.Utilities;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.Falc
{
    public class FalcSchema
    {
        FalcSchemaParams _param;

        public FalcSchema(FalcSchemaParams param)
        {
            _param = param;
        }

        public void Run(string filePath)
        {
            var boxes = PdfHelper.GetPagesInfo(filePath);

            // Логіка створення схеми для Falc на основі boxes та _param
            PDFlib p = new PDFlib();

            try
            {
                string targetfile = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(filePath),
                    System.IO.Path.GetFileNameWithoutExtension(filePath) + "_falc_schema" + System.IO.Path.GetExtension(filePath)
                    );

                MarkColor markColor = MarkColor.ProofColor;

                p.begin_document(targetfile, "optimize=true");

                int doc = -1;
                if (_param.IsMarkFile)
                {
                    doc = p.open_pdi_document(filePath, "");
                }

                foreach (PdfPageInfo pageInfo in boxes)
                {
                    p.begin_page_ext(pageInfo.Mediabox.width, pageInfo.Mediabox.height, "");

                    int idx = boxes.IndexOf(pageInfo);
                    int l_print = p.define_layer("print", "");
                    int v_layer = p.define_layer("visual", "");

                    if (_param.IsMarkFile)
                    {
                        p.begin_layer(l_print);
                        var page_handle = p.open_pdi_page(doc, idx + 1, "");
                        p.fit_pdi_page(page_handle, 0, 0, "");
                        p.close_pdi_page(page_handle);

                        DrawFalcMarks(p, pageInfo, idx);

                        p.end_layer();
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

                    if (_param.Mirrored && idx % 2 == 1)
                    {
                        // Логіка для парних частин
                        for (int i = 0; i < _param.PartsWidth.Length - 1; i++)
                        {
                            xOfs += (double)_param.PartsWidth[i] * PdfHelper.mn;
                            p.moveto(xOfs, yOfs);
                            p.lineto(xOfs, yOfs + pageInfo.Trimbox.height);
                            p.stroke();
                        }
                    }
                    else
                    {
                        // Логіка для звичайних частин
                        for (int i = _param.PartsWidth.Length - 1; i > 0; i--)
                        {
                            xOfs += (double)_param.PartsWidth[i] * PdfHelper.mn;
                            p.moveto(xOfs, yOfs);
                            p.lineto(xOfs, yOfs + pageInfo.Trimbox.height);
                            p.stroke();
                        }
                    }

                    p.end_layer();

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
            finally
            {
                p?.Dispose();
            }
        }

        private void DrawFalcMarks(PDFlib p, PdfPageInfo pageInfo, int pageIdx)
        {
            var trimbox = pageInfo.Trimbox;

            p.setcolor("fillstroke", "cmyk", 1, 0, 1, 0);
            p.setlinewidth(1.5);

            double x = trimbox.left;
            double y = trimbox.bottom;

            double distanceFromTrim = 2;
            double markLength = 2;

            y -= (distanceFromTrim + markLength) * PdfHelper.mn;

            if (pageIdx % 2 == 0)
            {
                x = pageInfo.Mediabox.width - trimbox.right;
                double xOfs = x;// + boxes.Media.left;

                for (int i = 0; i < _param.PartsWidth.Length-1; i++)
                {
                    xOfs -= (double)_param.PartsWidth[i] * PdfHelper.mn;

                    DrawHorLines(p, pageInfo, trimbox, xOfs, markLength, distanceFromTrim);
                }
            }
            else
            {
            double xOfs = x; //+ pageInfo.Mediabox.left;
            for (int i = 0; i < _param.PartsWidth.Length-1; i++)
            {
                xOfs += (double)_param.PartsWidth[i] * PdfHelper.mn;

                DrawHorLines(p, pageInfo, trimbox, xOfs, markLength, distanceFromTrim);
            }
            }

        }

        private void DrawHorLines(PDFlib p, PdfPageInfo pageInfo, Box box,double xOfs, double markLength, double distanceFromTrim)
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
