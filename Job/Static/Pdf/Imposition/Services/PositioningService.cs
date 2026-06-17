using Amazon.Runtime.Internal.Transform;
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

        public static void AnchorToAbsoluteCoordFront(RectangleD subject, TextMark mark, TextVariablesService textVariablesService)
        {
            double w = mark.GetW(textVariablesService);
            double h = mark.GetH(textVariablesService);

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

        public static void AnchorToAbsoluteCoordBack(RectangleD subject, PdfMark mark)
        {
            AnchorToToAbsoluteCoordBackPdfMark(subject, mark);
        }

        public static void AnchorToAbsoluteCoordBack(RectangleD subject, TextMark mark, TextVariablesService textVariablesService)
        {
            AnchorToToAbsoluteCoordBackTextMark(subject, mark, textVariablesService);
        }

        private static (double pX, double pY) GetAnchorCoefficientsBack(AnchorPoint anchor, bool isMirrored)
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

        private static (double pX, double pY) GetParentAnchorCoefficientsBack(AnchorPoint anchor)
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

        static void AnchorToToAbsoluteCoordBackTextMark(RectangleD subject, TextMark mark, TextVariablesService textVariablesService)
        {

            double w = mark.GetW(textVariablesService);
            double h = mark.GetH(textVariablesService);

            // 2. Отримуємо коефіцієнти для зміщення самої марки
            (double px, double py) = GetParentAnchorCoefficientsBack(mark.Parameters.ParentAnchorPoint);

            double x = subject.X1 + subject.W * px;
            double y = subject.Y1 + subject.H * py;

            (double mx, double my) = GetAnchorCoefficientsBack(mark.Parameters.MarkAnchorPoint, mark.Parameters.IsBackMirrored);
            double xMark = -w * mx;
            double yMark = -h * my;

            double xOfs = -mark.Parameters.Xofs;
            if (mark.Parameters.IsBackMirrored)
            {
                xOfs = mark.Parameters.Xofs;
            }

            mark.Back = new PointD
            (
                x: x + xMark + xOfs,
                y: y + yMark + mark.Parameters.Yofs
            );

        }

        static void AnchorToToAbsoluteCoordBackPdfMark(RectangleD subject, PdfMark mark)
        {
            double w = mark.GetW(null);
            double h = mark.GetH(null);

            // 2. Отримуємо коефіцієнти для зміщення самої марки
            (double px, double py) = GetParentAnchorCoefficientsBack(mark.Parameters.ParentAnchorPoint);

            double x = subject.X1 + subject.W * px;
            double y = subject.Y1 + subject.H * py;

            (double mx, double my) = GetAnchorCoefficientsBack(mark.Parameters.MarkAnchorPoint, mark.Parameters.IsBackMirrored);
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
