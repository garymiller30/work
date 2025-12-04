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
            sfi.UsedColorSpace = 0;

            try
            {
                MagickImageInfo info = new MagickImageInfo(sfi.FileInfo.FullName);

                switch (info.ColorSpace)
                {
                    case ColorSpace.CMY:
                    case ColorSpace.CMYK:
                        sfi.UsedColorSpace |= ColorSpaces.Cmyk;
                        break;
                    case ColorSpace.sRGB:
                    case ColorSpace.scRGB:
                    case ColorSpace.RGB:
                        sfi.UsedColorSpace |= ColorSpaces.Rgb;
                        break;
                    case ColorSpace.Lab:
                        sfi.UsedColorSpace |= ColorSpaces.Lab;
                        break;
                    case ColorSpace.LinearGray:
                    case ColorSpace.Gray:
                        sfi.UsedColorSpace |= ColorSpaces.Gray;
                        break;

                    default:
                        sfi.UsedColorSpace |= ColorSpaces.Unknown;
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
                sfi.UsedColorSpace = 0;

                var remDup = colorList.Distinct().ToList();

                foreach (var colorspace in remDup)
                {

                    if (colorspace.StartsWith("pantone", StringComparison.OrdinalIgnoreCase))
                    {
                        sfi.UsedColorSpace |= ColorSpaces.Spot;
                        continue;
                    }

                    switch (colorspace)
                    {
                        case "Black":
                        case "Yellow":
                        case "Magenta":
                        case "Cyan":
                        case "DeviceCMYK":
                            sfi.UsedColorSpace |= ColorSpaces.Cmyk;
                            break;

                        case "DeviceRGB":
                            sfi.UsedColorSpace |= ColorSpaces.Rgb;
                            break;
                        case "DeviceGray":
                            sfi.UsedColorSpace |= ColorSpaces.Gray;
                            break;
                        case "Indexed":
                            sfi.UsedColorSpace |= ColorSpaces.Pattern;
                            break;
                        case "ICCBased":
                            sfi.UsedColorSpace |= ColorSpaces.ICCBased;
                            break;
                        case "All":
                            sfi.UsedColorSpace |= ColorSpaces.All;
                            break;

                        case "ProofColor":
                            sfi.UsedColorSpace |= ColorSpaces.Spot;
                            break;

                        default:
                            sfi.UsedColorSpace |= ColorSpaces.Unknown;
                            break;
                    }
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

            for (int i = 1; i <= numberOfPages; i++)
            {
                List<string> pageColors = PdfColorExtractor.ExtractColorsFromPage(sfi.FileInfo.FullName, i);

                Debug.WriteLine($"Знайдені унікальні кольори на сторінці {i}:");
                sfi.UsedColorSpace = 0;
                if (pageColors.Count > 0)
                {
                    
                    foreach (string color in pageColors)
                    {
                        var c= color.ToLower();

                        switch (c)
                        {
                            case "cmyk":
                                sfi.UsedColorSpace |= ColorSpaces.Cmyk;
                                break;
                            case "lab":
                                sfi.UsedColorSpace |= ColorSpaces.Lab;
                                break;
                            case "rgb":
                                sfi.UsedColorSpace |= ColorSpaces.Rgb;
                                break;
                            case "grayscale":
                                sfi.UsedColorSpace |= ColorSpaces.Gray;
                                break;
                            case "indexed":
                                sfi.UsedColorSpace |= ColorSpaces.Pattern;
                                break;
                            case "iccbased":
                                sfi.UsedColorSpace |= ColorSpaces.ICCBased;
                                break;
                            case "spot":
                                sfi.UsedColorSpace |= ColorSpaces.Spot;
                                break;
                            default:
                                sfi.UsedColorSpace |= ColorSpaces.Unknown;
                                break;
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Кольорів не знайдено або сталася помилка.");
                }

            }
        }
    }
}
