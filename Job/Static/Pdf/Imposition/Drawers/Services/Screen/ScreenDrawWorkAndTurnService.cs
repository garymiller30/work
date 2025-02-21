using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop;
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
            DrawSheetMarksBack(g,sheet,sheet.Marks, foreground: false, (int)sheet.H);

            // draw pages
            foreach (var page in templateContainer.TemplatePages)
            {
                DrawPageFront(g, sheet, page, (int)sheet.H);
                DrawPageBack(g, sheet, page, (int)sheet.H);
            }
            DrawCropMarks(g, sheet);
            //draw foreground marks
            ScreenDrawSingleSideService.DrawSheetMarksFront(g, sheet, foreground: true, (int)sheet.H);
            DrawSheetMarksBack(g, sheet,sheet.Marks, foreground: true, (int)sheet.H);
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
            foreach (var mark in container.Pdf.Where(x=> x.GetMarkSideBack(sheet.SheetPlaceType) && x.Enable && x.IsForeground == foreground))
            {
                System.Drawing.Image bitmap = MarksService.GetBitmapFront(mark);

                var rect = new Rectangle
                {
                    X = (int)mark.Back.X,
                    Y = h - (int)mark.Back.Y - (int)mark.GetClippedH(),
                    Width = (int)mark.GetClippedW(),
                    Height = (int)mark.GetClippedH()
                };

                if (bitmap == null)
                {
                    Brush brush = new SolidBrush(Color.Aqua);
                    g.FillRectangle(brush, rect);
                    brush.Dispose();

                }
                else
                {
                    var mc = mark.ClipBoxBack;

                    Rectangle clipRect = new Rectangle((int)mc.Left, (int)mc.Bottom, (int)(mc.Right - mc.Left), (int)(mc.Top - mc.Bottom));
                    Bitmap croppedBitmap = new Bitmap(clipRect.Width, clipRect.Height);

                    using (Graphics gc = Graphics.FromImage(croppedBitmap))
                    {
                        // Draw the cropped section of the original bitmap
                        gc.DrawImage(bitmap, new Rectangle(0, 0, clipRect.Width, clipRect.Height), clipRect, GraphicsUnit.Pixel);

                    }

                    var i = ScreenDrawSingleSideService.RotateImage(bitmap, (float)mark.GetBackAngle(sheet.SheetPlaceType));
                    i.MakeTransparent(Color.White);

                    g.DrawImage(i, rect);
                    croppedBitmap.Dispose();
                    //cropped.Dispose();
                    i.Dispose();
                    bitmap.Dispose();
                }
            }

            container.Containers.ForEach(x => DrawPdfMarkBack(g, sheet, x, foreground, h));
        }

        private static void DrawSheet(TemplateSheet sheet, Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, (int)sheet.W, (int)sheet.H);

            g.DrawRectangle(pen, rect);
            pen.Dispose();


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
                    g.DrawLine(pen, p1, p2);
                }

            }
            pen.Dispose();
        }

        public static void DrawPageBack(Graphics g, TemplateSheet sheet, TemplatePage page, int sH)
        {
            DrawBleeds(g, page, page.Back, sH);

            int x = (int)page.GetPageDrawBackX();
            int y = sH - (int)page.GetPageDrawBackY() - (int)page.GetPageDrawBackH();
            int w = (int)page.GetPageDrawBackW();
            int h = (int)page.GetPageDrawBackH();

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


            ScreenDrawCommons.DrawPageRotateMarker(g, page, page.Back, rect, sH);
            DrawTextBack(g, sheet, page, sH);

        }

        public static void DrawBleeds(Graphics g, TemplatePage page, PageSide side, int sH)
        {
            var brush = new SolidBrush(Color.LightGreen);

            (RectangleD left, RectangleD right, RectangleD top, RectangleD bottom) = ScreenDrawCommons.GetDrawBleedsFront(page);

            Rectangle rect_left = new Rectangle
            {
                X = (int)left.X1,
                Y = sH - (int)left.Y1 - (int)left.H,
                Width = (int)left.W,
                Height = (int)left.H
            };
            g.FillRectangle(brush, rect_left);

            Rectangle rect_right = new Rectangle
            {
                X = (int)right.X1,
                Y = sH - (int)right.Y1 - (int)right.H,
                Width = (int)right.W,
                Height = (int)right.H
            };
            g.FillRectangle(brush, rect_right);

            Rectangle rect_top = new Rectangle
            {
                X = (int)top.X1,
                Y = sH - (int)top.Y1 - (int)top.H,
                Width = (int)top.W,
                Height = (int)top.H
            };
            g.FillRectangle(brush, rect_top);

            Rectangle rect_bottom = new Rectangle
            {
                X = (int)bottom.X1,
                Y = sH - (int)bottom.Y1 - (int)bottom.H,
                Width = (int)bottom.W,
                Height = (int)bottom.H
            };
            g.FillRectangle(brush, rect_bottom);


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

            var x = (float)(page.GetPageDrawBackX() + page.GetPageDrawBackW() / 2);
            var y = sH - page.GetPageDrawBackH() - page.GetPageDrawBackY() + page.GetPageDrawBackH() / 2;
            var state = g.Save();
            g.TranslateTransform(x, (float)y);

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
            Font font = new Font("Arial", 12);
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

            path.AddString(txt, family, 0, 12, new PointF { X = 0, Y = 0 }, drawFormat);
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

            int x = (int)page_x;
            int y = sH - (int)page_y - (int)page_h;
            int w = (int)page_w;
            int h = (int)page_h;

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
            g.TranslateTransform(x, (float)y);

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
            Font font = new Font("Arial", 12);
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

            path.AddString(txt, family, 0, 12, new PointF { X = 0, Y = 0 }, drawFormat);
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
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = (int)max,
                    Height = (int)sheet.H,
                });

                //right
                g.FillRectangle(Brushes.LightPink, new Rectangle
                {
                    X = (int)(sheet.W - max),
                    Y = 0,
                    Width = (int)max,
                    Height = (int)sheet.H,
                });
            }
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
        }
    }
}
