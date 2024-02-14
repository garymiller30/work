using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Content;
using Job.Static.Pdf.Imposition.Page;
using Job.Static.Pdf.Imposition.Scheme;
using Job.Static.Pdf.Imposition.Sheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Product
{
    public class PdfProductPart
    {
        public string Name { get; set; }
        public int PageTotal { get; set; }
        public WorkModeEnum WorkMode { get; set; } = WorkModeEnum.Imposition;

        public PdfMasterPage MasterPage { get; set; } = new PdfMasterPage();

        public PdfMasterSheetList MasterSheets { get; set; }
        public PdfSchemeList MasterSchemes { get; set; }
    
        public PdfPageRunList PageRunList { get; set; }
        public PdfSheetRunList SheetRunList { get; set; }   

        public PdfProductPart()
        {
            MasterSheets = new PdfMasterSheetList(this);
            MasterSchemes = new PdfSchemeList(this);
            PageRunList = new PdfPageRunList(this);
            SheetRunList = new PdfSheetRunList(this);
        }
    }
}
