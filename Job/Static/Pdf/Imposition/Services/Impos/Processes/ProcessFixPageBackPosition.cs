using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Perfecting;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessFixPageBackPosition
    {
        public static void FixPosition(TemplateSheet sheet, TemplatePage page)
        {
            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    LooseBindingSheetwise.FixBackPagePosition(sheet, page);
                    break;
                case TemplateSheetPlaceType.WorkAndTumble:
                   LooseBindingWorkAndTumble.FixBackPagePosition(sheet, page);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public static void FixPosition(TemplateSheet sheet, TemplatePageContainer container)
        {
            foreach (var page in container.TemplatePages)
            {
                FixPosition(sheet, page);
            }
        }
    }
}
