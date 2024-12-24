using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
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
        public static void Front(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            DrawerStatic.CurSide = DrawerSideEnum.Front;
            p.begin_page_ext(sheet.W * PdfHelper.mn, sheet.H * PdfHelper.mn, "");

            foreach (TemplatePage templatePage in sheet.TemplatePageContainer.TemplatePages)
            {
                // отримати сторінку з ран листа
                int runListPageIdx = templatePage.PrintFrontIdx -1;

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

                    (double llx, double lly, double angle) = templatePage.GetPageStartCoordFront();
                    string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} rotate={angle}";

                    document.fit_pdi_page(pageNo, llx, lly, clipping_optlist);
                }
               
                
                DrawCropMarks.Front(p, templatePage);

                Proof.DrawPageFront(p, templatePage, impos.Proof);
            }

            RecalculateAndDrawMarks(p, sheet, impos);

            p.end_page_ext($"mediabox={{{GetMediabox(impos,sheet)}}}");
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
                return (
                    pdfPage.Trim.X1 - templatePage.Bleeds.Left - pdfPage.Media.X1,
                    pdfPage.Trim.Y1 - templatePage.Bleeds.Bottom - pdfPage.Media.Y1
                );
            }
        }
        private static bool IsEmptyPage(ImposRunPage runPage, TemplatePage templatePage)
        {
            return (runPage.FileId == 0 && runPage.PageIdx == 0) || templatePage.PrintFrontIdx == 0;
        }

        private static ImposRunPage GetRunPage(ProductPart impos, int runListPageIdx)
        {
            return runListPageIdx < impos.RunList.RunPages.Count
        ? impos.RunList.RunPages[runListPageIdx]
        : new ImposRunPage { FileId = 0, PageIdx = 0 };
        }
        private static void RecalculateAndDrawMarks(PDFlib p, PrintSheet sheet, ProductPart impos)
        {
            PdfMarksService.RecalcMarkCoordFront(sheet);
            DrawPdfMarks.Front(p, sheet.Marks);

            TextMarksService.RecalcMarkCoordFront(sheet);
            DrawTextMarks.Front(p, sheet.Marks);

            PdfMarksService.RecalcMarkCoordFront(sheet.TemplatePageContainer);
            DrawPdfMarks.Front(p, sheet.TemplatePageContainer.Marks);

            TextMarksService.RecalcMarkCoordFront(sheet.TemplatePageContainer);
            DrawTextMarks.Front(p, sheet.TemplatePageContainer.Marks);

            Proof.DrawSheet(p, sheet, impos.Proof);
        }
        static string GetMediabox(ProductPart impos,PrintSheet sheet)
        {
            string mediabox;

            if (impos.TemplatePlate.IsLikePaperFormat)
            {
                mediabox = $"{-sheet.ExtraSpace * PdfHelper.mn} {-sheet.ExtraSpace * PdfHelper.mn} {(sheet.W + sheet.ExtraSpace) * PdfHelper.mn} {(sheet.H + sheet.ExtraSpace) * PdfHelper.mn}";
            }
            else
            {
                double x;
                if (impos.TemplatePlate.IsCenterHorizontal)
                {
                    x = (impos.TemplatePlate.W - sheet.W) / 2;
                }
                else
                {
                    x = impos.TemplatePlate.Xofs;
                }

                double y;
                if (impos.TemplatePlate.IsCenterVertical)
                {
                    y = (impos.TemplatePlate.H - sheet.H) / 2;
                }
                else
                {
                    y = impos.TemplatePlate.Yofs;
                }

                mediabox = $"{-x * PdfHelper.mn} {-y * PdfHelper.mn} {(impos.TemplatePlate.W - x) * PdfHelper.mn} {(impos.TemplatePlate.H - y) * PdfHelper.mn}";
            }

            return mediabox;
        }
    }
}
