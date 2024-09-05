using Job.Static.Pdf.Imposition.Drawers.PDF.Sheet;
using Job.Static.Pdf.Imposition.Models;
using Job.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Drawers.PDF
{
    public class PdfDrawer
    {
        public string TargetFile { get; set; }

        public PdfDrawer(string targetFile)
        {
            TargetFile = targetFile;
        }
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

    }
}
