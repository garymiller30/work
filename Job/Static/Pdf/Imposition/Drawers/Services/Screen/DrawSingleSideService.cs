using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class DrawSingleSideService
    {


        public static Bitmap Draw(ProductPart productPart)
        {
            var sheet = productPart.Sheet;
            var templateContainer = sheet.TemplatePageContainer;

            Bitmap bitmap = new Bitmap((int)sheet.W + 1, ((int)sheet.H + 1));

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            // draw sheet
            Pen pen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, (int)sheet.W, (int)sheet.H);

            g.DrawRectangle(pen, rect);
            pen.Dispose();

            // draw safe field
            DrawSheetSafeField(g, sheet);

            // draw pages
            foreach (var page in templateContainer.TemplatePages)
            {
                DrawPage(g, sheet, page, (int)sheet.H);
            }

            g.Dispose();

            return bitmap;

        }


        public static Bitmap Draw(TemplateSheet sheet)
        {
            var templateContainer = sheet.TemplatePageContainer;
            Bitmap bitmap = new Bitmap((int)sheet.W + 1, ((int)sheet.H + 1));

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            // draw sheet
            Pen pen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, (int)sheet.W, (int)sheet.H);

            g.DrawRectangle(pen, rect);
            pen.Dispose();

            // draw safe field
            DrawSheetSafeField(g, sheet);

            // draw pages
            foreach (var page in templateContainer.TemplatePages)
            {
                DrawPage(g, sheet, page, (int)sheet.H);
            }

            g.Dispose();

            return bitmap;
        }

        private static void DrawSheetSafeField(Graphics g, TemplateSheet sheet)
        {
            if (sheet.SafeFields.Left != 0)
                // left
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = (int)sheet.SafeFields.Left,
                    Height = (int)sheet.H,
                });

            if (sheet.SafeFields.Bottom != 0)
                // bottom
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = 0,
                    Y = (int)(sheet.H - sheet.SafeFields.Bottom),
                    Width = (int)sheet.W,
                    Height = (int)sheet.SafeFields.Bottom,
                });

            if (sheet.SafeFields.Top != 0)

                // top
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = (int)sheet.W,
                    Height = (int)sheet.SafeFields.Top,
                });

            if (sheet.SafeFields.Right != 0)
                //right
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = (int)(sheet.W - sheet.SafeFields.Right),
                    Y = 0,
                    Width = (int)sheet.SafeFields.Right,
                    Height = (int)sheet.H,
                });
        }

        static void DrawPage(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
            

            int x = (int)page.GetPageDrawX();
            int y = sH - (int)page.GetPageDrawY() - (int)page.GetPageDrawH();
            int w = (int)page.GetPageDrawW();
            int h = (int)page.GetPageDrawH();

            Brush brush = new SolidBrush(Color.AliceBlue);
            Pen pen = new Pen(Color.Black);

            var rect = new Rectangle
            {
                X = x,
                Y = y,
                Width = w,
                Height = h
            };

            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            brush.Dispose();
            pen.Dispose();

            DrawBleeds(g, page, rect, sH);
            DrawPageRotateMarker(g,page,rect,sH);
            DrawText(g, sheet, page, sH);
            DrawCropsMark(g,page,sH);
        }

        private static void DrawCropsMark(Graphics g, TemplatePage page, int sH)
        {
            var marks = page.CropMarksController.CropMarks;
            Pen pen = new Pen(Color.Black);

            foreach (var mark in marks.Where(x=>x.IsFront))
            {
                Point p1 = new Point
                {
                    X = (int)mark.From.X,
                    Y = sH - (int)(mark.From.Y)
                };
                Point p2 = new Point
                {
                    X = (int) mark.To.X,
                    Y = sH - (int)mark.To.Y
                };
                g.DrawLine(pen,p1,p2);
            }
            pen.Dispose();
        }

        private static void DrawPageRotateMarker(Graphics g, TemplatePage page,Rectangle rect, int sH)
        {
            int dist = 5;
            int height = 7;

            var brush = new SolidBrush(Color.Gray);

            int x = rect.X;
            int y = rect.Y;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = x + dist;
                    sy = y + dist;
                    sw = (int)page.GetPageDrawW() - dist * 2;
                    sh = height;
                    break;
                case 90:
                    sx = x + dist;
                    sy = y + dist;
                    sw = height;
                    sh = (int)page.GetPageDrawH() - dist * 2;
                    break;
                case 180:
                    sx = x + dist;
                    sy = y + (int)page.GetPageDrawH() - dist - height;
                    sw = (int)page.GetPageDrawW() - dist * 2;
                    sh = height;
                    break;
                case 270:

                    sx = x + (int)page.GetPageDrawW() - dist - height;
                    sy = y + dist;
                    sw = height;
                    sh = (int)page.GetPageDrawH() - dist * 2;
                    break;

                default:
                    break;
            }
            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
            brush.Dispose();
        }

        private static void DrawBleeds(Graphics g, TemplatePage page, Rectangle rect, int sH)
        {
            var brush = new SolidBrush(Color.LightGreen);

            DrawBleedLeft(g,page,sH,brush);
            DrawBleedTop(g, page, sH, brush);
            DrawBleedRight(g, page, sH,brush);
            DrawBleedBottom(g, page, sH,brush);
            
            brush.Dispose();
        }

        private static void DrawBleedBottom(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {

            if (page.Margins.Bottom == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;
            
            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Left);
                    sy = sH - (int) page.GetPageDrawY();
                    sw = (int)(page.Margins.Left + page.W + page.Margins.Right);
                    sh = (int)(page.Margins.Bottom);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() + page.GetPageDrawW());
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH() + page.Margins.Right);
                    sw = (int)(page.Margins.Bottom);
                    sh = (int)(page.Margins.Left + page.W + page.Margins.Right);
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Right);
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH() + page.Margins.Bottom);
                    sw = (int)(page.Margins.Left + page.W + page.Margins.Right);
                    sh = (int)(page.Margins.Bottom);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Bottom);
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH() + page.Margins.Left);
                    sw = (int)(page.Margins.Bottom);
                    sh = (int)(page.Margins.Left + page.W + page.Margins.Right);
                    break;
                default:
                    throw new NotImplementedException();
            }

            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
        }

        private static void DrawBleedRight(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {
            if (page.Margins.Right == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() + page.W);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Margins.Top);
                    sw = (int)page.Margins.Right;
                    sh = (int)(page.H + page.Margins.Top + page.Margins.Bottom);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Top);
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH()+ page.Margins.Right );
                    sw = (int)(page.H + page.Margins.Top + page.Margins.Bottom);
                    sh = (int)page.Margins.Right;
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Right);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Margins.Top);
                    sw = (int)page.Margins.Right;
                    sh = (int)(page.H + page.Margins.Top + page.Margins.Bottom);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Bottom);
                    sy = sH - (int)(page.GetPageDrawY());
                    sw = (int)(page.H + page.Margins.Top + page.Margins.Bottom);
                    sh = (int)page.Margins.Right;
                    break;
                default:
                    throw new NotImplementedException();
            }

            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
        }

        private static void DrawBleedTop(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {
            if (page.Margins.Top == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Left);
                    sy = sH - (int)( page.H + page.GetPageDrawY() + page.Margins.Top);
                    sw = (int)(page.Margins.Left + page.W + page.Margins.Right);
                    sh = (int)(page.Margins.Top);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Top);
                    sy = sH - (int)(page.GetPageDrawH() + page.GetPageDrawY() + page.Margins.Top);
                    sw = (int)(page.Margins.Top);
                    sh = (int)(page.GetPageDrawH() + page.Margins.Left + page.Margins.Right);
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Right);
                    sy = sH - (int)(page.GetPageDrawY());
                    sw = (int)(page.Margins.Left + page.W + page.Margins.Right);
                    sh = (int)(page.Margins.Top);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() +page.GetPageDrawW());
                    sy = sH - (int)(page.GetPageDrawH() + page.GetPageDrawY() + page.Margins.Top);
                    sw = (int)(page.Margins.Top);
                    sh = (int)(page.GetPageDrawH() + page.Margins.Left + page.Margins.Right);
                    break;
                default:
                    throw new NotImplementedException();
            }

            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
        }

        private static void DrawBleedLeft(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {
            if (page.Margins.Left == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Left);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Margins.Top);
                    sw = (int)page.Margins.Left;
                    sh = (int)(page.H + page.Margins.Top + page.Margins.Bottom);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Top);
                    sy = sH - (int)page.GetPageDrawY();
                    sw = (int)(page.Margins.Top + page.GetPageDrawW() + page.Margins.Bottom);
                    sh = (int)(page.Margins.Left);
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() + page.W);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Margins.Top);
                    sw = (int)page.Margins.Left;
                    sh = (int)(page.H + page.Margins.Top + page.Margins.Bottom);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() - page.Margins.Bottom);
                    sy = sH - (int)(page.GetPageDrawY() + page.Margins.Left+ page.GetPageDrawH());
                    sw = (int)(page.Margins.Top + page.GetPageDrawW() + page.Margins.Bottom);
                    sh = (int)(page.Margins.Left);
                    break;
                default:
                    throw new NotImplementedException();
            }

            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
        }

        private static void DrawText(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
            var drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var x = (float)(page.GetPageDrawX() + page.GetPageDrawW() / 2);
            var y = sH - page.GetPageDrawH() - page.GetPageDrawY() + page.GetPageDrawH() / 2;
            var state = g.Save();
            g.TranslateTransform(x, (float)y);

            if (page.Angle == 90)
            {
                g.RotateTransform(270);
            }
            else if (page.Angle == 270)
            {
                g.RotateTransform(90);
            }
            else if (page.Angle == 180)
            {
                g.RotateTransform(180);
            }

            var path = new GraphicsPath();
            Font font = new Font("Arial", 12);
            FontFamily family = font.FontFamily;
            Pen pen = new Pen(Color.Black);

            int front = 0;
            int back = 0;

            string txt;

            if (page.FrontIdx == 0 && back == 0)
            {
                txt = "пуста";
            }
            else
            {
                txt = $"{page.FrontIdx}•{page.BackIdx}";
            }


            if (sheet is PrintSheet printSheet)
            {

                if (page.FrontIdx != 0) front = page.FrontIdx + printSheet.RunPageIdx;
                if (page.BackIdx != 0) back = page.BackIdx + printSheet.RunPageIdx;
                txt = $"{front}•{back}";
            }
            path.AddString(txt, family, 0, 12, new PointF { X = 0, Y = 0 }, drawFormat);
            g.DrawPath(pen, path);
            family.Dispose();
            pen.Dispose();
            font.Dispose();
            g.Restore(state);
        }
    }
}

