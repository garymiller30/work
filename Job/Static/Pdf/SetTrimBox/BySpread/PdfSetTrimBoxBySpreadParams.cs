using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.SetTrimBox.BySpread
{
    public sealed class PdfSetTrimBoxBySpreadParams
    {
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Inside { get; set; }
        public double Outside { get; set; }
    }
}
