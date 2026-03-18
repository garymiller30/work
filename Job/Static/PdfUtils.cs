using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Ghostscript.NET.Rasterizer;
using ImageMagick;
using Interfaces;
using Interfaces.PdfUtils;
using iText.Kernel.Pdf;
using JobSpace.Static.Pdf.ColorSpaces;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using SharpCompress.Common;

namespace JobSpace.Static
{
    public static class PdfUtils
    {
        public static void GetColorspaces(IFileSystemInfoExt sfi)
        {
            var ext = sfi.FileInfo.Extension.ToLower(System.Globalization.CultureInfo.InvariantCulture);

            switch (ext)
            {
                case ".pdf":
                    GetPdfColorspace(sfi);
                    break;
                case ".psd":
                case ".jpeg":
                case ".jpg":
                case ".tif":
                case ".tiff":
                case ".png":
                    GetColorspaceImage(sfi);
                    break;
            }
        }

        private static void GetColorspaceImage(IFileSystemInfoExt sfi)
        {

            var uc = sfi.UsedColors;

            uc.Clear();

            try
            {
                MagickImageInfo info = new MagickImageInfo(sfi.FileInfo.FullName);

                switch (info.ColorSpace)
                {
                    case ColorSpace.CMY:
                    case ColorSpace.CMYK:
                        uc.Add("CMYK");
                        break;
                    case ColorSpace.sRGB:
                    case ColorSpace.scRGB:
                    case ColorSpace.RGB:
                        uc.Add("RGB");
                        break;
                    case ColorSpace.Lab:
                        uc.Add("Lab");
                        break;
                    case ColorSpace.LinearGray:
                    case ColorSpace.Gray:
                        uc.Add("Gray");
                        break;

                    default:
                        uc.Add("Unknown");
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "GetColorspaceImage", e.Message);
            }
        }

        private static void GetColorspacePdf(IFileSystemInfoExt sfi)
        {
            var colorList = new List<string>();
            if (File.Exists(sfi.FileInfo.FullName))
            {
                var p = new PDFlib();

                try
                {
                    p.begin_document("", "");

                    var doc = p.open_pdi_document(sfi.FileInfo.FullName, "");

                    var pagecount = p.pcos_get_number(doc, "length:pages"); // кількість сторінок

                    for (int i = 1; i <= pagecount; i++)
                    {
                        var page = p.open_pdi_page(doc, i, ""); // 
                        colorList.AddRange(PrintColorspaces(p, doc, page));
                    }

                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "GetColorspacePdf");
                }
                finally
                {
                    p?.Dispose();
                }


            }

            if (colorList.Any())
            {
                var uc = sfi.UsedColors;
                uc.Clear();


                var remDup = colorList.Distinct().OrderBy(s => s).ToList();

                foreach (var colorspace in remDup)
                {
                    uc.Add(colorspace);
                }

            }
        }

        private static List<string> PrintColorspaces(PDFlib p, int doc, int page)
        {
            var colorList = new List<string>();

            int colorspacecount = (int)p.pcos_get_number(doc, $"length:pages[{page}]/colorspaces");

            if (colorspacecount > 0)
            {
                for (int i = 0; i < colorspacecount; i++)
                {
                    var opt = $"pages[{page}]/colorspaces[{i}]";
                    colorList.AddRange(PrintColorspace(p, doc, i, opt));
                }
            }
            return colorList;
        }

        private static List<string> PrintColorspace(PDFlib p, int doc, int page, string colorSpacePath)
        {
            var colorlist = new List<string>();
            var colorspace = p.pcos_get_string(doc, $"{colorSpacePath}/name");

            if (colorspace.Equals("Separation"))
            {
                var colorant = p.pcos_get_string(doc, $"{colorSpacePath}/colorantname");
                colorlist.Add(colorant);
            }
            else if (colorspace.Equals("DeviceN"))
            {
                var colorantcount = (int)p.pcos_get_number(doc, $"length:{colorSpacePath}/colorantnames");

                for (var j = 0; j < colorantcount; j += 1)
                {
                    var colorname = p.pcos_get_string(doc, $"{colorSpacePath}/colorantnames[{j}]");
                    colorlist.Add(colorname);
                }
            }
            else
            {
                colorlist.Add(colorspace);
            }
            return colorlist;
        }

        private static void GetPdfColorspace(IFileSystemInfoExt sfi)
        {
            int numberOfPages = 0;
            using (PdfReader reader = new PdfReader(sfi.FileInfo.FullName))
            using (PdfDocument pdfDoc = new PdfDocument(reader))
            {
                numberOfPages = pdfDoc.GetNumberOfPages();
                Debug.WriteLine($"Кількість сторінок: {numberOfPages}");
            }

            var uc = new HashSet<string>();
            //uc.Clear();

            for (int i = 1; i <= numberOfPages; i++)
            {
                List<string> pageColors = PdfColorExtractor.ExtractColorsFromPage(sfi.FileInfo.FullName, i);

                //Debug.WriteLine($"Знайдені унікальні кольори на сторінці {i}:");
                foreach (string color in pageColors.OrderBy(s => s))
                {
                    uc.Add(color);
                }
            }
            // якщо в uc є CMYK і K то видаляємо K, бо це вже вказано в CMYK
            if (uc.Contains("CMYK") && uc.Contains("K"))
            {
                uc.Remove("K");
            }
            if (uc.Contains("CMYK") && uc.Contains("Black"))
            {
                uc.Remove("Black");
            }
            // якщо в uc є Cyan і є CMYK то видаляємо, бо це вже вказано в CMYK
            if (uc.Contains("CMYK") && uc.Contains("Cyan"))
            {
                uc.Remove("Cyan");
            }
            if (uc.Contains("CMYK") && uc.Contains("Magenta"))
            {
                uc.Remove("Magenta");
            }
            if (uc.Contains("CMYK") && uc.Contains("Yellow"))
            {
                uc.Remove("Yellow");
            }

            sfi.UsedColors = uc;
        }

        public static void GetFileCreator(IFileSystemInfoExt file)
        {
            var ext = file.FileInfo.Extension.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            switch (ext)
            {
                case ".pdf":
                    PdfHelper.GetPdfCreatorApp(file);
                    break;
                case ".psd":
                case ".jpeg":
                case ".jpg":
                case ".tif":
                case ".tiff":
                case ".png":
                    break;
            }
        }
    }
}
