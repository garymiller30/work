using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
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
        public static void Front(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            DrawerStatic.CurSide = DrawerSideEnum.Front;
            p.begin_page_ext(sheet.W * PdfHelper.mn, sheet.H * PdfHelper.mn, "");

            RecalcFrontMarks(sheet);

            // draw background marks
            DrawFrontMarks(p, impos, sheet, foreground: false);

            foreach (TemplatePage templatePage in sheet.TemplatePageContainer.TemplatePages)
            {
                // отримати сторінку з ран листа
                int runListPageIdx = templatePage.Front.PrintIdx - 1;

                //ImposRunPage runPage;
                var runPage = GetRunPage(impos, runListPageIdx);

                if (IsEmptyPage(runPage, templatePage))
                {
                    continue;
                }

                PdfFile pdfFile = impos.GetPdfFile(runPage);
                PdfFilePage pdfPage = impos.GetPdfPage(runPage);

                int pageNo = Array.IndexOf(pdfFile.Pages, pdfPage) + 1;

                using (PDFLIBDocument document = new PDFLIBDocument(p, pdfFile.FileName, ""))
                {
                    var (c_llx, c_lly) = GetClippingCoordinates(pdfFile, pdfPage, templatePage);

                    double c_urx = c_llx + templatePage.GetPageWidthWithBleeds;
                    double c_ury = c_lly + templatePage.GetPageHeightWithBleeds;

                    double llx = templatePage.Front.X;
                    double lly = templatePage.Front.Y;


                    var margins = templatePage.Margins;
                    var bleeds = templatePage.Bleeds;

                    switch (templatePage.Front.Angle)
                    {
                        case 0:
                            if (margins.Left < bleeds.Left)     llx = llx - (bleeds.Left - margins.Left);
                            if (margins.Bottom < bleeds.Bottom) lly = lly - (bleeds.Bottom - margins.Bottom);

                            break;
                        case 90:

                            if (margins.Top <bleeds.Top) llx = llx - margins.Top - bleeds.Top;
                            if (margins.Left < bleeds.Left) lly = lly - margins.Left - bleeds.Left;
                            break;
                        case 180:
                             if (margins.Right < bleeds.Right) llx = llx - margins.Right - bleeds.Right;
                             if (margins.Top < bleeds.Top) lly = lly - margins.Top - bleeds.Top;
                            break;
                        case 270:
                            if (margins.Bottom < bleeds.Bottom) llx = llx - margins.Bottom - bleeds.Bottom;
                            if (margins.Right < bleeds.Right) lly = lly - margins.Right - bleeds.Right;
                            break;
                        default:
                            throw new NotImplementedException();
                    }


                    double angle = templatePage.Front.Angle;
                    string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} orientate={Commons.Orientate[angle]}";
                    document.fit_pdi_page(pageNo, llx, lly, clipping_optlist);
                }
                DrawCropMarks.Front(p, templatePage);
                Proof.DrawPage(p, templatePage, templatePage.Front, impos.Proof);
            }

            // draw foreground marks
            DrawFrontMarks(p, impos, sheet, foreground: true);

            p.end_page_ext($"mediabox={{{GetMediabox(impos, sheet)}}}");
        }

        private static void DrawFrontMarks(PDFlib p, ProductPart impos, TemplateSheet sheet, bool foreground)
        {
            DrawPdfMarks.Front(p, sheet.Marks, foreground);
            DrawTextMarks.Front(p, sheet.Marks, foreground);
            Proof.DrawSheet(p, sheet, impos.Proof);
        }

        private static void RecalcFrontMarks(PrintSheet sheet)
        {
            PdfMarksService.RecalcMarkCoordFront(sheet);
            TextMarksService.RecalcMarkCoordFront(sheet);
        }

        private static (double c_llx, double c_lly) GetClippingCoordinates(PdfFile pdfFile, PdfFilePage pdfPage, TemplatePage templatePage)
        {
            if (pdfFile.IsMediaboxCentered)
            {
                return (
                    (pdfPage.Media.W - templatePage.W) / 2 - templatePage.Bleeds.Left,
                    (pdfPage.Media.H - templatePage.H) / 2 - templatePage.Bleeds.Bottom
                );
            }
            else
            {
                double c_llx = pdfPage.Trim.X1 - pdfPage.Media.X1 - templatePage.Bleeds.Left;
                double c_lly = pdfPage.Trim.Y1 - pdfPage.Media.Y1 - templatePage.Bleeds.Bottom;
                //c_llx -= templatePage.Bleeds.Left;
                //c_lly -= templatePage.Bleeds.Bottom;

                if (c_llx < 0) c_llx = 0;
                if (c_lly < 0) c_lly = 0;

                return (c_llx, c_lly);
            }
        }
        private static bool IsEmptyPage(ImposRunPage runPage, TemplatePage templatePage)
        {
            return (runPage.FileId == 0 && runPage.PageIdx == 0) || templatePage.Front.PrintIdx == 0;
        }

        private static ImposRunPage GetRunPage(ProductPart impos, int runListPageIdx)
        {
            return runListPageIdx < impos.RunList.RunPages.Count
        ? impos.RunList.RunPages[runListPageIdx]
        : new ImposRunPage { FileId = 0, PageIdx = 0 };
        }

        static string GetMediabox(ProductPart impos, PrintSheet sheet)
        {
            string mediabox;

            if (sheet.TemplatePlate == null)
            {
                mediabox = $"{-sheet.ExtraSpace * PdfHelper.mn} {-sheet.ExtraSpace * PdfHelper.mn} {(sheet.W + sheet.ExtraSpace) * PdfHelper.mn} {(sheet.H + sheet.ExtraSpace) * PdfHelper.mn}";
            }
            else
            {
                double x = 0;
                double y = 0;

                if (sheet.TemplatePlate.IsCenterHorizontal)
                {
                    x = (sheet.TemplatePlate.W - sheet.W) / 2;
                }

                x += sheet.TemplatePlate.Xofs;

                if (sheet.TemplatePlate.IsCenterVertical)
                {
                    y = (sheet.TemplatePlate.H - sheet.H) / 2;
                }

                y += sheet.TemplatePlate.Yofs;

                mediabox = $"{-x * PdfHelper.mn} {-y * PdfHelper.mn} {(sheet.TemplatePlate.W - x) * PdfHelper.mn} {(sheet.TemplatePlate.H - y) * PdfHelper.mn}";
            }
            return mediabox;
        }
    }
}
