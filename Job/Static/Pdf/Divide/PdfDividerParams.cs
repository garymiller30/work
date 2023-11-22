using Job.Static.Pdf.Common;
using PDFManipulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Divide
{
    public class PdfDividerParams
    {
        public DivideModeEnum Mode { get; set; } = DivideModeEnum.FixedCountPages;
        public int FixedCountPages { get; set; } = 1;
        public int[] CustomCountPages { get; set; }
    }
}
