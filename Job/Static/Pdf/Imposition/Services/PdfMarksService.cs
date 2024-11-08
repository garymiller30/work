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
        public static void RecalcMarkCoordFront(TemplatePageContainer templatePageContainer)
        {
            RectangleD subject = templatePageContainer.GetSubjectRectFront();
            PdfMarksService.RecalcMarkCoordFront(templatePageContainer.Marks, subject);
        }

        public static void RecalcMarkCoordFront(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            PdfMarksService.RecalcMarkCoordFront(sheet.Marks,subject);
        }

        static void RecalcMarkCoordFront(MarksContainer marksContainer, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsFront))
            {
                PositioningService.AnchorToAbsoluteCoordFront(subjectRect, mark);
            }
            marksContainer.Containers.ForEach(y => PdfMarksService.RecalcMarkCoordFront(y,subjectRect));
        }


        public static void RecalcMarkCoordBack(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            PdfMarksService.RecalcMarkCoordBack(sheet.Marks, subject);
        }

        public static void RecalcMarkCoordBack(TemplateSheet sheet, TemplatePageContainer templatePageContainer)
        {
            RectangleD subject = templatePageContainer.GetSubjectRectBack(sheet);
            PdfMarksService.RecalcMarkCoordBack(templatePageContainer.Marks,subject);
        }

        static void RecalcMarkCoordBack(MarksContainer marksContainer, RectangleD subjectRect)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsBack))
            {
                PositioningService.AnchorToAbsoluteCoordBack(subjectRect, mark);
            }

            marksContainer.Containers.ForEach(y => PdfMarksService.RecalcMarkCoordBack(y, subjectRect));
        }
    }
}
