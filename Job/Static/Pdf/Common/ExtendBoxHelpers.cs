using Job.Static.Pdf.Scale;
using System;

namespace Job.Static.Pdf.Common
{
    internal static class ExtendBoxHelpers
    {

        public static double xMM(this Box box) => Math.Round(box.x / PdfScaler.mn, 1);
        public static double yMM(this Box box) => Math.Round(box.y / PdfScaler.mn, 1);
        public static double wMM(this Box box) => Math.Round(box.width / PdfScaler.mn, 1);
        public static double hMM(this Box box) => Math.Round(box.height / PdfScaler.mn, 1);

        public static double x(this Box box, double scaleFactor) => box.x * scaleFactor;
        public static double y(this Box box, double scaleFactor) => box.y * scaleFactor;
        public static double w(this Box box, double scaleFactor) => box.width * scaleFactor;
        public static double h(this Box box, double scaleFactor) => box.height * scaleFactor;

        public static double xMn(this Box box) => box.x * PdfScaler.mn;
        public static double yMn(this Box box) => box.y * PdfScaler.mn;
        public static double wMn(this Box box) => box.width * PdfScaler.mn;
        public static double hMn(this Box box) => box.height * PdfScaler.mn;

        public static bool IsEmpty(this Box box) => box.width == 0 || box.height == 0;
    }
}