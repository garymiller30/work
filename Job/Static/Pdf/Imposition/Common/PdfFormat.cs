using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Common
{
    public class PdfFormat
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Fields Gaps { get; set; } = new Fields();
    }
}
