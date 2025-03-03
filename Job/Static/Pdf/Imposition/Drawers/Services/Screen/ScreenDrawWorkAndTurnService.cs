using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenDrawWorkAndTurnService
    {
        public static Bitmap Draw(TemplateSheet sheet)
        {
            var templateContainer = sheet.TemplatePageContainer;
            Bitmap bitmap = new Bitmap(
                (int)((sheet.W + 1) * ScreenDrawer.ZoomFactor),
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
            DrawSheetMarksBack(g, sheet, sheet.Marks, foreground: false, (int)sheet.H);

            // draw pages
            foreach (var page in templateContainer.TemplatePages)
            {
                DrawPageFront(g, sheet, page, (int)sheet.H);
                DrawPageBack(g, sheet, page, (int)sheet.H);
            }
            DrawCropMarks(g, sheet);
            //draw foreground marks
            ScreenDrawSingleSideService.DrawSheetMarksFront(g, sheet, foreground: true, (int)sheet.H);
            DrawSheetMarksBack(g, sheet, sheet.Marks, foreground: true, (int)sheet.H);
            g.Dispose();

            return bitmap;
        }

        public static void DrawSheetMarksBack(Graphics g, TemplateSheet sheet, MarksContainer container, bool foreground, int h)
        {
            DrawPdfMarkBack(g, sheet, container, foreground, h);
            DrawTextMarkBack(g, sheet, container, foreground, h);
        }

        private static void DrawTextMarkBack(Graphics g, TemplateSheet sheet, MarksContainer container, bool foreground, int h)
        {

        }

        private static void DrawPdfMarkBack(Graphics g, TemplateSheet sheet, MarksContainer container, bool foreground, int h)
        {
            foreach (var mark in container.Pdf.Where(x => x.GetMarkSideBack(sheet.SheetPlaceType) && x.Enable && x.IsForeground == foreground))
            {
                System.Drawing.Image bitmap = MarksService.GetBitmapFront(mark);

                var rect = new RectangleF
                {
                    X = (float)mark.Back.X,
                    Y = (float)(h - mark.Back.Y - mark.GetClippedH()),
                    Width = (float)mark.GetClippedW(),
                    Height = (float)mark.GetClippedH()
                };

                if (bitmap == null)
                {
                    Brush brush = new SolidBrush(Color.Aqua);
                    
                    ScreenDrawer.DrawFillRectangle(g, rect, brush);
                    //g.FillRectangle(brush, rect);
                    brush.Dispose();

                }
                else
                {
                    var mc = mark.ClipBoxBack;

                    Rectangle clipRect = new Rectangle(
                        (int)(mc.Left * ScreenDrawer.ZoomFactor), 
                        (int)(mc.Bottom * ScreenDrawer.ZoomFactor), 
                        (int)((mc.Right - mc.Left) * ScreenDrawer.ZoomFactor), 
                        (int)((mc.Top - mc.Bottom) * ScreenDrawer.ZoomFactor));
                    Bitmap croppedBitmap = new Bitmap(clipRect.Width, clipRect.Height);

                    using (Graphics gc = Graphics.FromImage(croppedBitmap))
                    {
                        // Draw the cropped section of the original bitmap
                        gc.DrawImage(bitmap, new Rectangle(0, 0, clipRect.Width, clipRect.Height), clipRect, GraphicsUnit.Pixel);

                    }

                    var i = ScreenDrawSingleSideService.RotateImage(bitmap, (float)mark.GetBackAngle(sheet.SheetPlaceType));
                    i.MakeTransparent(Color.White);

                    ScreenDrawer.DrawImage(g, i, rect);
                    //g.DrawImage(i, rect);
                    croppedBitmap.Dispose();
                    //cropped.Dispose();
                    i.Dispose();
                    bitmap.Dispose();
                }
            }

            container.Containers.ForEach(x => DrawPdfMarkBack(g, sheet, x, foreground, h));
        }

        public static void DrawCropMarks(Graphics g, TemplateSheet sheet)
        {
            Pen pen = new Pen(Color.Black);
            foreach (var page in sheet.TemplatePageContainer.TemplatePages)
            {
                var marks = page.CropMarksController.CropMarks;


                foreach (var mark in marks)
                {
                    Point p1 = new Point
                    {
                        X = (int)mark.From.X,
                        Y = (int)sheet.H - (int)(mark.From.Y)
                    };
                    Point p2 = new Point
                    {
                        X = (int)mark.To.X,
                        Y = (int)sheet.H - (int)mark.To.Y
                    };

                    ScreenDrawer.DrawLine(g, p1, p2, pen);
                    //g.DrawLine(pen, p1, p2);
                }

            }
            pen.Dispose();
        }

        public static void DrawPageBack(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
            DrawBleeds(g, page, page.Back, sH);

            double x = page.GetPageDrawBackX();
            double y = sH - page.GetPageDrawBackY() - page.GetPageDrawBackH();
            double w = page.GetPageDrawBackW();
            double h = page.GetPageDrawBackH();

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
                X = (float)x,
                Y = (float)y,
                Width = (float)w,
                Height = (float)h
            };

            ScreenDrawer.DrawFillRectangle(g, rect, brush);
            ScreenDrawer.DrawRectangle(g, rect, pen);

           
            brush.Dispose();
            pen.Dispose();


            ScreenDrawCommons.DrawPageRotateMarker(g, page, page.Back, rect, sH);
            DrawTextBack(g, sheet, page, sH);

        }

        public static void DrawBleeds(Graphics g, TemplatePage page, PageSide side, int sH)
        {
            var brush = new SolidBrush(Color.LightGreen);

            (RectangleD left, RectangleD right, RectangleD top, RectangleD bottom) = ScreenDrawCommons.GetDrawBleedsFront(page);

            var rect_left = new RectangleF
            {
                X = (float)(left.X1),
                Y = (float)((sH - left.Y1 - left.H)),
                Width = (float)(left.W),
                Height = (float)(left.H)
            };

            ScreenDrawer.DrawFillRectangle(g, rect_left, brush);

            //g.FillRectangle(brush, rect_left);

            var rect_right = new RectangleF
            {
                X = (float)(right.X1),
                Y = (float)((sH - right.Y1 - right.H)),
                Width = (float)(right.W),
                Height = (float)(right.H)
            };
            ScreenDrawer.DrawFillRectangle(g, rect_right, brush);
            //g.FillRectangle(brush, rect_right);

            var rect_top = new RectangleF
            {
                X = (float)(top.X1),
                Y = (float)((sH - top.Y1 - top.H)),
                Width = (float)(top.W),
                Height = (float)(top.H)
            };
            ScreenDrawer.DrawFillRectangle(g, rect_top, brush);

            //g.FillRectangle(brush, rect_top);

            var rect_bottom = new RectangleF
            {
                X = (float)(bottom.X1),
                Y = (float)((sH - bottom.Y1 - bottom.H)),
                Width = (float)(bottom.W),
                Height = (float)(bottom.H)
            };
            ScreenDrawer.DrawFillRectangle(g, rect_bottom, brush);
            //g.FillRectangle(brush, rect_bottom);


            //Rectangle rect = new Rectangle
            //{
            //    X = (int)side.X,
            //    Y = sH - (int)side.Y - (int)page.GetClippedHByRotate(),
            //    Width = (int)page.GetClippedWByRotate(),
            //    Height = (int)page.GetClippedHByRotate()
            //};

            //g.FillRectangle(brush, rect);

            brush.Dispose();
        }

        private static void DrawTextBack(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
            var drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var x = page.GetPageDrawBackX() + page.GetPageDrawBackW() / 2;
            var y = sH - page.GetPageDrawBackH() - page.GetPageDrawBackY() + page.GetPageDrawBackH() / 2;
            var state = g.Save();
            g.TranslateTransform((float)(x * ScreenDrawer.ZoomFactor), (float)(y * ScreenDrawer.ZoomFactor));

            double angle = page.Back.Angle;

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
            Font font = new Font("Arial", (float)(12*ScreenDrawer.ZoomFactor));
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
                else
                {
                    txt = $"{back}";
                }
            }
            else
            if (sheet is TemplateSheet)
            {
                if ((page.Front.MasterIdx == 0 && page.Back.MasterIdx == 0))
                {
                    txt = "пуста";
                }
                else
                {
                    txt = $"{page.Back.MasterIdx}";
                }
            }

            path.AddString(txt, family, 0, (float)(12 * ScreenDrawer.ZoomFactor), new PointF { X = 0, Y = 0 }, drawFormat);
            g.DrawPath(pen, path);
            family.Dispose();
            pen.Dispose();
            font.Dispose();
            g.Restore(state);
        }

        public static void DrawPageFront(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
            PageSide side = page.Front;

            DrawBleeds(g, page, side, sH);

            (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(page, side);

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
                X = (float)x,
                Y = (float)y,
                Width = (float)w,
                Height = (float)h
            };

            ScreenDrawer.DrawFillRectangle(g, rect, brush);
            ScreenDrawer.DrawRectangle(g, rect, pen);
           
            brush.Dispose();
            pen.Dispose();


            ScreenDrawCommons.DrawPageRotateMarker(g, page, side, rect, sH);
            DrawTextFront(g, sheet, page, sH);

        }

        private static void DrawTextFront(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
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
            g.TranslateTransform((float)(x * ScreenDrawer.ZoomFactor), (float)(y * ScreenDrawer.ZoomFactor));

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
            Font font = new Font("Arial", (float)(12 *ScreenDrawer.ZoomFactor));
            FontFamily family = font.FontFamily;
            Pen pen = new Pen(Color.Black);

            int front = 0;
            int back = 0;

            string txt = string.Empty;


            if (sheet is PrintSheet)
            {
                if (page.Front.PrintIdx != 0) front = page.Front.PrintIdx;
                if (page.Back.PrintIdx != 0) back = page.Back.PrintIdx;

                if (front == 0 && back == 0)
                {
                    txt = "пуста";
                }
                else
                {
                    txt = $"{front}";
                }
            }
            else
            if (sheet is TemplateSheet)
            {
                if ((page.Front.MasterIdx == 0 && page.Back.MasterIdx == 0))
                {
                    txt = "пуста";
                }
                else
                {
                    txt = $"{page.Front.MasterIdx}";
                }
            }

            path.AddString(txt, family, 0, (float)(12*ScreenDrawer.ZoomFactor), new PointF { X = 0, Y = 0 }, drawFormat);
            g.DrawPath(pen, path);
            family.Dispose();
            pen.Dispose();
            font.Dispose();
            g.Restore(state);
        }

        private static void DrawSheetSafeField(Graphics g, TemplateSheet sheet)
        {

            var max = sheet.SafeFields.GetMaxFieldW();
            if (max > 0)
            {
                // left
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = 0,
                    Y = 0,
                    Width = (float)max,
                    Height = (float)sheet.H,
                }, Brushes.LightPink);

                //right

                ScreenDrawer.DrawFillRectangle(g,new RectangleF
                {
                    X = (float)(sheet.W - max),
                    Y = 0,
                    Width = (float)max,
                    Height = (float)sheet.H,
                }, Brushes.LightPink);
            }
            if (sheet.SafeFields.Bottom != 0)
                // bottom
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = 0,
                    Y = (float)(sheet.H - sheet.SafeFields.Bottom),
                    Width = (float)sheet.W,
                    Height = (float)sheet.SafeFields.Bottom,
                }, Brushes.LightPink);

            if (sheet.SafeFields.Top != 0)

                // top
                ScreenDrawer.DrawFillRectangle(g, new RectangleF
                {
                    X = 0,
                    Y = 0,
                    Width = (float)sheet.W,
                    Height = (float)sheet.SafeFields.Top,
                }, Brushes.LightPink);
        }
    }
}
