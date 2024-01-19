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

        public List<PdfSheet> Sheets { get; set; } = new List<PdfSheet>();
        public List<PdfScheme> Schemes { get; set; } = new List<PdfScheme>();
        public PdfContent Content { get; set; } = new PdfContent();
    }
}
