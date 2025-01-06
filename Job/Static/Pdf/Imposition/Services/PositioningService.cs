using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class PositioningService
    {
        public static void AnchorToAbsoluteCoordFront(RectangleD subject, PdfMark mark)
        {
            double x = 0;
            double y = 0;

            switch (mark.Parameters.ParentAnchorPoint)
            {
                case AnchorPoint.BottomLeft:
                    x = subject.X1;
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomCenter:
                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomRight:
                    x = subject.X2;
                    y = subject.Y1;
                    break;
                case AnchorPoint.Center:

                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1 + subject.H / 2;

                    break;
                case AnchorPoint.TopLeft:

                    x = subject.X1;
                    y = subject.Y2;

                    break;
                case AnchorPoint.TopCenter:

                    x = subject.X1 + subject.W / 2;
                    y = subject.Y2;

                    break;
                case AnchorPoint.TopRight:

                    x = subject.X2;
                    y = subject.Y2;

                    break;
                case AnchorPoint.LeftCenter:

                    x = subject.X1;
                    y = subject.Y1 + subject.H / 2;

                    break;

                case AnchorPoint.RightCenter:

                    x = subject.X2;
                    y = subject.Y1 + subject.H / 2;
                    break;
            }

            double xMark = 0;
            double yMark = 0;

            double w = mark.GetW();
            double h = mark.GetH();

            switch (mark.Parameters.MarkAnchorPoint)
            {
                case AnchorPoint.BottomLeft:
                    xMark = 0;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomCenter:
                    xMark = -w / 2;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomRight:
                    xMark = -w;
                    yMark = 0;
                    break;
                case AnchorPoint.Center:
                    xMark = -w / 2;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.TopLeft:
                    xMark = 0;
                    yMark = -h;
                    break;
                case AnchorPoint.TopCenter:
                    xMark = -w / 2;
                    yMark = -h;
                    break;
                case AnchorPoint.TopRight:
                    xMark = -w;
                    yMark = -h;
                    break;
                case AnchorPoint.LeftCenter:
                    xMark = 0;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.RightCenter:
                    xMark = -w;
                    yMark = -h / 2;
                    break;
                default:
                    break;
            }

            mark.Front = new PointD()
            {
                X = x + xMark + mark.Parameters.Xofs,
                Y = y + yMark + mark.Parameters.Yofs
            };

        }

        public static void AnchorToAbsoluteCoordFront(RectangleD subject, TextMark mark)
        {
            double x = 0;
            double y = 0;

            switch (mark.Parameters.ParentAnchorPoint)
            {
                case AnchorPoint.BottomLeft:
                    x = subject.X1;
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomCenter:
                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomRight:
                    x = subject.X2;
                    y = subject.Y1;
                    break;
                case AnchorPoint.Center:

                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1 + subject.H / 2;

                    break;
                case AnchorPoint.TopLeft:

                    x = subject.X1;
                    y = subject.Y2;

                    break;
                case AnchorPoint.TopCenter:

                    x = subject.X1 + subject.W / 2;
                    y = subject.Y2;

                    break;
                case AnchorPoint.TopRight:

                    x = subject.X2;
                    y = subject.Y2;

                    break;
                case AnchorPoint.LeftCenter:

                    x = subject.X1;
                    y = subject.Y1 + subject.H / 2;

                    break;

                case AnchorPoint.RightCenter:

                    x = subject.X2;
                    y = subject.Y1 + subject.H / 2;
                    break;
            }

            double xMark = 0;
            double yMark = 0;

            double w = mark.GetW();
            double h = mark.GetH();

            switch (mark.Parameters.MarkAnchorPoint)
            {
                case AnchorPoint.BottomLeft:
                    xMark = 0;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomCenter:
                    xMark = -w / 2;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomRight:
                    xMark = -w;
                    yMark = 0;
                    break;
                case AnchorPoint.Center:
                    xMark = -w / 2;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.TopLeft:
                    xMark = 0;
                    yMark = -h;
                    break;
                case AnchorPoint.TopCenter:
                    xMark = -w / 2;
                    yMark = -h;
                    break;
                case AnchorPoint.TopRight:
                    xMark = -w;
                    yMark = -h;
                    break;
                case AnchorPoint.LeftCenter:
                    xMark = 0;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.RightCenter:
                    xMark = -w;
                    yMark = -h / 2;
                    break;
                default:
                    break;
            }

            mark.Front = new PointD()
            {
                X = x + xMark + mark.Parameters.Xofs,
                Y = y + yMark + mark.Parameters.Yofs
            };

        }

        public static void AnchorToAbsoluteCoordBack(RectangleD subject, PdfMark mark)
        {
            AnchorToToAbsoluteCoordBackPdfMark(subject, mark);
        }

        public static void AnchorToAbsoluteCoordBack(RectangleD subject, TextMark mark)
        {
            AnchorToToAbsoluteCoordBackTextMark(subject, mark);
        }

        static void AnchorToToAbsoluteCoordBackTextMark(RectangleD subject, TextMark mark)
        {
            double x = 0;
            double y = 0;

            switch (mark.Parameters.ParentAnchorPoint)
            {
                case AnchorPoint.BottomLeft:

                    x = subject.X1 + subject.W;
                    if (mark.Parameters.IsBackMirrored)
                    {
                        x = subject.X1;
                    }
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomCenter:
                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomRight:
                    x = subject.X1;
                    y = subject.Y1;
                    break;
                case AnchorPoint.Center:

                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1 + subject.H / 2;

                    break;
                case AnchorPoint.TopLeft:

                    x = subject.X2;
                    y = subject.Y2;

                    break;
                case AnchorPoint.TopCenter:

                    x = subject.X1 + subject.W / 2;
                    y = subject.Y2;

                    break;
                case AnchorPoint.TopRight:

                    x = subject.X1;
                    y = subject.Y2;

                    break;
                case AnchorPoint.LeftCenter:

                    x = subject.X2;
                    y = subject.Y1 + subject.H / 2;

                    break;

                case AnchorPoint.RightCenter:

                    x = subject.X1;
                    y = subject.Y1 + subject.H / 2;
                    break;
            }

            double xMark = 0;
            double yMark = 0;

            double w = mark.GetW();
            double h = mark.GetH();

            switch (mark.Parameters.MarkAnchorPoint)
            {
                case AnchorPoint.BottomLeft:
                    xMark = 0;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomCenter:
                    xMark = -w / 2;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomRight:
                    xMark = -w;
                    yMark = 0;
                    break;
                case AnchorPoint.Center:
                    xMark = -w / 2;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.TopLeft:
                    xMark = 0;
                    yMark = -h;
                    break;
                case AnchorPoint.TopCenter:
                    xMark = -w / 2;
                    yMark = -h;
                    break;
                case AnchorPoint.TopRight:
                    xMark = -w;
                    yMark = -h;
                    break;
                case AnchorPoint.LeftCenter:
                    xMark = 0;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.RightCenter:
                    xMark = -w;
                    yMark = -h / 2;
                    break;
                default:
                    break;
            }

            double xOfs = -mark.Parameters.Xofs;
            if (mark.Parameters.IsBackMirrored)
            {
                xOfs = mark.Parameters.Xofs;
            }
            mark.Back = new PointD()
            {
                X = x + xMark + xOfs,
                Y = y + yMark + mark.Parameters.Yofs
            };
        }

        static void AnchorToToAbsoluteCoordBackPdfMark(RectangleD subject, PdfMark mark)
        {
            double x = 0;
            double y = 0;

            switch (mark.Parameters.ParentAnchorPoint)
            {
                case AnchorPoint.BottomLeft:
                    x = subject.X1 + subject.W;
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomCenter:
                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1;
                    break;
                case AnchorPoint.BottomRight:
                    x = subject.X1;
                    y = subject.Y1;
                    break;
                case AnchorPoint.Center:
                    x = subject.X1 + subject.W / 2;
                    y = subject.Y1 + subject.H / 2;
                    break;
                case AnchorPoint.TopLeft:
                    x = subject.X2;
                    y = subject.Y2;
                    break;
                case AnchorPoint.TopCenter:
                    x = subject.X1 + subject.W / 2;
                    y = subject.Y2;
                    break;
                case AnchorPoint.TopRight:
                    x = subject.X1;
                    y = subject.Y2;
                    break;
                case AnchorPoint.LeftCenter:
                    x = subject.X2;
                    y = subject.Y1 + subject.H / 2;
                    break;
                case AnchorPoint.RightCenter:
                    x = subject.X1;
                    y = subject.Y1 + subject.H / 2;
                    break;
            }

            double xMark = 0;
            double yMark = 0;

            double w = mark.GetW();
            double h = mark.GetH();

            switch (mark.Parameters.MarkAnchorPoint)
            {
                case AnchorPoint.BottomLeft:
                    xMark = 0;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomCenter:
                    xMark = -w / 2;
                    yMark = 0;
                    break;
                case AnchorPoint.BottomRight:
                    xMark = -w;
                    yMark = 0;
                    break;
                case AnchorPoint.Center:
                    xMark = -w / 2;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.TopLeft:
                    xMark = 0;
                    yMark = -h;
                    break;
                case AnchorPoint.TopCenter:
                    xMark = -w / 2;
                    yMark = -h;
                    break;
                case AnchorPoint.TopRight:
                    xMark = -w;
                    yMark = -h;
                    break;
                case AnchorPoint.LeftCenter:
                    xMark = 0;
                    yMark = -h / 2;
                    break;
                case AnchorPoint.RightCenter:
                    xMark = -w;
                    yMark = -h / 2;
                    break;
                default:
                    break;
            }


            mark.Back = new PointD()
            {
                X = x + xMark - mark.Parameters.Xofs,
                Y = y + yMark + mark.Parameters.Yofs
            };
        }
    }
}
