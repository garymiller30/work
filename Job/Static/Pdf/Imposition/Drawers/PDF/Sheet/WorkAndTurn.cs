using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
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
        public static void WorkAndTurn(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            p.begin_page_ext(sheet.W * PdfHelper.mn, sheet.H * PdfHelper.mn, "");

            RecalcFrontMarks(sheet);
            RecalcBackMarks(sheet);
            // draw background marks
            DrawFrontMarks(p, impos, sheet, foreground: false);
            DrawBackMarks(p, impos, sheet, foreground: false);

            foreach (TemplatePage templatePage in sheet.TemplatePageContainer.TemplatePages)
            {
                PageSide side = templatePage.Front;
                // отримати сторінку з ран листа
                int runListPageIdx = side.PrintIdx - 1;

                ImposRunPage runPage = impos.RunList.RunPages[runListPageIdx];

                if (runPage.PageIdx == 0 && runPage.FileId == 0) { }
                else
                {
                    PdfFile pdfFile = impos.GetPdfFile(runPage);
                    PdfFilePage pdfPage = impos.GetPdfPage(runPage);

                    int pageNo = Array.IndexOf(pdfFile.Pages, pdfPage) + 1;

                    using (PDFLIBDocument document = new PDFLIBDocument(p, pdfFile.FileName, ""))
                    {
                        var (c_llx, c_lly) = GetClippingCoordinatesFront(pdfFile, pdfPage, templatePage);

                        double c_urx = c_llx + templatePage.GetPageWidthWithBleeds;
                        double c_ury = c_lly + templatePage.GetPageHeightWithBleeds;

                        var pd = ScreenDrawCommons.GetPageDraw(templatePage, side);

                        double llx = pd.page_x - ScreenDrawCommons.GetLeftBleedByAngleFront(templatePage, side);
                        double lly = pd.page_y - ScreenDrawCommons.GetBottomBleedByAngleFront(templatePage, side);

                        double angle = side.Angle;

                        string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} orientate={Commons.Orientate[angle]}";

                        document.fit_pdi_page(pageNo, llx, lly, clipping_optlist);

                        side = templatePage.Back;

                        runListPageIdx = side.PrintIdx - 1;
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
                                (c_llx, c_lly) = GetClippingCoordinatesBack(pdfFile, pdfPage, templatePage);

                                c_urx = c_llx + templatePage.GetPageWidthWithBleeds;
                                c_ury = c_lly + templatePage.GetPageHeightWithBleeds;

                                pd = ScreenDrawCommons.GetPageDrawBack(sheet, templatePage, side);

                                llx = pd.page_x - ScreenDrawCommons.GetLeftBleedByAngleBack(sheet, templatePage, side);
                                lly = pd.page_y - ScreenDrawCommons.GetBottomBleedByAngleBack(sheet, templatePage, side);

                                angle = side.Angle;
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
