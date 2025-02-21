using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Sheet
{
    public static partial class DrawSheet
    {
        public static void WorkAndTumble(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            p.begin_page_ext(sheet.W * PdfHelper.mn, sheet.H * PdfHelper.mn, "");

            RecalcFrontMarks(sheet);
            RecalcBackMarks(sheet);
            // draw background marks
            DrawFrontMarks(p, impos, sheet, foreground: false);
            DrawBackMarks(p, impos, sheet, foreground: false);

            foreach (TemplatePage templatePage in sheet.TemplatePageContainer.TemplatePages)
            {
                // отримати сторінку з ран листа
                int runListPageIdx = templatePage.Front.PrintIdx - 1;

                ImposRunPage runPage = impos.RunList.RunPages[runListPageIdx];

                if (runPage.PageIdx == 0 && runPage.FileId == 0) { }
                else
                {
                    PdfFile pdfFile = impos.GetPdfFile(runPage);
                    PdfFilePage pdfPage = impos.GetPdfPage(runPage);

                    int pageNo = Array.IndexOf(pdfFile.Pages, pdfPage) + 1;

                    using (PDFLIBDocument document = new PDFLIBDocument(p, pdfFile.FileName, ""))
                    {
                        double c_llx = 0;
                        double c_lly = 0;

                        if (pdfFile.IsMediaboxCentered)
                        {
                            c_llx = (pdfPage.Media.W - templatePage.W) / 2 - templatePage.Bleeds.Left;
                            c_lly = (pdfPage.Media.H - templatePage.H) / 2 - templatePage.Bleeds.Bottom;
                        }
                        else
                        {
                            c_llx = pdfPage.Trim.X1 - pdfPage.Media.X1 - templatePage.Margins.Left;
                            c_lly = pdfPage.Trim.Y1 - pdfPage.Media.Y1 - templatePage.Margins.Bottom;
                      
                        }

                        double c_urx = c_llx + templatePage.GetPageWidthWithBleeds;
                        double c_ury = c_lly + templatePage.GetPageHeightWithBleeds;

                        double llx = templatePage.Front.X;
                        double lly = templatePage.Front.Y;


                        double angle = templatePage.Front.Angle;

                        string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} orientate={Commons.Orientate[angle]}";

                        document.fit_pdi_page(pageNo, llx, lly, clipping_optlist);

                        runListPageIdx = templatePage.Back.PrintIdx - 1;
                        runPage = impos.RunList.RunPages[runListPageIdx];

                        if ((runPage.FileId == 0 && runPage.PageIdx == 0) || templatePage.Back.PrintIdx == 0)
                        {
                            // пропускаємо
                        }
                        else
                        {
                            pdfFile = impos.GetPdfFile(runPage);
                            pdfPage = impos.GetPdfPage(runPage);
                            pageNo = Array.IndexOf(pdfFile.Pages, pdfPage) + 1;

                            using (PDFLIBDocument documentB = new PDFLIBDocument(p, pdfFile.FileName, ""))
                            {
                                c_llx = 0;
                                c_lly = 0;

                                if (pdfFile.IsMediaboxCentered)
                                {
                                    c_llx = (pdfPage.Media.W - templatePage.W) / 2 - templatePage.Bleeds.Left;
                                    c_lly = (pdfPage.Media.H - templatePage.H) / 2 - templatePage.Bleeds.Bottom;

                                }
                                else
                                {
                                    c_llx = pdfPage.Trim.X1 - templatePage.Margins.Right - pdfPage.Media.X1;
                                    c_lly = pdfPage.Trim.Y1 - templatePage.Margins.Bottom - pdfPage.Media.Y1;

                                 
                                }

                                c_urx = c_llx + templatePage.GetPageWidthWithBleeds;
                                c_ury = c_lly + templatePage.GetPageHeightWithBleeds;

                                llx = templatePage.Back.X;
                                lly = templatePage.Back.Y;

                                angle = templatePage.Back.Angle;
                                //(double llx, double lly, double angle) = templatePage.GetPageStartCoordBack(sheet);
                                clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} orientate={Commons.Orientate[angle]}";
                                //string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} rotate={angle}";

                                documentB.fit_pdi_page(pageNo, llx, lly, clipping_optlist);


                            }
                        }
                    }
                }

                DrawCropMarks.Front(p, templatePage);
                DrawCropMarks.Back(p, templatePage);

                Proof.DrawPage(p, templatePage, templatePage.Front, impos.Proof);
                Proof.DrawPage(p, templatePage, templatePage.Back, impos.Proof);
            }

            // draw foreground marks
            DrawFrontMarks(p, impos, sheet, foreground: true);
            DrawBackMarks(p, impos, sheet, foreground: true);

            p.end_page_ext($"mediabox={{{GetMediabox(impos, sheet)}}}");
        }
    }
}
