using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSpace.Static.Pdf.Imposition.Drawers.Screen;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenDrawWorkAndTumbleService
    {
        public static Bitmap Draw(TemplateSheet sheet)
        {
            var templateContainer = sheet.TemplatePageContainer;
            Bitmap bitmap = new Bitmap(
                (int)((sheet.W + 1)*ScreenDrawer.ZoomFactor), 
                (int)((sheet.H + 1) * ScreenDrawer.ZoomFactor));

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            ScreenDrawCommons.DrawSheet(sheet, g);
            // draw safe field
            DrawSheetSafeField(g, sheet);
            //draw background marks
            PdfMarksService.RecalcMarkCoordFront(sheet);
            PdfMarksService.RecalcMarkCoordBack(sheet);

            TextMarksService.RecalcMarkCoordFront(sheet);
            TextMarksService.RecalcMarkCoordBack(sheet);

            ScreenDrawSingleSideService.DrawSheetMarksFront(g, sheet, foreground: false, (int)sheet.H);
            ScreenDrawWorkAndTurnService.DrawSheetMarksBack(g, sheet, sheet.Marks, foreground: false, (int)sheet.H);

            // draw pages
            foreach (var page in templateContainer.TemplatePages)
            {
                ScreenDrawWorkAndTurnService.DrawPageFront(g, sheet, page, (int)sheet.H);
                ScreenDrawWorkAndTurnService.DrawPageBack(g, sheet, page, (int)sheet.H);
            }
            ScreenDrawWorkAndTurnService.DrawCropMarks(g, sheet);
            //draw foreground marks
            ScreenDrawSingleSideService.DrawSheetMarksFront(g, sheet, foreground: true, (int)sheet.H);
            ScreenDrawWorkAndTurnService.DrawSheetMarksBack(g, sheet, sheet.Marks, foreground: true, (int)sheet.H);
            g.Dispose();

            return bitmap;
        }

        private static void DrawSheetSafeField(Graphics g, TemplateSheet sheet)
        {
            var max = sheet.SafeFields.GetMaxFieldH();
            if (max > 0)
            {


                // bottom
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = 0,
                    Y = (float)(sheet.H - max),
                    Width = (float)sheet.W,
                    Height = (float)max,
                }, Brushes.LightPink);


                // top
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = 0,
                    Y = 0,
                    Width = (float)sheet.W,
                    Height = (float)max,
                }, Brushes.LightPink);
             
            }

            if (sheet.SafeFields.Left > 0)
            {

                // left
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = 0,
                    Y = 0,
                    Width = (float)sheet.SafeFields.Left,
                    Height = (float)sheet.H,
                }, Brushes.LightPink);
               
            }

            if (sheet.SafeFields.Right > 0)
            {
                //right
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = (float)(sheet.W - sheet.SafeFields.Right),
                    Y = 0,
                    Width = (float)sheet.SafeFields.Right,
                    Height = (float)sheet.H,
                }, Brushes.LightPink);
             
            }


        }
    }
}
