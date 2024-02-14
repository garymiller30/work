using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Content;
using Job.Static.Pdf.Imposition.Export;
using Job.Static.Pdf.Imposition.Sheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Product
{
    public class PdfProduct
    {
        public string Number { get;set; }

        public string Name { get;set; }
        public string CustomerName { get;set;}

        public string OutputPath { get; set; }

        public List<PdfProductPart> ProductParts { get; set; } = new List<PdfProductPart>();

        public PdfContent Content { get; set; } = new PdfContent();

        public void ExportToPdf(string outputPath)
        {
            OutputPath = outputPath;

            using (EDocument doc = new EDocument(this,OutputPath))
            {
                foreach (var part in ProductParts)
                {
                    foreach (PdfSheetRun sheet in part.SheetRunList.Sheets){

                        using (EPage page = doc.AddPage(sheet))
                        {
                            // TODO: implement
                            page.DrawSubject();
                        }
                    }
                }
            }
        }
    }
}
