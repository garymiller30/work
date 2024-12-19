using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Models;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Sheet;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF
{
    public class PdfDrawer
    {

        public EventHandler<int> StartEvent { get; set; } = delegate { };
        public EventHandler<int> ProcessingEvent { get; set; } = delegate { };
        public EventHandler FinishEvent { get; set; } = delegate { };
        public string TargetFile { get; set; }

        public PdfDrawer(string targetFile)
        {
            TargetFile = targetFile;
        }

        public void Draw(ProductPart impos)
        {
            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(TargetFile, "");

                StartEvent(this, impos.PrintSheets.Count);

                for (int i = 0; i < impos.PrintSheets.Count; i++)
                {

                    ProcessingEvent(this, i + 1);

                    var sheet = impos.PrintSheets[i];

                    TextVariablesService.SetValue(ValueList.SheetIdx, i + 1);

                    TextVariablesService.SetValue(ValueList.SheetFormat, $"{sheet.W}x{sheet.H}");
                    TextVariablesService.SetValue(ValueList.CurDate, DateTime.Now.ToString());

                    switch (sheet.SheetPlaceType)
                    {
                        case TemplateSheetPlaceType.SingleSide:
                            TextVariablesService.SetValue(ValueList.SheetSide, "Без звороту");
                            DrawSheet.Front(p, impos, sheet);
                            break;

                        case TemplateSheetPlaceType.Sheetwise:

                            TextVariablesService.SetValue(ValueList.SheetSide, "Лице");
                            DrawSheet.Front(p, impos, sheet);
                            TextVariablesService.SetValue(ValueList.SheetSide, "Зворот");
                            DrawSheet.Back(p, impos, sheet);
                            break;

                        case TemplateSheetPlaceType.WorkAndTurn:
                            TextVariablesService.SetValue(ValueList.SheetSide, "Свій зворот");
                            DrawSheet.WorkAndTurn(p, impos, sheet);
                            break;

                        case TemplateSheetPlaceType.Perfecting:
                            throw new NotImplementedException();
                        default:
                            throw new NotImplementedException();
                    }

                }

                p.end_document("");
            }
            catch (PDFlibException e)
            {
            }
            finally
            {
                p?.Dispose();

                FinishEvent(this, null);
            }





        }

        void DrawPages(List<PdfPage> pages)
        {
            if (pages.Count == 0) throw new Exception("No Pages");

            PDFlib p = new PDFlib();
            try
            {
                p.begin_document(TargetFile, "");

                foreach (PdfPage page in pages)
                {
                    p.begin_page_ext(page.W * PdfHelper.mn, page.H * PdfHelper.mn, "");



                    p.end_page_ext("");
                }

                p.end_document("");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                p?.Dispose();
            }
        }
        /*
        public void Draw(ProductPart impos)
        {

            var sheet = impos.Sheet;
            var templateContainer = sheet.TemplatePageContainer;
            var runList = impos.RunList;

            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(TargetFile, "");

                // потрібно вичислити кількість листів
                // для цього потрібно взяти кількість сторінок в ран листі і поділити на  максимальний індекс сторінки шаблону 

                int maxIdx = templateContainer.GetMaxIdx();
                int cSheets = runList.RunPages.Count / maxIdx;


                for (int sheetIdx = 0; sheetIdx < cSheets; sheetIdx++)
                {
                    TextVariablesService.SetValue(ValueList.SheetIdx, sheetIdx + 1);
                    TextVariablesService.SetValue(ValueList.SheetSide, "Лице");
                    TextVariablesService.SetValue(ValueList.SheetFormat,$"{sheet.W}x{sheet.H}");
                    TextVariablesService.SetValue(ValueList.CurDate,DateTime.Now.ToString());

                    DrawSheet.Front(p, impos, sheetIdx);

                    // малюємо зворот
                    if (templateContainer.HasBack())
                    {
                        TextVariablesService.SetValue(ValueList.SheetSide, "Зворот");
                        DrawSheet.Back(p, impos, sheetIdx);
                    }
                }
                p.end_document("");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                p?.Dispose();
            }
        }
        */
    }
}
