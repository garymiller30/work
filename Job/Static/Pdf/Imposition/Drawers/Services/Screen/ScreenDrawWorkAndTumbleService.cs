using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenDrawWorkAndTumbleService
    {
        public static Bitmap Draw(TemplateSheet sheet)
        {
            var templateContainer = sheet.TemplatePageContainer;
            Bitmap bitmap = new Bitmap((int)sheet.W + 1, ((int)sheet.H + 1));

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
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = 0,
                    Y = (int)(sheet.H - max),
                    Width = (int)sheet.W,
                    Height = (int)max,
                });

                // top
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = (int)sheet.W,
                    Height = (int)max,
                });
            }

            if (sheet.SafeFields.Left > 0)
            {

                // left
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = (int)sheet.SafeFields.Left,
                    Height = (int)sheet.H,
                });
            }

            if (sheet.SafeFields.Right > 0)
            {
                //right
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = (int)(sheet.W - sheet.SafeFields.Right),
                    Y = 0,
                    Width = (int)sheet.SafeFields.Right,
                    Height = (int)sheet.H,
                });
            }


        }
    }
}
