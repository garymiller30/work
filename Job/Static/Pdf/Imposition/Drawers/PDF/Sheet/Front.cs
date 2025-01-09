using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
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

            RecalcFrontMarks(sheet);

            // draw background marks
            DrawFrontMarks(p, impos, sheet, foreground:false);

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

            // draw foreground marks
            DrawFrontMarks(p, impos, sheet, foreground: true);
            //RecalculateAndDrawMarks(p, sheet, impos);

            p.end_page_ext($"mediabox={{{GetMediabox(impos,sheet)}}}");
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
        
        static string GetMediabox(ProductPart impos,PrintSheet sheet)
        {
            string mediabox;

            if (sheet.TemplatePlate == null)
            {
                mediabox = $"{-sheet.ExtraSpace * PdfHelper.mn} {-sheet.ExtraSpace * PdfHelper.mn} {(sheet.W + sheet.ExtraSpace) * PdfHelper.mn} {(sheet.H + sheet.ExtraSpace) * PdfHelper.mn}";
            }
            else
            {
                double x,y;
                if (sheet.TemplatePlate.IsCenterHorizontal)
                {
                    x = (sheet.TemplatePlate.W - sheet.W) / 2;
                }
                else
                {
                    x = sheet.TemplatePlate.Xofs;
                }

                if (sheet.TemplatePlate.IsCenterVertical)
                {
                    y = (sheet.TemplatePlate.H - sheet.H) / 2;
                }
                else
                {
                    y = sheet.TemplatePlate.Yofs;
                }
                mediabox = $"{-x * PdfHelper.mn} {-y * PdfHelper.mn} {(sheet.TemplatePlate.W - x) * PdfHelper.mn} {(sheet.TemplatePlate.H - y) * PdfHelper.mn}";
            }

            //if (impos.TemplatePlate.IsLikePaperFormat)
            //{
            //    mediabox = $"{-sheet.ExtraSpace * PdfHelper.mn} {-sheet.ExtraSpace * PdfHelper.mn} {(sheet.W + sheet.ExtraSpace) * PdfHelper.mn} {(sheet.H + sheet.ExtraSpace) * PdfHelper.mn}";
            //}
            //else
            //{
            //    double x;
            //    if (impos.TemplatePlate.IsCenterHorizontal)
            //    {
            //        x = (impos.TemplatePlate.W - sheet.W) / 2;
            //    }
            //    else
            //    {
            //        x = impos.TemplatePlate.Xofs;
            //    }

            //    double y;
            //    if (impos.TemplatePlate.IsCenterVertical)
            //    {
            //        y = (impos.TemplatePlate.H - sheet.H) / 2;
            //    }
            //    else
            //    {
            //        y = impos.TemplatePlate.Yofs;
            //    }

            //    mediabox = $"{-x * PdfHelper.mn} {-y * PdfHelper.mn} {(impos.TemplatePlate.W - x) * PdfHelper.mn} {(impos.TemplatePlate.H - y) * PdfHelper.mn}";
            //}

            return mediabox;
        }
    }
}
