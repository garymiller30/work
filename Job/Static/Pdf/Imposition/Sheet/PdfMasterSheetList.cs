using Job.Static.Pdf.Imposition.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Sheet
{
    public class PdfMasterSheetList
    {
        PdfProductPart _part;
        public IEnumerable<PdfMasterSheet> Sheets { get;set;} = new List<PdfMasterSheet>();

        public PdfMasterSheetList(PdfProductPart part)
        {
            _part = part;
        }

        internal void AddSheet(PdfMasterSheet sheet)
        {
            Sheets.Append(sheet);
        }
    }
}
