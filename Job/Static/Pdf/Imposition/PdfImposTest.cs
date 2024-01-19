using Amazon.Runtime.EventStreams;
using BrightIdeasSoftware;
using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Content;
using Job.Static.Pdf.Imposition.Product;
using Job.Static.Pdf.Imposition.Scheme;
using Job.Static.Pdf.Imposition.Sheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition
{
    internal class PdfImposTest
    {
        public void Run()
        {
            PdfProduct product = new PdfProduct();

            PdfProductPart part = new PdfProductPart();
            part.MasterPage.Auto = true;

            PdfSheet sheet = new PdfSheet();
            sheet.Width = 640;
            sheet.Height = 450;
            sheet.Gaps.SetAll(3);

            sheet.SheetWorkStyle = SheetWorkStyleEnum.SingleSide;
            sheet.SubjectSettings.SubjectPosition.CenterHorizontally = true;

            product.Content.Add("test.pdf");

            PdfScheme scheme = new PdfScheme();

            scheme.CreateSchemePages(rows:1,columns:1);
            




        }
    }
}
