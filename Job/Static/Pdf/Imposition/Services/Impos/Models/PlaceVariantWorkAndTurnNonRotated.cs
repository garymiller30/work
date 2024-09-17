using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos.Models
{
    public class PlaceVariantWorkAndTurnNonRotated : AbstractPlaceVariant
    {
        public PlaceVariantWorkAndTurnNonRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            Calc(Parameters.TemplatePage.W + Parameters.TemplatePage.Margins.Left + Parameters.TemplatePage.Margins.Right, 
                 Parameters.TemplatePage.H + Parameters.TemplatePage.Margins.Bottom + Parameters.TemplatePage.Margins.Top);
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
