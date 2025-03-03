using JobSpace.Static.Pdf.Imposition.Drawers.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenDrawCommons
    {
        public static void DrawSheet(TemplateSheet sheet, Graphics g)
        {
            

            Pen pen = new Pen(Color.Black);
            var rect = new RectangleF(0, 0, (float)sheet.W, (float)sheet.H);

            ScreenDrawer.DrawRectangle(g, rect, pen);
            //g.DrawRectangle(pen, rect);
            pen.Dispose();
        }

        public static double GetPageDrawX(TemplatePage page, PageSide side)
        {
            // проти годинникової стрілки
            switch (side.Angle)
            {
                case 0:
                    return side.X + page.Margins.Left;
                case 90:
                    return side.X + page.Margins.Top;
                case 180:
                    return side.X + page.Margins.Right;
                case 270:
                    return side.X + page.Margins.Bottom;
                default:
                    throw new NotImplementedException();
            }
        }

        public static double GetPageDrawXBack(TemplateSheet sheet, TemplatePage page, PageSide side)
        {

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    switch (side.Angle)
                    {
                        case 0:
                            return side.X + page.Margins.Right;
                        case 90:
                            return side.X + page.Margins.Top;
                        case 180:
                            return side.X + page.Margins.Left;
                        case 270:
                            return side.X + page.Margins.Bottom;
                        default:
                            throw new NotImplementedException();
                    }

                case TemplateSheetPlaceType.WorkAndTumble:
                    switch (side.Angle)
                    {
                        case 0:
                            return side.X + page.Margins.Right;
                        case 90:
                            return side.X + page.Margins.Top;
                        case 180:
                            return side.X + page.Margins.Left;
                        case 270:
                            return side.X + page.Margins.Bottom;
                        default:
                            throw new NotImplementedException();
                    }

                default:
                    throw new NotImplementedException();
            }

            // проти годинникової стрілки
        }

        public static double GetPageDrawY(TemplatePage page, PageSide side)
        {
            // проти годинникової стрілки
            switch (side.Angle)
            {
                case 0:
                    return side.Y + page.Margins.Bottom;
                case 90:
                    return side.Y + page.Margins.Left;
                case 180:
                    return side.Y + page.Margins.Top;
                case 270:
                    return side.Y + page.Margins.Right;
                default:
                    throw new NotImplementedException();
            }
        }

        public static double GetPageDrawYBack(TemplateSheet sheet, TemplatePage page, PageSide side)
        {
            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
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
                case TemplateSheetPlaceType.WorkAndTumble:
                    switch (side.Angle)
                    {
                        case 0:
                            return side.Y + page.Margins.Bottom;
                        case 90:
                            return side.Y + page.Margins.Left;
                        case 180:
                            return side.Y + page.Margins.Top;
                        case 270:
                            return side.Y + page.Margins.Right;
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public static double GetBottomBleedByAngleFront(TemplatePage page, PageSide side)
        {
            var b = page.Bleeds;

            switch (side.Angle)
            {
                case 0: return b.Bottom;
                case 90: return b.Left;
                case 180: return b.Top;
                case 270: return b.Right;
                default:
                    throw new NotImplementedException();
            }
        }

        public static double GetLeftBleedByAngleFront(TemplatePage page, PageSide side)
        {
            var b = page.Bleeds;

            switch (side.Angle)
            {
                case 0: return b.Left;
                case 90: return b.Top;
                case 180: return b.Right;
                case 270: return b.Bottom;
                default:
                    throw new NotImplementedException();
            }
        }

        public static double GetLeftBleedByAngleBack(TemplateSheet sheet, TemplatePage page, PageSide side)
        {
            var b = page.Bleeds;

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    switch (side.Angle)
                    {
                        case 0: return b.Right;
                        case 90: return b.Top;
                        case 180: return b.Left;
                        case 270: return b.Bottom;
                        default:
                            throw new NotImplementedException();
                    }
                case TemplateSheetPlaceType.WorkAndTumble:
                    switch (side.Angle)
                    {
                        case 0: return b.Right;
                        case 90: return b.Top;
                        case 180: return b.Left;
                        case 270: return b.Bottom;
                        default:
                            throw new NotImplementedException();
                    }

                default:
                    throw new NotImplementedException();
            }
        }
        public static double GetBottomBleedByAngleBack(TemplateSheet sheet, TemplatePage page, PageSide side)
        {
            var b = page.Bleeds;

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                case TemplateSheetPlaceType.Sheetwise:
                case TemplateSheetPlaceType.WorkAndTurn:
                    switch (side.Angle)
                    {
                        case 0: return b.Bottom;
                        case 90: return b.Right;
                        case 180: return b.Top;
                        case 270: return b.Left;
                        default:
                            throw new NotImplementedException();
                    }

                case TemplateSheetPlaceType.WorkAndTumble:
                    switch (side.Angle)
                    {
                        case 0: return b.Bottom;
                        case 90: return b.Left;
                        case 180: return b.Top;
                        case 270: return b.Right;
                        default:
                            throw new NotImplementedException();
                    }


                default: throw new NotImplementedException();
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

        public static void DrawPageRotateMarker(Graphics g, TemplatePage page, PageSide side, RectangleF rect, int sH)
        {
           

            var dist = 5;
            var height = 7;

            var brush = new SolidBrush(Color.Gray);

            var x = rect.X;
            var y = rect.Y;

            float sx = 0;
            float sy = 0;
            float sw = 0;
            float sh = 0;

            var page_w = (float)GetPageDrawW(page, side);
            var page_h = (float)GetPageDrawH(page, side);

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
            ScreenDrawer.DrawFillRectangle(g, new RectangleF(sx, sy, sw, sh), brush);

            //g.FillRectangle(brush, new System.Drawing.Rectangle((int)(sx),(int) (sy),(int)( sw),(int) (sh)));
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

            var m = page.Margins;
            var b = page.Bleeds;

            switch (side.Angle)
            {
                case 0:

                    return new RectangleD()
                    {
                        X1 = page_x - getLeft(),
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h,
                    }
                        ;
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - getLeft(),
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + getLeft(),
                        Y2 = page_y + page_h,
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + getLeft(),
                    };
                default:
                    throw new NotImplementedException();
            }

            double getLeft()
            {
                //if (b.Left > m.Left) return m.Left;
                return b.Left;

            }
            ;// => b.Left;// m.Left < b.Left ? m.Left : b.Left;
        }

        public static RectangleD GetDrawBleedRightFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            var m = page.Margins;
            var b = page.Bleeds;

            switch (side.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + getRight(),
                        Y2 = page_y + page_h,
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + getRight()
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x - getRight(),
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - getRight(),
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                default:
                    throw new NotImplementedException();
            }

            double getRight()
            {
                //if (b.Right > m.Right) return m.Right;

                return b.Right;


            } // m.Right < b.Right ? m.Right : b.Right;

        }

        public static RectangleD GetDrawBleedTopFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            var m = page.Margins;
            var b = page.Bleeds;

            switch (side.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + getTop()
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x - getTop(),
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - getTop(),
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + getTop(),
                        Y2 = page_y + page_h
                    };
                default:
                    throw new NotImplementedException();
            }

            double getTop()
            {

                //if (b.Top > m.Top) return m.Top;

                return b.Top;

            }
            //b.Top;// m.Top < b.Top ? m.Top : b.Top;

        }

        public static RectangleD GetDrawBleedBottomFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            var m = page.Margins;
            var b = page.Bleeds;

            switch (side.Angle)
            {
                case 0:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y - getBottom(),
                        X2 = page_x + page_w,
                        Y2 = page_y
                    };
                case 90:
                    return new RectangleD()
                    {
                        X1 = page_x + page_w,
                        Y1 = page_y,
                        X2 = page_x + page_w + getBottom(),
                        Y2 = page_y + page_h
                    };
                case 180:
                    return new RectangleD()
                    {
                        X1 = page_x,
                        Y1 = page_y + page_h,
                        X2 = page_x + page_w,
                        Y2 = page_y + page_h + getBottom(),
                    };
                case 270:
                    return new RectangleD()
                    {
                        X1 = page_x - getBottom(),
                        Y1 = page_y,
                        X2 = page_x,
                        Y2 = page_y + page_h
                    };
                default:
                    throw new NotImplementedException();
            }

            double getBottom()
            {

                //if (b.Bottom > m.Bottom) return m.Bottom;
                return b.Bottom;
            }// => b.Bottom;// m.Bottom < b.Bottom ? m.Bottom : b.Bottom;
        }

        public static (double page_x, double page_y, double page_w, double page_h) GetPageDrawBack(TemplateSheet sheet, TemplatePage page, PageSide side)
        {
            double page_x = GetPageDrawXBack(sheet, page, side);
            double page_y = GetPageDrawYBack(sheet, page, side);
            double page_w = GetPageDrawW(page, side);
            double page_h = GetPageDrawH(page, side);
            return (page_x, page_y, page_w, page_h);
        }
    }
}
