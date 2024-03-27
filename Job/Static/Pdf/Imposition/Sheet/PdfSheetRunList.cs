using Job.Static.Pdf.Imposition.Controllers;
using Job.Static.Pdf.Imposition.Product;
using Job.Static.Pdf.Imposition.Scheme;
using Job.Static.Pdf.Imposition.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Sheet
{
    public class PdfSheetRunList
    {
        PdfProductPart _part;
        public List<PdfSheetRun> Sheets { get;set;} = new List<PdfSheetRun>();


        public PdfSheetRunList(PdfProductPart part)
        {
            _part = part;   
        }

        public void Create(PdfMasterSheet masterSheet, PdfMasterScheme scheme)
        {
            PdfSheetRun sheet = new PdfSheetRun(masterSheet);
            PdfSubject subject = PdfImpositionController.PlaceMax(_part,sheet, scheme);
            sheet.Subject = subject;
            Sheets.Add(sheet);

            if (sheet.SheetWorkStyle == Common.SheetWorkStyleEnum.Sheetwise)
            {
                PdfSheetRun sheetBack = new PdfSheetRun(masterSheet) {Side=Common.PdfSideEnum.Back };
                sheetBack.Subject = subject;
                Sheets.Add(sheetBack);
            }
        }
    }
}
