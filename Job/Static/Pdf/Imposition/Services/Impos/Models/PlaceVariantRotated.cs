using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantRotated : AbstractPlaceVariant
    {
        public PlaceVariantRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            IsRotated = true;
            var page = bindingParameters.Sheet.MasterPage;
            Calc(page.H + page.Margins.Bottom + page.Margins.Top,
                 page.W + page.Margins.Left + page.Margins.Right);
        }
    }
}
