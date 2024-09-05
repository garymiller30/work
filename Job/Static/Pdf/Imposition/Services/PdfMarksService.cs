using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services
{
    public static class PdfMarksService
    {
        public static void RecalcMarkCoordFront(TemplatePageContainer templatePageContainer)
        {
            RectangleD subject = templatePageContainer.GetSubjectRectFront();
            foreach (var mark in templatePageContainer.Marks.Pdf.Where(x => x.Parameters.IsFront))
            {
                PositioningService.AnchorToAbsoluteCoordFront(subject, mark);
            }
        }

        public static void RecalcMarkCoordFront(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            foreach (var mark in sheet.Marks.Pdf.Where(x => x.Parameters.IsFront))
            {
                PositioningService.AnchorToAbsoluteCoordFront(subject, mark);
            }
        }


        public static void RecalcMarkCoordBack(TemplateSheet sheet)
        {
            RectangleD subject = new RectangleD { X1 = 0, Y1 = 0, X2 = sheet.W, Y2 = sheet.H };
            foreach (var mark in sheet.Marks.Pdf.Where(x => x.Parameters.IsBack))
            {
                PositioningService.AnchorToAbsoluteCoordBack(subject, mark);
            }
        }

        public static void RecalcMarkCoordBack(TemplateSheet sheet, TemplatePageContainer templatePageContainer)
        {
            RectangleD subject = templatePageContainer.GetSubjectRectBack(sheet);
            foreach (var mark in templatePageContainer.Marks.Pdf.Where(x => x.Parameters.IsBack))
            {
                PositioningService.AnchorToAbsoluteCoordBack(subject, mark);
            }
        }


    }
}
