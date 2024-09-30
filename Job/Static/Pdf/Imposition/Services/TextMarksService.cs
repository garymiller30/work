using JobSpace.Static.Pdf.Imposition.Models;
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

            foreach (var mark in templateContainer.Marks.Text.Where(x => x.Parameters.IsFront))
            {
                PositioningService.AnchorToAbsoluteCoordFront(subject, mark);
            }
        }

        public static void RecalcMarkCoordFront(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };

            foreach (var mark in sheet.Marks.Text.Where(x => x.Parameters.IsFront))
            {
                PositioningService.AnchorToAbsoluteCoordFront(subject, mark);
            }
        }

        public static void RecalcMarkCoordBack(TemplateSheet sheet, TemplatePageContainer templateContainer)
        {
            RectangleD subject = templateContainer.GetSubjectRectBack(sheet);

            foreach (var mark in templateContainer.Marks.Text.Where(x => x.Parameters.IsBack))
            {
                PositioningService.AnchorToAbsoluteCoordBack(subject, mark);
            }

        }

        public static void RecalcMarkCoordBack(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            foreach (var mark in sheet.Marks.Text.Where(x => x.Parameters.IsBack))
            {
                PositioningService.AnchorToAbsoluteCoordBack(subject, mark);
            }
        }

    }
}
