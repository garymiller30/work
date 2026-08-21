using Interfaces.Pdf.Imposition;
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
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen
{
    public static class ScreenDrawCommons
    {
        public static void DrawSheet(TemplateSheet sheet, Graphics g)
        {
            Pen pen = Pens.Black;
            var rect = new RectangleF(0, 0, (float)sheet.W, (float)sheet.H);
            ScreenDrawer.DrawRectangle(g, rect, pen);
        }

        public static double GetPageDrawX(TemplatePage page, PageSide side)
        {
            // проти годинникової стрілки
            return side.Angle switch
            {
                0 => side.X + page.Margins.Left,
                90 => side.X + page.Margins.Top,
                180 => side.X + page.Margins.Right,
                270 => side.X + page.Margins.Bottom,
                _ => throw new NotImplementedException()
            };
        }
        public static double GetPageDrawXBack(TemplateSheet sheet, TemplatePage page, PageSide side)
        {
            // 1. Перевіряємо, чи підтримується тип аркуша (використовуємо pattern matching)
            if (sheet.SheetPlaceType is not (TemplateSheetPlaceType.SingleSide or
                                           TemplateSheetPlaceType.Sheetwise or
                                           TemplateSheetPlaceType.WorkAndTurn or
                                           TemplateSheetPlaceType.WorkAndTumble))
            {
                throw new NotImplementedException();
            }

            // 2. Використовуємо switch expression для вибору відступу залежно від кута
            return side.Angle switch
            {
                0 => side.X + page.Margins.Right,
                90 => side.X + page.Margins.Top,
                180 => side.X + page.Margins.Left,
                270 => side.X + page.Margins.Bottom,
                _ => throw new NotImplementedException()
            };
        }
        public static double GetPageDrawY(TemplatePage page, PageSide side) =>
            side.Angle switch
            {
                0 => side.Y + page.Margins.Bottom,
                90 => side.Y + page.Margins.Left,
                180 => side.Y + page.Margins.Top,
                270 => side.Y + page.Margins.Right,
                _ => throw new NotImplementedException()
            };

        public static double GetPageDrawYBack(TemplateSheet sheet, TemplatePage page, PageSide side)
        {
            // 1. Визначаємо, яку саме межу (Margin) нам потрібно взяти
            double margin = sheet.SheetPlaceType switch
            {
                TemplateSheetPlaceType.WorkAndTumble => side.Angle switch
                {
                    0 => page.Margins.Bottom,
                    90 => page.Margins.Left,
                    180 => page.Margins.Top,
                    270 => page.Margins.Right,
                    _ => throw new NotImplementedException($"Недопустимий кут: {side.Angle}")
                },

                // Для всіх інших типів (SingleSide, Sheetwise, WorkAndTurn) логіка інша
                TemplateSheetPlaceType.SingleSide or
                TemplateSheetPlaceType.Sheetwise or
                TemplateSheetPlaceType.WorkAndTurn => side.Angle switch
                {
                    0 => page.Margins.Bottom,
                    90 => page.Margins.Right,
                    180 => page.Margins.Top,
                    270 => page.Margins.Left,
                    _ => throw new NotImplementedException($"Недопустимий кут: {side.Angle}")
                },

                _ => throw new NotImplementedException($"Недопустимий тип аркуша: {sheet.SheetPlaceType}")
            };

            // 2. Виконуємо єдиний математичний розрахунок
            return side.Y + margin;
        }

        public static double GetBottomBleedByAngleFront(TemplatePage page, PageSide side)
        {
            var b = page.Bleeds;
            return side.Angle switch
            {
                0 => b.Bottom,
                90 => b.Left,
                180 => b.Top,
                270 => b.Right,
                _ => throw new NotImplementedException() // Використання discard pattern (_)
            };
        }


        public static double GetLeftBleedByAngleFront(TemplatePage page, PageSide side)
        {
            var b = page.Bleeds;

            return side.Angle switch
            {
                0 => b.Left,
                90 => b.Top,
                180 => b.Right,
                270 => b.Bottom,
                _ => throw new NotImplementedException()
            };
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

        public static double GetPageDrawW(TemplatePage page, PageSide side) => side.Angle is 0 or 180 ? page.W : page.H;
        public static double GetPageDrawH(TemplatePage page, PageSide side) => side.Angle is 0 or 180 ? page.H : page.W;

        public static void DrawPageRotateMarker(Graphics g, TemplatePage page, PageSide side, RectangleF rect, int sH)
        {
            const float dist = 5f;
            const float markerHeight = 7f;
            float zoom = (float)ScreenDrawer.ZoomFactor;

            float pageW = (float)GetPageDrawW(page, side);
            float pageH = (float)GetPageDrawH(page, side);

            // Явно вказуємо типи в кортежі, щоб уникнути помилки CS8506
            (RectangleF marker, PointF textTransform, float rotation) = side.Angle switch
            {
                0 => (
                    new RectangleF(rect.X + dist, rect.Y + dist, pageW - dist * 2, markerHeight),
                    new PointF(rect.X + dist, rect.Y + dist),
                    0f),
                90 => (
                    new RectangleF(rect.X + dist, rect.Y + dist, markerHeight, pageH - dist * 2),
                    new PointF(rect.X + dist, rect.Y + pageH - dist),
                    270f),
                180 => (
                    new RectangleF(rect.X + dist, rect.Y + pageH - dist - markerHeight, pageW - dist * 2, markerHeight),
                    new PointF(rect.X + pageW - 5, rect.Y + pageH + dist - markerHeight - 2),
                    180f),
                270 => (
                    new RectangleF(rect.X + pageW - dist - markerHeight, rect.Y + dist, markerHeight, (int)pageH - dist * 2),
                    new PointF(rect.X + pageW - markerHeight + 3, rect.Y + dist),
                    90f),
                _ => (RectangleF.Empty, PointF.Empty, 0f)
            };

            // 1. Малюємо маркер
            if (marker.Width > 0 && marker.Height > 0)
            {
                using var brush = new SolidBrush(Color.Gray);
                ScreenDrawer.DrawFillRectangle(g, marker, brush);
            }

            // 2. Малюємо текст групи
            if (page.Group > 0)
            {
                var state = g.Save();

                g.TranslateTransform(textTransform.X * zoom, textTransform.Y * zoom);
                g.RotateTransform(rotation);

                ScreenDrawer.DrawText(g, $"група {page.Group}", new PointF(0, 0), "Arial", 5);

                g.Restore(state);
            }
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

            // Використовуємо константу замість локальної функції для простих значень
            double bleedLeft = page.Bleeds.Left;

            // Застосовуємо pattern matching та умовні оператори для компактності
            return side.Angle switch
            {
                0 => new RectangleD(page_x - bleedLeft, page_y, page_x, page_y + page_h),
                90 => new RectangleD(page_x, page_y - bleedLeft, page_x + page_w, page_y),
                180 => new RectangleD(page_x + page_w, page_y, page_x + page_w + bleedLeft, page_y + page_h),
                270 => new RectangleD(page_x, page_y + page_h, page_x + page_w, page_y + page_h + bleedLeft),
                _ => throw new NotImplementedException($"Не підтримується кут {side.Angle}")
            };
        }

        public static RectangleD GetDrawBleedRightFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            var b = page.Bleeds;
            double bleedRight = b.Right;

            return side.Angle switch
            {
                0 => new RectangleD(page_x + page_w, page_y, page_x + page_w + bleedRight, page_y + page_h),
                90 => new RectangleD(page_x, page_y + page_h, page_x + page_w, page_y + page_h + bleedRight),
                180 => new RectangleD(page_x - bleedRight, page_y, page_x, page_y + page_h),
                270 => new RectangleD(page_x, page_y - bleedRight, page_x + page_w, page_y),
                _ => throw new NotImplementedException($"Не підтримується кут {side.Angle}")
            };
        }

        public static RectangleD GetDrawBleedTopFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            var b = page.Bleeds;
            double bleedTop = b.Top;

            return side.Angle switch
            {
                0=> new RectangleD(page_x, page_y + page_h, page_x + page_w, page_y + page_h + bleedTop),
                90=>new RectangleD(page_x - bleedTop,page_y,page_x, page_y + page_h),
                180=> new RectangleD( page_x, page_y - bleedTop, page_x + page_w,page_y),
                270=> new RectangleD(page_x + page_w,page_y, page_x + page_w + bleedTop,page_y + page_h),
                _ => throw new NotImplementedException($"Не підтримується кут {side.Angle}")
            };
        }

        public static RectangleD GetDrawBleedBottomFront(TemplatePage page)
        {
            PageSide side = page.Front;
            (double page_x, double page_y, double page_w, double page_h) = GetPageDraw(page, side);

            var b = page.Bleeds;
            double bleedBottom = b.Bottom;

            return side.Angle switch
            {
                0=> new RectangleD(page_x,page_y - bleedBottom,page_x + page_w,page_y),
                90=>new RectangleD(page_x + page_w,page_y,page_x + page_w + bleedBottom, page_y + page_h),
                180=>new RectangleD(page_x,page_y + page_h,page_x + page_w, page_y + page_h + bleedBottom),
                270=>new RectangleD(page_x - bleedBottom, page_y, page_x,  page_y + page_h),
                _ => throw new NotImplementedException($"Не підтримується кут {side.Angle}")
            };
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
