using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessRotatePage
    {
        public static void Right(TemplateSheet sheet, TemplatePage page)
        {
            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    page.Front.Angle = (page.Front.Angle + 270) % 360;
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    page.Front.Angle = (page.Front.Angle + 270) % 360;
                    page.Back.Angle = (page.Front.Angle + 180) % 360;
                    break;
                    
                case TemplateSheetPlaceType.WorkAndTumble:
                    break;
                default:
                    throw new Exception("Unknown sheet place type");
            }
        }

        public static void Left(TemplateSheet sheet, TemplatePage page)
        {
            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    page.Front.Angle = (page.Front.Angle + 90) % 360;
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    page.Front.Angle = (page.Front.Angle + 90) % 360;
                    page.Back.Angle = (page.Front.Angle + 180) % 360;
                    break;
                case TemplateSheetPlaceType.WorkAndTumble:
                    break;
                default:
                    throw new Exception("Unknown sheet place type");
            }
        }
    }
}
