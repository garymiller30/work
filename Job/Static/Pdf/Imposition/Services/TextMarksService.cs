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
        public static void RecalcMarkCoordFront(TemplatePageContainer templateContainer)
        {

            RectangleD subject = templateContainer.GetSubjectRectFront();
            TextMarksService.RecalcMarkCoordFront(templateContainer.Marks,subject);
        }

        public static void RecalcMarkCoordFront(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            TextMarksService.RecalcMarkCoordFront(sheet.Marks,subject);
        }

        static void RecalcMarkCoordFront(MarksContainer marksContainer,RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsFront))
            {
                PositioningService.AnchorToAbsoluteCoordFront(subjectRect, mark);
            }

            marksContainer.Containers.ForEach(y=> TextMarksService.RecalcMarkCoordFront(y,subjectRect));
        }

        public static void RecalcMarkCoordBack(TemplateSheet sheet, TemplatePageContainer templateContainer)
        {
            RectangleD subject = templateContainer.GetSubjectRectBack(sheet);
            TextMarksService.RecalcMarkCoordBack(templateContainer.Marks, subject);
        }

        public static void RecalcMarkCoordBack(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            TextMarksService.RecalcMarkCoordBack(sheet.Marks,subject);
        }

        static void RecalcMarkCoordBack(MarksContainer marksContainer, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsBack))
            {
                PositioningService.AnchorToAbsoluteCoordBack(subjectRect, mark);
            }

            marksContainer.Containers.ForEach(y=> TextMarksService.RecalcMarkCoordBack(y, subjectRect));
        }

    }
}
