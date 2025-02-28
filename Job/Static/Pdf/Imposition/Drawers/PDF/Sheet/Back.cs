using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using MongoDB.Bson.Serialization.Conventions;
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


        public static void Back(PDFlib p, ProductPart impos, PrintSheet sheet)
        {

            DrawerStatic.CurSide = DrawerSideEnum.Back;

            p.begin_page_ext(sheet.W * PdfHelper.mn, sheet.H * PdfHelper.mn, "");

            RecalcBackMarks(sheet);
            // draw background marks
            DrawBackMarks(p, impos, sheet, foreground: false);

            foreach (TemplatePage templatePage in sheet.TemplatePageContainer.TemplatePages)
            {
                PageSide side = templatePage.Back;


                if (side.PrintIdx != 0)
                {

                    // отримати сторінку з ран листа
                    int runListPageIdx = side.PrintIdx - 1;
                    ImposRunPage runPage = impos.RunList.RunPages[runListPageIdx];

                    if ((runPage.FileId == 0 && runPage.PageIdx == 0) || side.PrintIdx == 0)
                    {
                        // пропускаємо
                    }
                    else
                    {
                        PdfFile pdfFile = impos.GetPdfFile(runPage);
                        PdfFilePage pdfPage = impos.GetPdfPage(runPage);

                        int pageNo = Array.IndexOf(pdfFile.Pages, pdfPage) + 1;

                        using (PDFLIBDocument document = new PDFLIBDocument(p, pdfFile.FileName, ""))
                        {

                            (double c_llx, double c_lly) = GetClippingCoordinatesBack(pdfFile, pdfPage, templatePage);

                            double c_urx = c_llx + templatePage.GetPageWidthWithBleeds;
                            double c_ury = c_lly + templatePage.GetPageHeightWithBleeds;

                            var pd  = ScreenDrawCommons.GetPageDrawBack(sheet,templatePage, side);

                            double llx = pd.page_x - ScreenDrawCommons.GetLeftBleedByAngleBack(sheet,templatePage, side);
                            double lly = pd.page_y - ScreenDrawCommons.GetBottomBleedByAngleBack(sheet,templatePage, side);

                            double angle = side.Angle;
                            
                            string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} orientate={Commons.Orientate[angle]}";
                            document.fit_pdi_page(pageNo, llx, lly, clipping_optlist);
                        }
                    }
                }

                DrawCropMarks.Back(p, templatePage);

                Proof.DrawPageBack(p,sheet, templatePage, templatePage.Back, impos.Proof);
            }

            //draw foreground marks
            DrawBackMarks(p, impos, sheet, foreground: true);
            p.end_page_ext($"mediabox={{{GetMediabox(impos, sheet)}}}");
        }

        private static (double c_llx, double c_lly) GetClippingCoordinatesBack(PdfFile pdfFile, PdfFilePage pdfPage, TemplatePage templatePage)
        {
            if (pdfFile.IsMediaboxCentered)
            {
                return ((pdfPage.Media.W - templatePage.W) / 2 - templatePage.Bleeds.Right,
                    (pdfPage.Media.H - templatePage.H) / 2 - templatePage.Bleeds.Bottom
                    );
            }
            else
            {
                double c_llx = pdfPage.Trim.X1 - templatePage.Bleeds.Right - pdfPage.Media.X1;
                double c_lly = pdfPage.Trim.Y1 - templatePage.Bleeds.Bottom - pdfPage.Media.Y1;
                return (c_llx, c_lly);
            }
        }

        private static void DrawBackMarks(PDFlib p, ProductPart impos, PrintSheet sheet, bool foreground)
        {
            DrawPdfMarks.Back(p, sheet, sheet.Marks, foreground);
            DrawTextMarks.Back(p, sheet.Marks, foreground);
            Proof.DrawSheet(p, sheet, impos.Proof);
        }

        private static void RecalcBackMarks(PrintSheet sheet)
        {
            PdfMarksService.RecalcMarkCoordBack(sheet);
            TextMarksService.RecalcMarkCoordBack(sheet);
        }
    }
}
