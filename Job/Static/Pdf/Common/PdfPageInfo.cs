using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Common
{
    public class PdfPageInfo
    {
        public Box Mediabox { get;set; }
        public Box Trimbox { get;set; }
        public double Rotate { get;set; } = 0.0;
    }
}
