using JobSpace.Static.Pdf.Scale;
using PDFlib_dotnet;
using System;

namespace JobSpace.Static.Pdf.Common
{
    internal static class ExtendBoxHelpers
    {
        /// <summary>
        /// х координата в мм
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double xMM(this Box box) => Math.Round(box.left / PdfScaler.mn, 1);
        /// <summary>
        /// у координата в мм
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double yMM(this Box box) => Math.Round(box.bottom / PdfScaler.mn, 1);
        /// <summary>
        /// Ширина в мм
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double wMM(this Box box) => Math.Round(box.width / PdfScaler.mn, 1);
        /// <summary>
        /// Висота в мм
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double hMM(this Box box) => Math.Round(box.height / PdfScaler.mn, 1);
        public static double leftMM(this Box box) => Math.Round(box.left / PdfScaler.mn, 1);
        public static double rightMM(this Box box) => Math.Round(box.right / PdfScaler.mn, 1);
        public static double bottomMM(this Box box) => Math.Round(box.bottom / PdfScaler.mn, 1);
        public static double topMM(this Box box) => Math.Round(box.top / PdfScaler.mn, 1);
        public static double x(this Box box, double scaleFactor) => box.left * scaleFactor;
        public static double y(this Box box, double scaleFactor) => box.bottom * scaleFactor;
        public static double w(this Box box, double scaleFactor) => box.width * scaleFactor;
        public static double h(this Box box, double scaleFactor) => box.height * scaleFactor;

        public static double xMn(this Box box) => box.left * PdfScaler.mn;
        public static double yMn(this Box box) => box.bottom * PdfScaler.mn;
        public static double wMn(this Box box) => box.width * PdfScaler.mn;
        public static double hMn(this Box box) => box.height * PdfScaler.mn;

        public static bool IsEmpty(this Box box) => box.width == 0 || box.height == 0;

        public static void GetMediabox(this Box box, PDFlib p, int indoc,int page)
        {
            box.width = p.pcos_get_number(indoc, "pages[" + page + "]/width");
            box.height = p.pcos_get_number(indoc, "pages[" + page + "]/height");
        }

        public static void RotateCounerClockWise90deg(this Box box, Box media)
        {
            box.left = media.width - box.left - box.width;
            box.bottom = media.height - box.bottom - box.height;

            var tmp = box.width;
            box.width = box.height;
            box.height = tmp;

            tmp = box.left;
            box.left = box.bottom;
            box.bottom = tmp;
        }

        public static void RotateClockWise90deg(this Box box)
        {
            var tmp = box.width;
            box.width = box.height;
            box.height = tmp;

            tmp = box.left;
            box.left = box.bottom;
            box.bottom = tmp;

        }

        public static void CreateCustomBox(this Box box, double width, double height, double bleeds)
        {
            box.left = bleeds * PdfHelper.mn;
            box.bottom = bleeds * PdfHelper.mn;
            box.width = width * PdfHelper.mn;
            box.height = height * PdfHelper.mn;

        }

        public static (double Width, double Height) GetMediaBox(this Box box)
        {
            return (Width: box.width + box.left * 2, Height: box.height + box.bottom * 2);

        }
    }
}