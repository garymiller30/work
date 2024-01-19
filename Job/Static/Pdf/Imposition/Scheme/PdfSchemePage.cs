using Job.Static.Pdf.Imposition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Scheme
{
    public class PdfSchemePage
    {
        public int RowIdx { get;set; }
        public int ColumnIdx { get;set; }
        public Fields Gaps { get;set; } = new Fields();
        public PdfSchemePageSide Front {  get; set; } = new PdfSchemePageSide();
        public PdfSchemePageSide Back { get; set; } = new PdfSchemePageSide();

       
    }
}
