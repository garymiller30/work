using Ghostscript.NET.Rasterizer;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text;
using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenDrawSingleSideService
    {
        public static Bitmap Draw(TemplateSheet sheet)
        {
            var zoom = ScreenDrawer.ZoomFactor;

            var templateContainer = sheet.TemplatePageContainer;

            Bitmap bitmap = new Bitmap(
                (int)((sheet.W + 1)*zoom), 
                (int)((sheet.H + 1)*zoom));

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            ScreenDrawCommons.DrawSheet(sheet, g);

            // draw safe field
            DrawSheetSafeField(g, sheet);

            //draw background marks
            PdfMarksService.RecalcMarkCoordFront(sheet);
            TextMarksService.RecalcMarkCoordFront(sheet);

            DrawSheetMarksFront(g, sheet, foreground: false, (int)sheet.H);

            // draw pages
            foreach (var page in templateContainer.TemplatePages)
            {
                DrawPageFront(g, sheet, page, (int)sheet.H);
            }

            //draw foreground marks
            DrawSheetMarksFront(g, sheet, foreground: true, (int)sheet.H);

            g.Dispose();

            return bitmap;
        }

        private static void DrawContainerMarksFront(Graphics g, TemplateSheet sheet, MarksContainer container, bool foreground, int h)
        {
            DrawPdfMarksFront(g,sheet, container, foreground, h);
            DrawTextMarksFront(g, container, foreground, h);

        }

        private static void DrawTextMarksFront(Graphics g, MarksContainer container, bool foreground, int h)
        {
            Brush brush = new SolidBrush(Color.MidnightBlue);
            foreach (var mark in container.Text.Where(x => x.Parameters.IsFront && x.Enable && x.IsForeground == foreground))
            {
                var previewPoints = (mark.FontSize / 72.0) * 25.4;
                Font font = new Font(mark.FontName, (float)(previewPoints* ScreenDrawer.ZoomFactor));
                SizeF size = g.MeasureString(mark.Text, font);
                var state = g.Save();
                g.TranslateTransform(
                    (float)(mark.Front.X*ScreenDrawer.ZoomFactor), 
                    (float)((h - mark.Front.Y - mark.GetH())* ScreenDrawer.ZoomFactor));

                float angle = mark.Angle == 90 || mark.Angle == 270 ? (float)(mark.Angle + 180) : (float)mark.Angle;

                g.RotateTransform((angle));
                g.DrawString(mark.Text, font, brush, 0, 0);
                g.Restore(state);
                font.Dispose();
            }

            brush.Dispose();

            container.Containers.ForEach(x => DrawTextMarksFront(g, x, foreground, h));
        }

        private static void DrawPdfMarksFront(Graphics g, TemplateSheet sheet, MarksContainer container, bool foreground, int h)
        {
            foreach (var mark in container.Pdf.Where(x => x.GetMarkSideFront(sheet.SheetPlaceType) && x.Enable && x.IsForeground == foreground))
            {

                System.Drawing.Image bitmap = MarksService.GetBitmapFront(mark);
                
                var rect = new RectangleF
                {
                    X = (float)mark.Front.X,
                    Y = (float)(h - mark.Front.Y - mark.GetClippedH()),
                    Width = (float)mark.GetClippedW(),
                    Height = (float)mark.GetClippedH()
                };


                if (bitmap == null)
                {
                    Brush brush = new SolidBrush(Color.Aqua);
                    ScreenDrawer.DrawFillRectangle(g,rect,brush);
                    //g.FillRectangle(brush, rect);
                    brush.Dispose();

                }
                else
                {
                    var mc = mark.ClipBoxFront;

                    Rectangle clipRect = new Rectangle(
                        (int)(mc.Left * ScreenDrawer.ZoomFactor), 
                        (int)(mc.Bottom * ScreenDrawer.ZoomFactor), 
                        (int)((mc.Right - mc.Left) * ScreenDrawer.ZoomFactor), 
                        (int)((mc.Top - mc.Bottom) * ScreenDrawer.ZoomFactor));
                    Bitmap croppedBitmap = new Bitmap(
                       (clipRect.Width), 
                       (clipRect.Height)); 

                    using (Graphics gc = Graphics.FromImage(croppedBitmap))
                    {
                        // Draw the cropped section of the original bitmap
                        gc.DrawImage(bitmap, new Rectangle(0, 0,
                            (int)(clipRect.Width),
                            (int)(clipRect.Height)), clipRect, GraphicsUnit.Pixel);

                    }

                    var i = RotateImage(bitmap, (float)mark.Angle);
                    i.MakeTransparent(Color.White);

                    ScreenDrawer.DrawImage(g,i,rect);

                    //g.DrawImage(i, rect);
                    
                    croppedBitmap.Dispose();
                    //cropped.Dispose();
                    i.Dispose();
                    bitmap.Dispose();
                }
            }
            container.Containers.ForEach(x => DrawPdfMarksFront(g, sheet, x, foreground, h));
        }

        public static Bitmap RotateImage(System.Drawing.Image b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);


                // кут залежить від типу друку: чужий/свій зворот
                //rotate
                g.RotateTransform((angle == 0 || angle == 180) ? angle : (angle + 180) % 360);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }


        public static void DrawSheetMarksFront(Graphics g, TemplateSheet sheet, bool foreground, int h)
        {
            DrawPdfMarksFront(g, sheet, sheet.Marks, foreground, h);
            DrawTextMarksFront(g, sheet.Marks, foreground, h);
        }

        private static void DrawSheetSafeField(Graphics g, TemplateSheet sheet)
        {
            

            if (sheet.SafeFields.Left != 0)
                // left
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = 0,
                    Y = 0,
                    Width = (int)(sheet.SafeFields.Left),
                    Height = (int)(sheet.H),
                }, Brushes.LightPink);

            if (sheet.SafeFields.Bottom != 0)
                // bottom

                ScreenDrawer.DrawFillRectangle(g, new Rectangle
                {
                    X = 0,
                    Y = (int)((sheet.H - sheet.SafeFields.Bottom)),
                    Width = (int)(sheet.W),
                    Height = (int)(sheet.SafeFields.Bottom),
                }, Brushes.LightPink);

            if (sheet.SafeFields.Top != 0)

                // top
                ScreenDrawer.DrawFillRectangle(g, new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = (int)(sheet.W),
                    Height = (int)(sheet.SafeFields.Top),
                }, Brushes.LightPink);

            if (sheet.SafeFields.Right != 0)
                //right
                ScreenDrawer.DrawFillRectangle(g, new Rectangle
                {
                    X = (int)((sheet.W - sheet.SafeFields.Right)),
                    Y = 0,
                    Width = (int)(sheet.SafeFields.Right),
                    Height = (int)(sheet.H),
                }, Brushes.LightPink);
        }

        public static void DrawPageFront(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
            ScreenDrawWorkAndTurnService.DrawBleeds(g, page, page.Front, sH);

            (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(page, page.Front);

            double x = page_x;
            double y = sH - page_y - page_h;
            double w = page_w;
            double h = page_h;

            Brush brush;

            if (sheet is PrintSheet printSheet)
            {
                if (page.Front.AssignedRunPage == null)
                {
                    brush = new SolidBrush(Color.LightSlateGray);
                }
                else if (page.Front.AssignedRunPage.IsValidFormat)
                {
                    brush = new SolidBrush(Color.AliceBlue);
                }
                else
                {
                    brush = new SolidBrush(Color.LightCoral);
                }
            }
            else
            {
                brush = new SolidBrush(Color.AliceBlue);
            }

            Pen pen = new Pen(Color.Black);

            var rect = new RectangleF
            {
                X = (float)(x),
                Y = (float)(y),
                Width = (float)(w),
                Height = (float)(h)
            };

            ScreenDrawer.DrawFillRectangle(g, rect, brush);
            ScreenDrawer.DrawRectangle(g, rect, pen);
            brush.Dispose();
            pen.Dispose();

            ScreenDrawCommons.DrawPageRotateMarker(g, page, page.Front, rect, sH);
            DrawTextFront(g, sheet, page, sH);
            DrawCropsMark(g, page, sH);
        }

        private static void DrawCropsMark(Graphics g, TemplatePage page, int sH)
        {
            var marks = page.CropMarksController.CropMarks;
            Pen pen = new Pen(Color.Black);

            foreach (var mark in marks.Where(x => x.IsFront))
            {
                var p1 = new PointF
                {
                    X = (float)mark.From.X,
                    Y = sH - (float)(mark.From.Y)
                };
                var p2 = new PointF
                {
                    X = (float)mark.To.X,
                    Y = sH - (float)mark.To.Y
                };

                ScreenDrawer.DrawLine(g, p1, p2, pen);
                //g.DrawLine(pen, p1, p2);
            }
            pen.Dispose();
        }

        private static void DrawTextFront(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
           var zoom = ScreenDrawer.ZoomFactor;

            var drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            PageSide side = page.Front;

            (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(page, side);

            var x = (float)(page_x + page_w / 2);
            var y = sH - page_y - page_h / 2;
            var state = g.Save();
            g.TranslateTransform((float)(x*zoom), (float)(y*zoom));

            double angle = page.Front.Angle;

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

            var path = new GraphicsPath();
            Font font = new Font("Arial",(float)(12*zoom));
            FontFamily family = font.FontFamily;
            Pen pen = new Pen(Color.Black);

            int front = 0;
            int back = 0;

            string txt = string.Empty;


            if (sheet is PrintSheet)
            {

                if (page.Front.PrintIdx != 0) front = page.Front.PrintIdx;
                if (page.Back.PrintIdx != 0) back = page.Back.PrintIdx;

                if (page.Front.PrintIdx == 0 && page.Back.PrintIdx == 0)
                {
                    txt = "пуста";
                }

                if (page.Back.PrintIdx == 0)
                {
                    txt = $"{front}";
                }
                else
                {
                    txt = $"{front}•{back}";
                }
            }
            else
            if (sheet is TemplateSheet)
            {
                if ((page.Front.MasterIdx == 0 && page.Back.MasterIdx == 0))
                {
                    txt = "пуста";
                }
                else if (page.Back.MasterIdx == 0)
                {
                    txt = $"{page.Front.MasterIdx}";
                }
                else
                {
                    txt = $"{page.Front.MasterIdx}•{page.Back.MasterIdx}";
                }
            }

            path.AddString(txt, family, 0, (float)(12*zoom), new PointF { X = 0, Y = 0 }, drawFormat);
            g.DrawPath(pen, path);
            family.Dispose();
            pen.Dispose();
            font.Dispose();
            g.Restore(state);
        }
        public static Bitmap CreateBitmapInMillimeters(double widthMm, double heightMm, float dpi = 96)
        {
            int widthPx = (int)(widthMm / 25.4 * dpi);
            int heightPx = (int)(heightMm / 25.4 * dpi);
            Bitmap bitmap = new Bitmap(widthPx, heightPx);
            bitmap.SetResolution(dpi, dpi);
            return bitmap;
        }


    }
}

