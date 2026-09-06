using Interfaces.Pdf.Imposition;
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
            var angle = page.Front.Angle;

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    page.Front.Angle = (angle + 270) % 360;
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    page.Front.Angle = (angle + 270) % 360;

                    if (page.Front.Angle == 0 || page.Front.Angle == 180)
                    {
                        page.Back.Angle = page.Front.Angle;
                    }
                    else
                    {
                        page.Back.Angle = (angle + 90) % 360;
                    }

                    ProcessFixPageBackPosition.FixPosition(sheet, page);
                    break;
                    
                case TemplateSheetPlaceType.WorkAndTumble:
                    page.Front.Angle = (angle + 270) % 360;

                    if (page.Front.Angle == 0 || page.Front.Angle == 180)
                    {
                        page.Back.Angle = (angle + 90) % 360;
                    }
                    else
                    {
                        page.Back.Angle = page.Front.Angle;
                    }
                    ProcessFixPageBackPosition.FixPosition(sheet, page);
                    break;
            }
        }

        public static void Left(TemplateSheet sheet, TemplatePage page)
        {
            var angle = page.Front.Angle;

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    page.Front.Angle = (angle + 90) % 360;
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    page.Front.Angle = (angle + 90) % 360;

                    if (page.Front.Angle == 0 || page.Front.Angle == 180)
                    {
                        page.Back.Angle = page.Front.Angle;
                    }
                    else
                    {
                        page.Back.Angle = (angle + 270) % 360;
                    }
                    ProcessFixPageBackPosition.FixPosition(sheet, page);
                    break;
                case TemplateSheetPlaceType.WorkAndTumble:
                    page.Front.Angle = (angle + 90) % 360;

                    if (page.Front.Angle == 0 || page.Front.Angle == 180)
                    {
                        page.Back.Angle = (angle + 270) % 360;
                    }
                    else
                    {
                        page.Back.Angle = page.Front.Angle;
                    }
                    ProcessFixPageBackPosition.FixPosition(sheet, page);
                    break;
            }
        }
    }
}
