using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantNonRotated : AbstractPlaceVariant
    {
        public PlaceVariantNonRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            var page = bindingParameters.Sheet.MasterPage;
            Calc(page.W + page.Margins.Left + page.Margins.Right,
                 page.H + page.Margins.Bottom + page.Margins.Top);
        }
    }
}
