using Job.Static.Pdf.Imposition.Models;
using Job.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise;
using Job.Static.Pdf.Imposition.Services.Impos.Binding.Loose.WorkAndTurn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos.Binding
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
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
