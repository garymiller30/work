using Job.Static.Pdf.Common;
using Job.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using Job.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using Job.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using Job.Static.Pdf.Imposition.Models;
using Job.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Drawers.PDF.Sheet
{
    public static partial class DrawSheet
    {
        public static void Front(PDFlib p, ProductPart impos, int curSheetIdx)
        {
            var sheet = impos.Sheet;

            p.begin_page_ext(sheet.W * PdfHelper.mn, sheet.H * PdfHelper.mn, "");

            foreach (TemplatePage templatePage in sheet.TemplatePageContainer.TemplatePages)
            {
                // отримати сторінку з ран листа
                int runListPageIdx = curSheetIdx * sheet.TemplatePageContainer.GetMaxIdx() + templatePage.FrontIdx - 1;

                ImposRunPage runPage = impos.RunList.RunPages[runListPageIdx];

                PdfFile pdfFile = impos.GetPdfFile(runPage);
                PdfFilePage pdfPage = impos.GetPdfPage(runPage);

                int pageNo = Array.IndexOf(pdfFile.Pages, pdfPage) + 1;

                using (PDFLIBDocument document = new PDFLIBDocument(p, pdfFile.FileName, ""))
                {
                    double c_llx = 0;
                    double c_lly = 0;

                    if (pdfFile.IsMediaboxCentered)
                    {
                        c_llx = (pdfPage.Media.W - templatePage.W) / 2 - templatePage.Clip.Left;
                        c_lly = (pdfPage.Media.H - templatePage.H) / 2 - templatePage.Clip.Bottom;
                    }
                    else
                    {
                        c_llx = pdfPage.Trim.X1 - templatePage.Clip.Left - pdfPage.Media.X1;
                        c_lly = pdfPage.Trim.Y1 - templatePage.Clip.Bottom - pdfPage.Media.Y1;
                    }

                    double c_urx = c_llx + templatePage.GetClippedW;
                    double c_ury = c_lly + templatePage.GetClippedH;

                    (double llx, double lly, double angle) = templatePage.GetPageStartCoordFront();
                    string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} rotate={angle}";

                    document.fit_pdi_page(pageNo, llx, lly, clipping_optlist);

                    CropMarksService.FixCropMarksFront(sheet.TemplatePageContainer);
                    DrawCropMarks.Front(p, templatePage);

                    Proof.DrawPageFront(p, templatePage);
                }
            }

            PdfMarksService.RecalcMarkCoordFront(sheet);
            DrawPdfMarks.Front(p, sheet.Marks);

            TextMarksService.RecalcMarkCoordFront(sheet);
            DrawTextMarks.Front(p, sheet.Marks);

            PdfMarksService.RecalcMarkCoordFront(sheet.TemplatePageContainer);
            DrawPdfMarks.Front(p, sheet.TemplatePageContainer.Marks);

            TextMarksService.RecalcMarkCoordFront(sheet.TemplatePageContainer);
            DrawTextMarks.Front(p, sheet.TemplatePageContainer.Marks);

            string mediabox = $"{-sheet.ExtraSpace * PdfHelper.mn} {-sheet.ExtraSpace * PdfHelper.mn} {(sheet.W + sheet.ExtraSpace) * PdfHelper.mn} {(sheet.H + sheet.ExtraSpace) * PdfHelper.mn}";
            //string trimbox = $"{0} {0} {(sheet.W) * PdfHelper.mn} {(sheet.H) * PdfHelper.mn}";


            Proof.DrawSheet(p, sheet);

            p.end_page_ext($"mediabox={{{mediabox}}}");
        }
    }
}
