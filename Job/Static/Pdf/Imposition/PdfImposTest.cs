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
    public class PdfImposTest
    {
        public void Run()
        {

            string file = @"F:\Jobs\MAYKO\2024\#2024-1-8_MAYKO_SPACE_TEA_Stiker_tester_vse_vkusy\Стикер тестер все вкусы.pdf";

            PdfProduct product = new PdfProduct();

            PdfProductPart part = new PdfProductPart();
            part.MasterPage.Width = 50;
            part.MasterPage.Height = 40;
            part.MasterPage.Bleed = 2;
            //part.MasterPage.Auto = true;

            PdfMasterSheet sheet = new PdfMasterSheet();
            sheet.Width = 640;
            sheet.Height = 450;
            sheet.Gaps.Set(3);

            sheet.SheetWorkStyle = SheetWorkStyleEnum.Sheetwise;
            sheet.SubjectSettings.SubjectPosition.CenterHorizontally = true;
            sheet.SubjectSettings.SubjectPosition.YOffset = 10;

            part.MasterSheets.AddSheet(sheet);

            product.Content.Add(file);

            PdfMasterScheme scheme = new PdfMasterScheme();
            scheme.CreateSchemePages(rows: 1, columns: 1);
            scheme.SchemePages[0, 0].Front.Idx = 1;
            scheme.SchemePages[0, 0].Back.Idx = 2;

            part.MasterSchemes.AddScheme(scheme);

            part.PageRunList.SetPagesCount(2);
            part.PageRunList.AssingPage(1, 1);
            part.PageRunList.AssingPage(2, 2);

            part.SheetRunList.Create(sheet,scheme);
            product.ProductParts.Add(part); // add dat
            product.ExportToPdf(file + "_export.pdf");

        }
    }
}
