using JobSpace.Static.Pdf.Imposition.Models;
using ImageMagick;
using System;

namespace JobSpace.Static
{
    public static class ColorUtil
    {
        public static (int R, int G, int B) LabToRGB(double L, double a, double b)
        {
            // First, convert Lab to XYZ
            double Y = (L + 16.0) / 116.0;
            double X = a / 500.0 + Y;
            double Z = Y - b / 200.0;

            X = 95.047 * ((X > 0.206897) ? X * X * X : (X - 16.0 / 116.0) / 7.787);
            Y = 100.000 * ((Y > 0.206897) ? Y * Y * Y : (Y - 16.0 / 116.0) / 7.787);
            Z = 108.883 * ((Z > 0.206897) ? Z * Z * Z : (Z - 16.0 / 116.0) / 7.787);

            // Then, convert XYZ to RGB
            X /= 100.0;
            Y /= 100.0;
            Z /= 100.0;

            double R = X * 3.2406 + Y * -1.5372 + Z * -0.4986;
            double G = X * -0.9689 + Y * 1.8758 + Z * 0.0415;
            double B = X * 0.0557 + Y * -0.2040 + Z * 1.0570;

            R = (R > 0.0031308) ? (1.055 * Math.Pow(R, 1.0 / 2.4) - 0.055) : 12.92 * R;
            G = (G > 0.0031308) ? (1.055 * Math.Pow(G, 1.0 / 2.4) - 0.055) : 12.92 * G;
            B = (B > 0.0031308) ? (1.055 * Math.Pow(B, 1.0 / 2.4) - 0.055) : 12.92 * B;

            R = Math.Max(0, Math.Min(255, R * 255.0));
            G = Math.Max(0, Math.Min(255, G * 255.0));
            B = Math.Max(0, Math.Min(255, B * 255.0));

            return ((int)R, (int)G, (int)B);
        }

        public static (int R, int G, int B) CMYKToRGB(double c, double m, double y, double k)
        {
            try
            {
                var pixels = new[]
                {
                    ToBytePercent(c),
                    ToBytePercent(m),
                    ToBytePercent(y),
                    ToBytePercent(k)
                };

                var settings = new PixelReadSettings(1, 1, StorageType.Char, PixelMapping.CMYK);

                using (var image = new MagickImage())
                {
                    image.ReadPixels(pixels, settings);
                    image.ColorSpace = ColorSpace.CMYK;
                    image.TransformColorSpace(ColorProfiles.USWebCoatedSWOP, ColorProfiles.SRGB);

                    var color = image.GetPixels().GetPixel(0, 0).ToColor();
                    return (ToRgbChannel(color.R), ToRgbChannel(color.G), ToRgbChannel(color.B));
                }
            }
            catch
            {
                return DeviceCMYKToRGB(c, m, y, k);
            }
        }

        public static (int R, int G, int B) CMYKToRGB(MarkColor markColor)
        {
            return CMYKToRGB(markColor.C, markColor.M, markColor.Y, markColor.K);
        }

        public static (int R, int G, int B) CMYKToRGB(decimal c, decimal m, decimal y, decimal k)
        {
            return CMYKToRGB((double)c, (double)m, (double)y, (double)k);
        }

        private static (int R, int G, int B) DeviceCMYKToRGB(double c, double m, double y, double k)
        {
            c = ClampPercent(c) * 0.01;
            m = ClampPercent(m) * 0.01;
            y = ClampPercent(y) * 0.01;
            k = ClampPercent(k) * 0.01;

            return (
                (byte)Math.Round(255 * (1 - c) * (1 - k)),
                (byte)Math.Round(255 * (1 - m) * (1 - k)),
                (byte)Math.Round(255 * (1 - y) * (1 - k)));
        }

        private static byte ToBytePercent(double value)
        {
            return (byte)Math.Round(ClampPercent(value) * 255.0 / 100.0);
        }

        private static double ClampPercent(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return 0;
            }

            return Math.Max(0, Math.Min(100, value));
        }

        private static int ToRgbChannel(ushort value)
        {
            return (int)Math.Round(value / 257.0);
        }
    }
}
