using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class TextMarksService
    {
        public static void RecalcMarkCoordFront(TemplateSheet sheet)
        {
            RectangleD sheetRect = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            RectangleD subjectRect = sheet.TemplatePageContainer.GetSubjectRectFront();
            TextMarksService.RecalcMarkCoordFront(sheet.Marks, sheetRect, subjectRect);
        }

        static void RecalcMarkCoordFront(MarksContainer marksContainer, RectangleD sheetRect, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsFront))
            {
                if (mark.Parent == MarkParentEnum.Sheet)
                {
                    PositioningService.AnchorToAbsoluteCoordFront(sheetRect, mark);
                }
                else
                {
                    PositioningService.AnchorToAbsoluteCoordFront(subjectRect, mark);
                }
            }
            marksContainer.Containers.ForEach(y => TextMarksService.RecalcMarkCoordFront(y, sheetRect, subjectRect));
        }
        public static void RecalcMarkCoordBack(TemplateSheet sheet)
        {
            RectangleD sheetRect = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            RectangleD subjectRect = sheet.TemplatePageContainer.GetSubjectRectBack(sheet);
            TextMarksService.RecalcMarkCoordBack(sheet.Marks, sheetRect, subjectRect);
        }

        static void RecalcMarkCoordBack(MarksContainer marksContainer, RectangleD sheetRect, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsBack && x.Enable))
            {
                if (mark.Parent == MarkParentEnum.Sheet)
                {
                    PositioningService.AnchorToAbsoluteCoordBack(sheetRect, mark);
                }
                else
                {
                    PositioningService.AnchorToAbsoluteCoordBack(subjectRect, mark);
                }
            }
            marksContainer.Containers.ForEach(y => TextMarksService.RecalcMarkCoordBack(y, sheetRect, subjectRect));
        }

    }
}
