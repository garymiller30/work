using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Screen
{
    public static class ScreenDrawer
    {
        public static double ZoomFactor = 1.0;

        public static Bitmap Draw(TemplateSheet sheet)
        {

            CropMarksService.FixCropMarks(sheet);

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                case TemplateSheetPlaceType.Sheetwise:
                    return ScreenDrawSingleSideService.Draw(sheet);

                case TemplateSheetPlaceType.WorkAndTurn:
                    return ScreenDrawWorkAndTurnService.Draw(sheet);

                case TemplateSheetPlaceType.WorkAndTumble:
                    return ScreenDrawWorkAndTumbleService.Draw(sheet);
                default:
                    throw new NotImplementedException();
            }
        }

        public static void DrawText(Graphics g, string text, PointF point, Font font, Brush brush)
        {
            g.DrawString(text, font, brush, new PointF((float)(point.X * ZoomFactor), (float)(point.Y * ZoomFactor)));
        }

        public static void DrawFillRectangle(Graphics g, RectangleF rect, Brush brush)
        {
            g.FillRectangle(brush, 
                (float)(rect.X*ZoomFactor), 
                (float)(rect.Y*ZoomFactor), 
                (float)(rect.Width*ZoomFactor), 
                (float)(rect.Height*ZoomFactor));
        }

        public static void DrawRectangle(Graphics g, RectangleF rect, Pen pen)
        {
            g.DrawRectangle(pen,
                (float)(rect.X * ZoomFactor),
                (float)(rect.Y * ZoomFactor),
                (float)(rect.Width * ZoomFactor),
                (float)(rect.Height * ZoomFactor));
        }

        public static void DrawLine(Graphics g, PointF p1, PointF p2, Pen pen)
        {
            g.DrawLine(pen,
                new PointF((float)(p1.X * ZoomFactor), (float)(p1.Y * ZoomFactor)),
                new PointF((float)(p2.X * ZoomFactor), (float)(p2.Y * ZoomFactor)));
        }

       public static void DrawImage(Graphics g,Bitmap bitmap,RectangleF rect)
        {
            g.DrawImage(bitmap, new RectangleF
            {
                X = (float)(rect.X * ZoomFactor),
                Y = (float)(rect.Y * ZoomFactor),
                Width = (float)(rect.Width * ZoomFactor),
                Height = (float)(rect.Height * ZoomFactor)
            });
        }

        public static void DrawText(Graphics g, string str, PointF pointF, string font, int fontH)
        {
            g.DrawString(str, new Font(font, (float)(fontH * ZoomFactor)),  Brushes.Yellow, new PointF((float)(pointF.X * ZoomFactor), (float)(pointF.Y * ZoomFactor)));
        }
    }
}
