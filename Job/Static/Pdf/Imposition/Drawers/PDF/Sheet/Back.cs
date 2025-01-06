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
        public static void Back(PDFlib p, ProductPart impos, PrintSheet sheet)
        {
            DrawerStatic.CurSide = DrawerSideEnum.Back;

            p.begin_page_ext(sheet.W * PdfHelper.mn, sheet.H * PdfHelper.mn, "");

            RecalcBackMarks(sheet);
            // draw background marks
            DrawBackMarks(p, impos, sheet, foreground: false);

            foreach (TemplatePage templatePage in sheet.TemplatePageContainer.TemplatePages)
            {
                // отримати сторінку з ран листа
                int runListPageIdx = templatePage.PrintBackIdx - 1;
                ImposRunPage runPage = impos.RunList.RunPages[runListPageIdx];

                if ((runPage.FileId == 0 && runPage.PageIdx == 0) || templatePage.PrintBackIdx == 0)
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
                        double c_llx = 0;
                        double c_lly = 0;

                        if (pdfFile.IsMediaboxCentered)
                        {
                            c_llx = (pdfPage.Media.W - templatePage.W) / 2 - templatePage.Bleeds.Left;
                            c_lly = (pdfPage.Media.H - templatePage.H) / 2 - templatePage.Bleeds.Bottom;

                        }
                        else
                        {
                            c_llx = pdfPage.Trim.X1 - templatePage.Bleeds.Right - pdfPage.Media.X1;
                            c_lly = pdfPage.Trim.Y1 - templatePage.Bleeds.Bottom - pdfPage.Media.Y1;
                        }

                        double c_urx = c_llx + templatePage.GetPageWidthWithBleeds;
                        double c_ury = c_lly + templatePage.GetPageHeightWithBleeds;

                        (double llx, double lly, double angle) = templatePage.GetPageStartCoordBack(sheet);
                        string clipping_optlist = $"matchbox={{clipping={{{c_llx * PdfHelper.mn} {c_lly * PdfHelper.mn} {c_urx * PdfHelper.mn} {c_ury * PdfHelper.mn}}}}} rotate={angle}";

                        document.fit_pdi_page(pageNo, llx, lly, clipping_optlist);
                    }
                }
                CropMarksService.FixCropMarksBack(sheet);
                DrawCropMarks.Back(p, templatePage);

                Proof.DrawPageBack(p, sheet, templatePage, impos.Proof);
            }

            //draw foreground marks
            DrawBackMarks(p, impos, sheet, foreground: true);
            p.end_page_ext($"mediabox={{{GetMediabox(impos,sheet)}}}");
        }

        private static void DrawBackMarks(PDFlib p, ProductPart impos, PrintSheet sheet, bool foreground)
        {
            DrawPdfMarks.Back(p, sheet.Marks, foreground);
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
