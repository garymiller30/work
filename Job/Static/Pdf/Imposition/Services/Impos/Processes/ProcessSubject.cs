using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessSubject
    {
        public static RectangleD GetSubjectRect(TemplateSheet sheet)
        {
            return new RectangleD
            {
                X1 = sheet.SafeFields.Left,
                Y1 = sheet.SafeFields.Bottom,
                X2 = sheet.W - sheet.SafeFields.Right,
                Y2 = sheet.H - sheet.SafeFields.Top,
            };
        }

        public static RectangleD GetSubjectRect(TemplateSheet sheet, TemplatePageContainer pageContainer)
        {
            var tp = pageContainer.TemplatePages;
            return GetSubjectRect(sheet, tp);
        }

        public static RectangleD GetSubjectRect(TemplateSheet sheet, List<TemplatePage> tp)
        {
            if (tp.Count == 0) return new RectangleD();

            double x1 = tp.Min(x => x.Front.X);
            double y1 = tp.Min(x => x.Front.Y);

            double x2, y2;


            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                case TemplateSheetPlaceType.Sheetwise:
                    x2 = tp.Max(x => x.Front.X + x.GetClippedWByRotate());
                    y2 = tp.Max(x => x.Front.Y + x.GetClippedHByRotate());
                    break;
                case TemplateSheetPlaceType.WorkAndTurn:
                case TemplateSheetPlaceType.WorkAndTumble:
                    x2 = tp.Max(x => x.Back.X + x.GetClippedWByRotate());
                    y2 = tp.Max(x => x.Back.Y + x.GetClippedHByRotate());
                    break;
                default:
                    throw new Exception("Unknown sheet place type");
            }


            return new RectangleD { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
        }
    }
}
