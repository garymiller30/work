using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenPagePreviewRenderer
    {
        private const int PreviewDpi = 96;
        private static readonly Dictionary<string, Bitmap> PreviewCache = new Dictionary<string, Bitmap>();

        public static bool DrawFront(Graphics g, ProductPart productPart, TemplatePage page, RectangleF rect)
        {
            return Draw(g, productPart, page.Front.AssignedRunPage, page.Front, page.W, page.H, rect);
        }

        public static bool DrawBack(Graphics g, ProductPart productPart, TemplatePage page, RectangleF rect)
        {
            return Draw(g, productPart, page.Back.AssignedRunPage, page.Back, page.W, page.H, rect);
        }

        private static bool Draw(Graphics g, ProductPart productPart, ImposRunPage runPage, PageSide side, double pageW, double pageH, RectangleF rect)
        {
            if (productPart == null || runPage == null || runPage.FileId == 0 || runPage.PageIdx == 0) return false;

            Bitmap preview = GetPreview(productPart, runPage);
            if (preview == null) return false;

            GraphicsState state = g.Save();

            RectangleF scaledRect = new RectangleF(
                (float)(rect.X * ScreenDrawer.ZoomFactor),
                (float)(rect.Y * ScreenDrawer.ZoomFactor),
                (float)(rect.Width * ScreenDrawer.ZoomFactor),
                (float)(rect.Height * ScreenDrawer.ZoomFactor));

            g.SetClip(scaledRect);
            g.TranslateTransform(
                scaledRect.X + scaledRect.Width / 2,
                scaledRect.Y + scaledRect.Height / 2);

            RotateForScreen(g, side.Angle);

            RectangleF imageRect = new RectangleF(
                (float)(-pageW * ScreenDrawer.ZoomFactor / 2),
                (float)(-pageH * ScreenDrawer.ZoomFactor / 2),
                (float)(pageW * ScreenDrawer.ZoomFactor),
                (float)(pageH * ScreenDrawer.ZoomFactor));

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(preview, imageRect);
            g.Restore(state);

            return true;
        }

        private static void RotateForScreen(Graphics g, double angle)
        {
            if (angle == 90)
            {
                g.RotateTransform(270);
            }
            else if (angle == 270)
            {
                g.RotateTransform(90);
            }
            else if (angle == 180)
            {
                g.RotateTransform(180);
            }
        }

        private static Bitmap GetPreview(ProductPart productPart, ImposRunPage runPage)
        {
            PdfFile pdfFile = productPart.PdfFiles.FirstOrDefault(x => x.Id == runPage.FileId);
            if (pdfFile == null) return null;

            string key = $"{pdfFile.FileName}|{runPage.PageIdx}|{PreviewDpi}";
            if (PreviewCache.TryGetValue(key, out Bitmap cached)) return cached;

            try
            {
                Bitmap preview = PdfHelper.RenderByTrimBox(pdfFile.FileName, runPage.PageIdx - 1, PreviewDpi);
                PreviewCache[key] = preview;
                return preview;
            }
            catch
            {
                return null;
            }
        }
    }
}
