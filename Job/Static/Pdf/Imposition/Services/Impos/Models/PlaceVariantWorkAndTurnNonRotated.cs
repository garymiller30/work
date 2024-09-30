using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantWorkAndTurnNonRotated : AbstractPlaceVariant
    {
        public PlaceVariantWorkAndTurnNonRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            var page = bindingParameters.Sheet.MasterPage;
            Calc(page.W + page.Margins.Left + page.Margins.Right,
                 page.H + page.Margins.Bottom + page.Margins.Top);
        }

        public override (double sheetW, double sheetH) GetSheetFormat()
        {
            var _sheet = Parameters.Sheet;
            double sheetW = _sheet.W/2 - _sheet.SafeFields.Left;
            double sheetH = _sheet.H - _sheet.SafeFields.Top - _sheet.SafeFields.Bottom;

            return (sheetW, sheetH);
        }
    }
}
