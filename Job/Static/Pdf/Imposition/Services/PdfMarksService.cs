using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class PdfMarksService
    {
        public static void RecalcMarkCoordFront(TemplateSheet sheet)
        {
            //Потрібно перерахувати координати міток для листа і сюжету

            RectangleD sheetRect = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            RectangleD subjectRect = ProcessSubject.GetSubjectRect(sheet, sheet.TemplatePageContainer);
            PdfMarksService.RecalcMarkCoordFront(sheet, sheet.Marks, sheetRect, subjectRect);
        }

        static void RecalcMarkCoordFront(TemplateSheet sheet, MarksContainer marksContainer, RectangleD sheetRect, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsFront && x.Enable))
            {
                if (mark.Parent == MarkParentEnum.Sheet)
                {
                    PositioningService.AnchorToAbsoluteCoordFront(sheetRect, mark);
                    PositioningService.CalcClipMarkCoordFront(sheet,sheetRect, subjectRect, mark);
                }
                else
                {
                    PositioningService.AnchorToAbsoluteCoordFront(subjectRect, mark);
                    PositioningService.CalcClipMarkCoordFront(sheet,sheetRect, subjectRect, mark);
                }
            }
            marksContainer.Containers.ForEach(y => PdfMarksService.RecalcMarkCoordFront(sheet,y, sheetRect, subjectRect));
        }


        public static void RecalcMarkCoordBack(TemplateSheet sheet)
        {
            RectangleD sheetRect = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            RectangleD subjectRect = sheet.TemplatePageContainer.GetSubjectRectBack(sheet);
            PdfMarksService.RecalcMarkCoordBack(sheet, sheet.Marks, sheetRect, subjectRect);
        }

        static void RecalcMarkCoordBack(TemplateSheet sheet, MarksContainer marksContainer, RectangleD sheetRect, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsBack && x.Enable))
            {
                if (mark.Parent == MarkParentEnum.Sheet)
                {
                    PositioningService.AnchorToAbsoluteCoordBack(sheetRect, mark);
                    PositioningService.CalcClipMarkCoordBack(sheet, sheetRect, subjectRect, mark);
                }
                else
                {
                    PositioningService.AnchorToAbsoluteCoordBack(subjectRect, mark);
                    PositioningService.CalcClipMarkCoordBack(sheet, sheetRect, subjectRect, mark);
                }
            }

            marksContainer.Containers.ForEach(y => PdfMarksService.RecalcMarkCoordBack(sheet,y, sheetRect, subjectRect));
        }
    }
}
