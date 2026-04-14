using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantWorkAndTurnRotated : AbstractPlaceVariant
    {
        public PlaceVariantWorkAndTurnRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            IsRotated = true;
            var page = bindingParameters.Sheet.MasterPage;

            Calc(page.H + page.Margins.Bottom + page.Margins.Top,
                 page.W + page.Margins.Left + page.Margins.Right);
        }

        public override (double sheetW, double sheetH) GetSheetFormat()
        {
            var _sheet = Parameters.Sheet;
            double sheetW = _sheet.W / 2 - _sheet.SafeFields.Left;
            double sheetH = _sheet.H - _sheet.SafeFields.Top - _sheet.SafeFields.Bottom;

            return (sheetW, sheetH);
        }
    }
}
