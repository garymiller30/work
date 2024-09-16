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

        public TemplatePage TemplatePage { get; set; }

        public double Xofs { get; set; } = 0;
        public double Yofs { get; set; } = 0;

        public bool IsCenterHorizontal { get; set; } = true;
        public bool IsCenterVertical { get; set; } = true;

    }
}
