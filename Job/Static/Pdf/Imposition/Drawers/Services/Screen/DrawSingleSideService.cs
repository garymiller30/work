using Ghostscript.NET.Rasterizer;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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
            PdfMarksService.RecalcMarkCoordFront(sheet);
            DrawSheetMarksFront(g,sheet, (int)sheet.H);

            PdfMarksService.RecalcMarkCoordFront(sheet.TemplatePageContainer);
            DrawContainerMarksFront(g, sheet.TemplatePageContainer.Marks, (int)sheet.H);

            g.Dispose();

            return bitmap;
        }

        private static void DrawContainerMarksFront(Graphics g, MarksContainer container, int h)
        {
            Brush brush = new SolidBrush(Color.Aqua);
            foreach (var mark in container.Pdf.Where(x => x.Parameters.IsFront && x.Enable))
            {

                Image bitmap = MarksService.GetBitmap(mark);
                var rect = new Rectangle
                {
                    X = (int)mark.Front.X,
                    Y = h - (int)mark.Front.Y - (int)mark.GetH(),
                    Width = (int)mark.GetW(),
                    Height = (int)mark.GetH()
                };

                if (bitmap == null) 
                    {
                    g.FillRectangle(brush,rect );

                }
                else
                {
                    var i = RotateImage(bitmap,(float)mark.Angle);
                    g.DrawImage(i,rect);
                    bitmap.Dispose();
                }
            }
            brush.Dispose();

            container.Containers.ForEach(x=> DrawContainerMarksFront(g,x,h));

        }


        public static Bitmap RotateImage(Image b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                //rotate
                g.RotateTransform(angle);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }


        private static void DrawSheetMarksFront(Graphics g, TemplateSheet sheet, int h)
        {
            DrawSheetPdfMarksFront(g,sheet,h);
        }

        private static void DrawSheetPdfMarksFront(Graphics g, TemplateSheet sheet, int h)
        {
            Brush brush = new SolidBrush(Color.Aqua);
            foreach (var mark in sheet.Marks.Pdf.Where(x=> x.Parameters.IsFront && x.Enable))
            {
                g.FillRectangle(brush, new Rectangle { 
                    X=(int)mark.Front.X,
                    Y=h-(int)mark.Front.Y,
                    Width=(int)mark.GetW(),
                    Height=(int)mark.GetH()
                    });
            }
            brush.Dispose();
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

            DrawBleeds(g, page, sH);
            DrawPageRotateMarker(g, page, rect, sH);
            DrawText(g, sheet, page, sH);
            DrawCropsMark(g, page, sH);
        }

        private static void DrawCropsMark(Graphics g, TemplatePage page, int sH)
        {
            var marks = page.CropMarksController.CropMarks;
            Pen pen = new Pen(Color.Black);

            foreach (var mark in marks.Where(x => x.IsFront))
            {
                Point p1 = new Point
                {
                    X = (int)mark.From.X,
                    Y = sH - (int)(mark.From.Y)
                };
                Point p2 = new Point
                {
                    X = (int)mark.To.X,
                    Y = sH - (int)mark.To.Y
                };
                g.DrawLine(pen, p1, p2);
            }
            pen.Dispose();
        }

        private static void DrawPageRotateMarker(Graphics g, TemplatePage page, Rectangle rect, int sH)
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

        private static void DrawBleeds(Graphics g, TemplatePage page, int sH)
        {
            var brush = new SolidBrush(Color.LightGreen);

            DrawBleedLeft(g, page, sH, brush);
            DrawBleedTop(g, page, sH, brush);
            DrawBleedRight(g, page, sH, brush);
            DrawBleedBottom(g, page, sH, brush);

            brush.Dispose();
        }

        private static void DrawBleedBottom(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {

            if (page.Bleeds.Bottom == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Left);
                    sy = sH - (int)page.GetPageDrawY();
                    sw = (int)(page.Bleeds.Left + page.W + page.Bleeds.Right);
                    sh = (int)(page.Bleeds.Bottom);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() + page.GetPageDrawW());
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH() + page.Bleeds.Right);
                    sw = (int)(page.Bleeds.Bottom);
                    sh = (int)(page.Bleeds.Left + page.W + page.Bleeds.Right);
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Right);
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH() + page.Bleeds.Bottom);
                    sw = (int)(page.Bleeds.Left + page.W + page.Bleeds.Right);
                    sh = (int)(page.Bleeds.Bottom);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Bottom);
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH() + page.Bleeds.Left);
                    sw = (int)(page.Bleeds.Bottom);
                    sh = (int)(page.Bleeds.Left + page.W + page.Bleeds.Right);
                    break;
                default:
                    throw new NotImplementedException();
            }

            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
        }

        private static void DrawBleedRight(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {
            if (page.Bleeds.Right == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() + page.W);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Bleeds.Top);
                    sw = (int)page.Bleeds.Right;
                    sh = (int)(page.H + page.Bleeds.Top + page.Bleeds.Bottom);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Top);
                    sy = sH - (int)(page.GetPageDrawY() + page.GetPageDrawH() + page.Bleeds.Right);
                    sw = (int)(page.H + page.Bleeds.Top + page.Bleeds.Bottom);
                    sh = (int)page.Bleeds.Right;
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Right);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Bleeds.Top);
                    sw = (int)page.Bleeds.Right;
                    sh = (int)(page.H + page.Bleeds.Top + page.Bleeds.Bottom);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Bottom);
                    sy = sH - (int)(page.GetPageDrawY());
                    sw = (int)(page.H + page.Bleeds.Top + page.Bleeds.Bottom);
                    sh = (int)page.Bleeds.Right;
                    break;
                default:
                    throw new NotImplementedException();
            }

            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
        }

        private static void DrawBleedTop(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {
            if (page.Bleeds.Top == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Left);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Bleeds.Top);
                    sw = (int)(page.Bleeds.Left + page.W + page.Bleeds.Right);
                    sh = (int)(page.Bleeds.Top);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Top);
                    sy = sH - (int)(page.GetPageDrawH() + page.GetPageDrawY() + page.Bleeds.Top);
                    sw = (int)(page.Bleeds.Top);
                    sh = (int)(page.GetPageDrawH() + page.Bleeds.Left + page.Bleeds.Right);
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Right);
                    sy = sH - (int)(page.GetPageDrawY());
                    sw = (int)(page.Bleeds.Left + page.W + page.Bleeds.Right);
                    sh = (int)(page.Bleeds.Top);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() + page.GetPageDrawW());
                    sy = sH - (int)(page.GetPageDrawH() + page.GetPageDrawY() + page.Bleeds.Top);
                    sw = (int)(page.Bleeds.Top);
                    sh = (int)(page.GetPageDrawH() + page.Bleeds.Left + page.Bleeds.Right);
                    break;
                default:
                    throw new NotImplementedException();
            }

            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
        }

        private static void DrawBleedLeft(Graphics g, TemplatePage page, int sH, SolidBrush brush)
        {

            if (page.Bleeds.Left == 0) return;

            int sx = 0;
            int sy = 0;
            int sw = 0;
            int sh = 0;

            switch (page.Angle)
            {
                case 0:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Left);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Bleeds.Top);
                    sw = (int)page.Bleeds.Left;
                    sh = (int)(page.H + page.Bleeds.Top + page.Bleeds.Bottom);
                    break;
                case 90:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Top);
                    sy = sH - (int)page.GetPageDrawY();
                    sw = (int)(page.Bleeds.Top + page.GetPageDrawW() + page.Bleeds.Bottom);
                    sh = (int)(page.Bleeds.Left);
                    break;
                case 180:
                    sx = (int)(page.GetPageDrawX() + page.W);
                    sy = sH - (int)(page.H + page.GetPageDrawY() + page.Bleeds.Top);
                    sw = (int)page.Bleeds.Left;
                    sh = (int)(page.H + page.Bleeds.Top + page.Bleeds.Bottom);
                    break;
                case 270:
                    sx = (int)(page.GetPageDrawX() - page.Bleeds.Bottom);
                    sy = sH - (int)(page.GetPageDrawY() + page.Bleeds.Left + page.GetPageDrawH());
                    sw = (int)(page.Bleeds.Top + page.GetPageDrawW() + page.Bleeds.Bottom);
                    sh = (int)(page.Bleeds.Left);
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

            string txt = string.Empty;

            
            if (sheet is PrintSheet)
            {

                if (page.PrintFrontIdx != 0) front = page.PrintFrontIdx;
                if (page.PrintBackIdx != 0) back = page.PrintBackIdx;

                if (page.PrintFrontIdx == 0 && page.PrintBackIdx == 0)
                {
                    txt = "пуста";
                }

                if (page.PrintBackIdx == 0)
                {
                    txt = $"{front}";
                }
                else
                {
                    txt = $"{front}•{back}";
                }
            } else
            if (sheet is TemplateSheet)
            {
                if ((page.MasterFrontIdx == 0 && page.MasterBackIdx == 0))
                {
                    txt = "пуста";
                }
                else if (page.MasterBackIdx == 0)
                {
                    txt = $"{page.MasterFrontIdx}";
                }
                else
                {
                    txt = $"{page.MasterFrontIdx}•{page.MasterBackIdx}";
                }
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

