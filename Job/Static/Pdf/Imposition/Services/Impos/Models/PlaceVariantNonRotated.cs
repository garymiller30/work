using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantNonRotated : AbstractPlaceVariant
    {
        public PlaceVariantNonRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            Calc(Parameters.TemplatePage.W + Parameters.TemplatePage.Bleeds * 2, Parameters.TemplatePage.H + Parameters.TemplatePage.Bleeds * 2);
        }
    }
}
