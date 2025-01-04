using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class PdfMarksService
    {
        //public static void RecalcMarkCoordFront(TemplatePageContainer templatePageContainer)
        //{
        //    RectangleD subject = templatePageContainer.GetSubjectRectFront();
        //    PdfMarksService.RecalcMarkCoordFront(templatePageContainer.Marks, subject);
        //}

        public static void RecalcMarkCoordFront(TemplateSheet sheet)
        {
            //Потрібно перерахувати координати міток для листа і сюжету

            RectangleD sheetRect = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            RectangleD subjectRect = sheet.TemplatePageContainer.GetSubjectRectFront();
            PdfMarksService.RecalcMarkCoordFront(sheet.Marks, sheetRect, subjectRect);
        }

        static void RecalcMarkCoordFront(MarksContainer marksContainer, RectangleD sheetRect, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsFront))
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
            marksContainer.Containers.ForEach(y => PdfMarksService.RecalcMarkCoordFront(y, sheetRect, subjectRect));
        }


        public static void RecalcMarkCoordBack(TemplateSheet sheet)
        {
            RectangleD sheetRect = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            RectangleD subjectRect = sheet.TemplatePageContainer.GetSubjectRectBack(sheet);
            PdfMarksService.RecalcMarkCoordBack(sheet.Marks, sheetRect, subjectRect);
        }

        //public static void RecalcMarkCoordBack(TemplateSheet sheet, TemplatePageContainer templatePageContainer)
        //{
        //    RectangleD subject = templatePageContainer.GetSubjectRectBack(sheet);
        //    PdfMarksService.RecalcMarkCoordBack(templatePageContainer.Marks, subject);
        //}

        static void RecalcMarkCoordBack(MarksContainer marksContainer, RectangleD sheetRect, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsBack))
            {
                if (mark.Parent == MarkParentEnum.Sheet)
                {
                    PositioningService.AnchorToAbsoluteCoordBack(sheetRect, mark);
                }
                else
                {
                    PositioningService.AnchorToAbsoluteCoordBack(sheetRect, mark);
                }
            }

            marksContainer.Containers.ForEach(y => PdfMarksService.RecalcMarkCoordBack(y, subjectRect, subjectRect));
        }
    }
}
