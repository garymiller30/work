using Amazon.Runtime.Internal.Transform;
using Interfaces.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class PositioningService
    {

        // Універсальний метод, який замінює два словника і гігантський switch
        private static (double pX, double pY) GetAnchorCoefficientsFront(AnchorPoint anchor) => anchor switch
        {
            AnchorPoint.TopLeft => (0.0, 1.0),
            AnchorPoint.TopCenter => (0.5, 1.0),
            AnchorPoint.TopRight => (1.0, 1.0),

            AnchorPoint.LeftCenter => (0.0, 0.5),
            AnchorPoint.Center => (0.5, 0.5),
            AnchorPoint.RightCenter => (1.0, 0.5),

            AnchorPoint.BottomLeft => (0.0, 0.0),
            AnchorPoint.BottomCenter => (0.5, 0.0),
            AnchorPoint.BottomRight => (1.0, 0.0),
            _ => (0.0, 0.0)
        };

        public static void AnchorToAbsoluteCoordFront(RectangleD subject, PdfMark mark)
        {
            double w = mark.GetW(null);
            double h = mark.GetH(null);

            // 1. Отримуємо координати точки прив'язки на батьківському об'єкті
            (double px, double py) = GetAnchorCoefficientsFront(mark.Parameters.ParentAnchorPoint);

            double x = subject.X1 + subject.W * px;
            double y = subject.Y1 + subject.H * py;

            // 2. Отримуємо коефіцієнти для зміщення самої марки
            (double mx, double my) = GetAnchorCoefficientsFront(mark.Parameters.MarkAnchorPoint);
            double xMark = -w * mx;
            double yMark = -h * my;

            mark.Front = new PointD
            (
                x: x + xMark + mark.Parameters.Xofs,
                y: y + yMark + mark.Parameters.Yofs
            );

        }

        public static (double rx, double ry) RotateOffset(double dx, double dy, double angle)
        {
            return angle switch
            {
                0 => (dx, dy),
                90 => (-dy, dx),
                180 => (-dx, -dy),
                270 => (dy, -dx),
                _ => (dx, dy)
            };
        }

        public static void AnchorToAbsoluteCoordFront(RectangleD subject, TextMark mark, TextVariablesService textVariablesService)
        {
            double w = mark.GetW(textVariablesService);
            double h = mark.GetH(textVariablesService);

            // Розміри габаритного прямокутника (bounding box) з урахуванням кута повороту
            double boxW = (mark.Angle == 90 || mark.Angle == 270) ? h : w;
            double boxH = (mark.Angle == 90 || mark.Angle == 270) ? w : h;

            // 1. Отримуємо координати точки прив'язки на батьківському об'єкті
            (double px, double py) = GetAnchorCoefficientsFront(mark.Parameters.ParentAnchorPoint);

            double parentAnchorX = subject.X1 + subject.W * px;
            double parentAnchorY = subject.Y1 + subject.H * py;

            // 2. Отримуємо коефіцієнти для точки прив'язки габаритного прямокутника самої мітки
            (double mx, double my) = GetAnchorCoefficientsFront(mark.Parameters.MarkAnchorPoint);

            // Нижній лівий кут (X1, Y1) габаритного прямокутника мітки в системі координат PDF (Y знизу вгору)
            double boxX1 = parentAnchorX - boxW * mx + mark.Parameters.Xofs;
            double boxY1 = parentAnchorY - boxH * my + mark.Parameters.Yofs;

            // 3. Знаходимо опорну точку початку рядка (mark.Front) в залежності від кута повороту
            double originX = mark.Angle switch
            {
                0 => boxX1,
                90 => boxX1 + h,
                180 => boxX1 + w,
                270 => boxX1,
                _ => boxX1
            };

            double originY = mark.Angle switch
            {
                0 => boxY1,
                90 => boxY1,
                180 => boxY1 + h,
                270 => boxY1 + w,
                _ => boxY1
            };

            mark.Front = new PointD(originX, originY);
        }

        public static void AnchorToAbsoluteCoordBack(RectangleD subject, PdfMark mark)
        {
            AnchorToToAbsoluteCoordBackPdfMark(subject, mark);
        }

        public static void AnchorToAbsoluteCoordBack(RectangleD subject, TextMark mark, TextVariablesService textVariablesService, TemplateSheetPlaceType placeType = TemplateSheetPlaceType.Sheetwise)
        {
            AnchorToToAbsoluteCoordBackTextMark(subject, mark, textVariablesService, placeType);
        }

        private static (double pX, double pY) GetMarkAnchorCoefficientsBack(AnchorPoint anchor, bool isMirrored)
        {
            if (isMirrored)
            {
                return anchor switch
                {
                    AnchorPoint.TopLeft => (0.0, 1.0),
                    AnchorPoint.TopCenter => (0.5, 1.0),
                    AnchorPoint.TopRight => (1.0, 1.0),

                    AnchorPoint.LeftCenter => (0.0, 0.5),
                    AnchorPoint.Center => (0.5, 0.5),
                    AnchorPoint.RightCenter => (1.0, 0.5),

                    AnchorPoint.BottomLeft => (0.0, 0.0),
                    AnchorPoint.BottomCenter => (0.5, 0.0),
                    AnchorPoint.BottomRight => (1.0, 0.0),
                    _ => (0.0, 0.0)
                };

            }
            else
            {
                return anchor switch
                {
                    AnchorPoint.TopLeft => (1.0, 1.0),
                    AnchorPoint.TopCenter => (0.5, 1.0),
                    AnchorPoint.TopRight => (0.0, 1.0),

                    AnchorPoint.LeftCenter => (1.0, 0.5),
                    AnchorPoint.Center => (0.5, 0.5),
                    AnchorPoint.RightCenter => (0.0, 0.5),

                    AnchorPoint.BottomLeft => (1.0, 0.0),
                    AnchorPoint.BottomCenter => (0.5, 0.0),
                    AnchorPoint.BottomRight => (0.0, 0.0),
                    _ => (0.0, 0.0)
                };
            }
        }

        private static (double pX, double pY) GetParentAnchorCoefficientsBack(AnchorPoint anchor, TemplateSheetPlaceType placeType = TemplateSheetPlaceType.Sheetwise)
        {
            (double fx, double fy) = GetAnchorCoefficientsFront(anchor);

            return placeType switch
            {
                TemplateSheetPlaceType.WorkAndTumble => (fx, 1.0 - fy),
                _ => (1.0 - fx, fy)
            };
        }

        static void AnchorToToAbsoluteCoordBackTextMark(RectangleD subject, TextMark mark, TextVariablesService textVariablesService, TemplateSheetPlaceType placeType = TemplateSheetPlaceType.Sheetwise)
        {
            double w = mark.GetW(textVariablesService);
            double h = mark.GetH(textVariablesService);

            // Кут на звороті
            double backAngle = mark.GetBackAngle(placeType);

            // Розміри габаритного прямокутника мітки з урахуванням кута на звороті
            double boxW = (backAngle == 90 || backAngle == 270) ? h : w;
            double boxH = (backAngle == 90 || backAngle == 270) ? w : h;

            // 1. Отримуємо коефіцієнти для точки прив'язки на батьківському об'єкті
            (double px, double py) = GetParentAnchorCoefficientsBack(mark.Parameters.ParentAnchorPoint, placeType);

            double parentAnchorX = subject.X1 + subject.W * px;
            double parentAnchorY = subject.Y1 + subject.H * py;

            // 2. Зміщення точки прив'язки габаритного прямокутника з урахуванням дзеркалення
            (double mx, double my) = GetMarkAnchorCoefficientsBack(mark.Parameters.MarkAnchorPoint, mark.Parameters.IsBackMirrored);

            double xOfs = -mark.Parameters.Xofs;
            double yOfs = mark.Parameters.Yofs;

            if (mark.Parameters.IsBackMirrored)
            {
                xOfs = mark.Parameters.Xofs;
            }

            if (placeType == TemplateSheetPlaceType.WorkAndTumble)
            {
                yOfs = -mark.Parameters.Yofs;
            }

            // Нижній лівий кут (X1, Y1) габаритного прямокутника мітки на звороті
            double boxX1 = parentAnchorX - boxW * mx + xOfs;
            double boxY1 = parentAnchorY - boxH * my + yOfs;

            // 3. Знаходимо опорну точку початку рядка (mark.Back) для зворотного кута
            double originX = backAngle switch
            {
                0 => boxX1,
                90 => boxX1 + h,
                180 => boxX1 + w,
                270 => boxX1,
                _ => boxX1
            };

            double originY = backAngle switch
            {
                0 => boxY1,
                90 => boxY1,
                180 => boxY1 + h,
                270 => boxY1 + w,
                _ => boxY1
            };

            mark.Back = new PointD(originX, originY);
        }

        static void AnchorToToAbsoluteCoordBackPdfMark(RectangleD subject, PdfMark mark)
        {
            double w = mark.GetW(null);
            double h = mark.GetH(null);

            // 2. Отримуємо коефіцієнти для зміщення самої марки
            (double px, double py) = GetParentAnchorCoefficientsBack(mark.Parameters.ParentAnchorPoint);

            double x = subject.X1 + subject.W * px;
            double y = subject.Y1 + subject.H * py;

            (double mx, double my) = GetMarkAnchorCoefficientsBack(mark.Parameters.MarkAnchorPoint, mark.Parameters.IsBackMirrored);
            double xMark = -w * mx;
            double yMark = -h * my;


            double xOfs = -mark.Parameters.Xofs;
            if (mark.Parameters.IsBackMirrored)
            {
                xOfs = mark.Parameters.Xofs;
            }

            mark.Back = new PointD
            (
                x: x + xMark - xOfs,
                y: y + yMark + mark.Parameters.Yofs
            );
        }

        public static void CalcClipMarkCoordFront(TemplateSheet sheet, RectangleD sheetRect, RectangleD subjectRect, PdfMark mark)
        {
            var param = mark.Parameters;

            double x1 = param.ClipBox.Left;
            double y1 = param.ClipBox.Bottom;
            double x2 = param.ClipBox.Left + mark.GetClippedW();
            double y2 = param.ClipBox.Bottom + mark.GetClippedH();

            if (param.IsAutoClipX)
            {
                if (param.AutoClipRelativeX == AutoClipMarkEnum.Sheet)
                {

                }
                else if (param.AutoClipRelativeX == AutoClipMarkEnum.Subject)
                {
                    double mark_w = mark.GetW(null);

                    if (mark_w > subjectRect.W)
                    {
                        double left = subjectRect.X1 - mark.Front.X;
                        double right = mark_w + mark.Front.X - subjectRect.X2;

                        x1 = left;
                        x2 = left + subjectRect.W;

                        mark.Parameters.ClipBox.Left = left;
                        mark.Parameters.ClipBox.Right = right;
                    }
                }
            }


            if (param.IsAutoClipY)
            {
                if (param.AutoClipRelativeY == AutoClipMarkEnum.Sheet)
                {

                }
                else if (param.AutoClipRelativeY == AutoClipMarkEnum.Subject)
                {
                }
            }

            mark.ClipBoxFront = new RectangleD(x1: x1, y1: y1, x2: x2, y2: y2);



            double mark_x = mark.Front.X + mark.GetClippedLeftByAngleFront();
            double mark_y = mark.Front.Y + mark.GetClippedBottomByAngleFront();

            mark.Front = new PointD(mark_x, mark_y);
        }

        public static void CalcClipMarkCoordBack(TemplateSheet sheet, RectangleD sheetRect, RectangleD subjectRect, PdfMark mark)
        {
            var param = mark.Parameters;

            double x1 = param.ClipBox.Left;
            double y1 = param.ClipBox.Bottom;
            double x2 = param.ClipBox.Left + mark.GetClippedW();
            double y2 = param.ClipBox.Bottom + mark.GetClippedH();


            if (param.IsAutoClipX)
            {
                if (param.AutoClipRelativeX == AutoClipMarkEnum.Sheet)
                {

                }
                else if (param.AutoClipRelativeX == AutoClipMarkEnum.Subject)
                {
                    double mark_w = mark.GetW(null);

                    if (mark_w > subjectRect.W)
                    {
                        double left = subjectRect.X1 - mark.Back.X;
                        double right = mark_w + mark.Back.X - subjectRect.X2;

                        x1 = left;
                        x2 = left + subjectRect.W;

                        mark.Parameters.ClipBox.Left = left;
                        mark.Parameters.ClipBox.Right = right;
                    }
                }
            }


            if (param.IsAutoClipY)
            {
                if (param.AutoClipRelativeY == AutoClipMarkEnum.Sheet)
                {

                }
                else if (param.AutoClipRelativeY == AutoClipMarkEnum.Subject)
                {
                }
            }

            mark.ClipBoxBack = new RectangleD(x1: x1, y1: y1, x2: x2, y2: y2);

            double mark_x = mark.Back.X + mark.GetClippedLeftByAngleBack(sheet.SheetPlaceType);
            double mark_y = mark.Back.Y + mark.GetClippedBottomByAngleBack(sheet.SheetPlaceType);

            mark.Back = new PointD(mark_x, mark_y);
        }
    }
}
