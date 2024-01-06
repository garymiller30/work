using Job.Static.Pdf.Scale;
using PDFlib_dotnet;
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

        public static void GetMediabox(this Box box, PDFlib p, int indoc,int page)
        {
            box.width = p.pcos_get_number(indoc, "pages[" + page + "]/width");
            box.height = p.pcos_get_number(indoc, "pages[" + page + "]/height");
        }

        public static void RotateCounerClockWise90deg(this Box box, Box media)
        {
            box.x = media.width - box.x - box.width;
            box.y = media.height - box.y - box.height;

            var tmp = box.width;
            box.width = box.height;
            box.height = tmp;

            tmp = box.x;
            box.x = box.y;
            box.y = tmp;
        }

        public static void RotateClockWise90deg(this Box box)
        {
            var tmp = box.width;
            box.width = box.height;
            box.height = tmp;

            tmp = box.x;
            box.x = box.y;
            box.y = tmp;

        }

        public static void CreateCustomBox(this Box box, double width, double height, double bleeds)
        {
            box.x = bleeds * PdfHelper.mn;
            box.y = bleeds * PdfHelper.mn;
            box.width = width * PdfHelper.mn;
            box.height = height * PdfHelper.mn;

        }

        public static (double Width, double Height) GetMediaBox(this Box box)
        {
            return (Width: box.width + box.x * 2, Height: box.height + box.y * 2);

        }
    }
}