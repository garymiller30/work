using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.SplitSpread
{
    public sealed class PdfSplitterParams
    {
        public double Bleed { get; set; } = 3;
        public int From { get; set; } = 1;
        public int To { get; set; } = 0;
    }
}
