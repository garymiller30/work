using Job.Static.Pdf.Imposition.Drawers.PDF.Sheet;
using Job.Static.Pdf.Imposition.Models;
using Job.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Drawers.Services
{
    public static class DrawSheetwiseService
    {
        public static void Draw(ProductPart productPart, string targetFile)
        {
            var sheet = productPart.Sheet;
            var templateContainer = sheet.TemplatePageContainer;
            var runList = productPart.RunList;

            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(targetFile, "");

                // потрібно вичислити кількість листів
                // для цього потрібно взяти кількість сторінок в ран листі і поділити на  максимальний індекс сторінки шаблону 

                int maxIdx = templateContainer.GetMaxIdx();
                int cSheets = runList.RunPages.Count / maxIdx;

                for (int sheetIdx = 0; sheetIdx < cSheets; sheetIdx++)
                {
                    DrawSheet.Front(p, productPart, sheetIdx);
                    DrawSheet.Back(p, productPart, sheetIdx);
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
