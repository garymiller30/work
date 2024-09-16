using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantRotated : AbstractPlaceVariant
    {
        public PlaceVariantRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            IsRotated = true;
            Calc(Parameters.TemplatePage.H + Parameters.TemplatePage.Bleeds * 2, Parameters.TemplatePage.W + Parameters.TemplatePage.Bleeds * 2);
        }
    }
}
