using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos
{
    public class LooseBindingParameters
    {
        public TemplateSheet Sheet { get; set; }

        public double Xofs { get; set; } = 0;
        public double Yofs { get; set; } = 0;

        public bool IsCenterHorizontal { get; set; } = true;
        public bool IsCenterVertical { get; set; } = true;

        public bool IsOneCut { get; set; }

        public BindingPlaceEnum BindingPlace { get; set; } = BindingPlaceEnum.Normal;

    }
}
