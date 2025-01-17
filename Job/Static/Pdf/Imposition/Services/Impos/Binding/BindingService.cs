using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Perfecting;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.WorkAndTurn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Binding
{
    public static class BindingService
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {
            switch (parameters.Sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    return LooseBindingSingleSide.Impos(parameters);
                case TemplateSheetPlaceType.Sheetwise:
                    return LooseBindingSheetwise.Impos(parameters);
                case TemplateSheetPlaceType.WorkAndTurn:
                    return LooseBindingWorkAndTurn.Impos(parameters);
                case TemplateSheetPlaceType.Perfecting:
                    return LooseBindingPerfecting.Impos(parameters);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
