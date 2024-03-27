using Job.Static.Pdf.Imposition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Page
{
    public class PdfMasterPage
    {
        public bool Auto { get; set; }

        public double Width {  get; set; }
        public double Height { get; set; }
        public double Bleed { get; set; }
        public bool IsUseCustomBleeds { get;set; }
        public Fields CustomBleed { get;set; } = new Fields();
    }
}
