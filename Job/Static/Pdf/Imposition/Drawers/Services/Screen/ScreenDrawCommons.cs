using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenDrawCommons
    {
        public static void DrawSheet(TemplateSheet sheet, Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, (int)sheet.W, (int)sheet.H);

            g.DrawRectangle(pen, rect);
            pen.Dispose();
        }

        public static double GetPageDrawX(TemplatePage page, PageSide side)
        {
            switch (side.Angle)
            {
                case 0:
                    return side.X + page.Margins.Left;
                case 90:
                    return side.X + page.Margins.Top;
                case 180:
                    return side.X + page.Margins.Bottom;
                case 270:
                    return side.X + page.Margins.Right;
                default:
                    throw new NotImplementedException();
            }
        }

        public static double GetPageDrawY(TemplatePage page, PageSide side)
        {
            switch (side.Angle)
            {
                case 0:
                    return side.Y + page.Margins.Bottom;
                case 90:
                    return side.Y + page.Margins.Right;
                case 180:
                    return side.Y + page.Margins.Top;
                case 270:
                    return side.Y + page.Margins.Left;
                default:
                    throw new NotImplementedException();
            }
        }

        public static double GetPageDrawW(TemplatePage page, PageSide side)
        {
            if (side.Angle == 0 || side.Angle == 180) return page.W;
            return page.H;
        }

        public static double GetPageDrawH(TemplatePage page, PageSide side)
        {
            if (side.Angle == 0 || side.Angle == 180) return page.H;
            return page.W;
        }

        public static void DrawPageRotateMarker(Graphics g, TemplatePage page, PageSide side, Rectangle rect, int sH)
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

            int page_w = (int)GetPageDrawW(page, side);
            int page_h = (int)GetPageDrawH(page, side);

            switch (side.Angle)
            {
                case 0:
                    sx = x + dist;
                    sy = y + dist;
                    sw = page_w - dist * 2;
                    sh = height;
                    break;
                case 90:
                    sx = x + dist;
                    sy = y + dist;
                    sw = height;
                    sh = page_h - dist * 2;
                    break;
                case 180:
                    sx = x + dist;
                    sy = y + page_h - dist - height;
                    sw = page_w - dist * 2;
                    sh = height;
                    break;
                case 270:

                    sx = x + page_w - dist - height;
                    sy = y + dist;
                    sw = height;
                    sh = (int)page_h - dist * 2;
                    break;

                default:
                    break;
            }
            g.FillRectangle(brush, new System.Drawing.Rectangle(sx, sy, sw, sh));
            brush.Dispose();
        }


        public static (RectangleD left, RectangleD right, RectangleD top, RectangleD bottom) GetDrawBleedsFront(TemplatePage page)
        {
            RectangleD left = GetDrawBleedLeftFront(page);
            RectangleD right = GetDrawBleedRightFront(page);
            RectangleD top = GetDrawBleedTopFront(page);
            RectangleD bottom = GetDrawBleedBottomFront(page);
            return (left, right, top, bottom);
        }

        public static (double page_x, double page_y, double page_w, double page_h) GetPageDraw(TemplatePage page, PageSide side)
        {
            double page_x = GetPageDrawX(page, side);
            double page_y = GetPageDrawY(page, side);
            double page_w = GetPageDrawW(page, side);
            double page_h = GetPageDrawH(page, side);
            return (page_x, page_y, page_w, page_h);
        }

        public static RectangleD GetDrawBleedLeftFront(TemplatePage page)
        {
            PageSide side = page.Front;

            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            switch (side.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = page_x - page.Bleeds.Left,
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h,
                    }
                        ;
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - page.Bleeds.Left,
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + page.Bleeds.Left,
                        Y2 = page_y + page_h,
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + page.Bleeds.Left,
                    };
                default:
                    throw new NotImplementedException();
            }
        }


        public static RectangleD GetDrawBleedRightFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            switch (side.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + page.Bleeds.Right,
                        Y2 = page_y + page_h,
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + page.Bleeds.Right
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x - page.Bleeds.Right,
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - page.Bleeds.Right,
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                default:
                    throw new NotImplementedException();
            }
        }

        public static RectangleD GetDrawBleedTopFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            switch (side.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + page.Bleeds.Top
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x - page.Bleeds.Top,
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - page.Bleeds.Top,
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + page.Bleeds.Top,
                        Y2 = page_y + page_h
                    };
                default:
                    throw new NotImplementedException();
            }
        }

        public static RectangleD GetDrawBleedBottomFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            switch (side.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - page.Bleeds.Bottom,
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + page.Bleeds.Bottom,
                        Y2 = page_y + page_h
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + page.Bleeds.Bottom,
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x - page.Bleeds.Bottom,
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
