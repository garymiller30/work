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
            Calc(Parameters.TemplatePage.W + Parameters.TemplatePage.Margins.Left + Parameters.TemplatePage.Margins.Right,
                 Parameters.TemplatePage.H + Parameters.TemplatePage.Margins.Bottom + Parameters.TemplatePage.Margins.Top);
        }
    }
}
