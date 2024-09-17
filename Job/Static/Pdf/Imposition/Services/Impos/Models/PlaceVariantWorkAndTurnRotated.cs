using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantWorkAndTurnRotated : AbstractPlaceVariant
    {
        public PlaceVariantWorkAndTurnRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            IsRotated = true;
            Calc(Parameters.TemplatePage.H + Parameters.TemplatePage.Margins.Bottom + Parameters.TemplatePage.Margins.Top,
                 Parameters.TemplatePage.W + Parameters.TemplatePage.Margins.Left + Parameters.TemplatePage.Margins.Right);
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
