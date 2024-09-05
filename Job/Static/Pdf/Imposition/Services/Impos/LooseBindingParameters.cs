using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos
{
    public class LooseBindingParameters
    {
        public TemplateSheet Sheet { get; set; }

        public double PageW { get; set; }
        public double PageH { get; set; }

        public double Bleed { get; set; }

        public bool IsSingleSide { get; set; }

        public double Xofs { get; set; } = 0;
        public double Yofs { get; set; } = 0;

        public bool IsCenterHorizontal { get; set; } = true;
        public bool IsCenterVertical { get; set; } = true;

        public double GetWidthWithBleeds => PageW + Bleed * 2;
        public double GetHeightWithBleeds => PageH + Bleed * 2;


    }
}
