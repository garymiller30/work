using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
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
                foreach (var pageInfo in boxes)
                {
                    p.begin_page_ext(pageInfo.Mediabox.width, pageInfo.Mediabox.height, "");

                    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                    p.set_gstate(gstate);

                    p.setcolor("fillstroke", "cmyk", markColor.C/100, markColor.M/100, markColor.Y/100, markColor.K/100);
                    int spot = p.makespotcolor(markColor.Name);
                    p.setlinewidth(1.0);
                    p.rect(pageInfo.Trimbox.left, pageInfo.Trimbox.bottom, pageInfo.Trimbox.width, pageInfo.Trimbox.height);
                    p.stroke();

                    double xOfs = pageInfo.Trimbox.left;
                    double yOfs = pageInfo.Trimbox.bottom;

                    if (_param.Mirrored) {
                        // Логіка для дзеркальних частин
                        //отримати індекс поточної частини
                        
                        int idx = boxes.IndexOf(pageInfo);
                        if (idx % 2 == 1)
                        {
                            // Логіка для парних частин
                            for (int i = 0; i < _param.PartsWidth.Length-1; i++)
                            {
                                xOfs += (double)_param.PartsWidth[i] *  PdfHelper.mn ;
                                p.moveto(xOfs, yOfs);
                                p.lineto(xOfs, yOfs + pageInfo.Trimbox.height);
                                p.stroke();
                            }
                        }
                        else
                        {
                            // Логіка для непарних частин
                            for (int i = _param.PartsWidth.Length - 1; i > 0; i--)
                            {
                                xOfs += (double)_param.PartsWidth[i] * PdfHelper.mn;
                                p.moveto(xOfs, yOfs);
                                p.lineto(xOfs, yOfs + pageInfo.Trimbox.height);
                                p.stroke();
                            }
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
                    // Тут має бути логіка створення схеми Falc
                    // Використовуйте _param.Mirrored та _param.PartsWidth для налаштування схеми
                    p.end_page_ext("");
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
    }
}
