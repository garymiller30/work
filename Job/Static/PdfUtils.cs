using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ghostscript.NET.Rasterizer;
using ImageMagick;
using Interfaces;
using Interfaces.PdfUtils;
using iText.Kernel.Pdf;
using JobSpace.Static.Pdf.ColorSpaces;
using JobSpace.Static.Pdf.Common;
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
                    GetPdfColorspaceUnified(sfi);
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
            if (!File.Exists(sfi.FileInfo.FullName))
            {
                return;
            }

            var colorList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                using (PdfReader reader = new PdfReader(sfi.FileInfo.FullName))
                using (PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        var page = pdfDoc.GetPage(i);
                        ExtractPageColorSpaces(page, colorList);

                        foreach (var color in PdfColorExtractor.ExtractColorsFromPage(page))
                        {
                            colorList.Add(color);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "GetColorspacePdf", e.Message);
            }

            if (colorList.Any())
            {
                sfi.UsedColors = NormalizeColors(colorList);
            }
        }

        private static void ExtractPageColorSpaces(PdfPage page, HashSet<string> colorList)
        {
            if (page == null)
            {
                return;
            }

            ExtractResourceColorSpaces(page.GetResources()?.GetPdfObject(), colorList);
        }

        private static void ExtractResourceColorSpaces(PdfDictionary resources, HashSet<string> colorList)
        {
            if (resources == null)
            {
                return;
            }

            var colorSpaces = resources.GetAsDictionary(PdfName.ColorSpace);
            if (colorSpaces != null)
            {
                foreach (var entry in colorSpaces.EntrySet())
                {
                    ExtractColorFromPdfObject(entry.Value, colorList);
                }
            }

            var xObjects = resources.GetAsDictionary(PdfName.XObject);
            if (xObjects == null)
            {
                return;
            }

            foreach (var entry in xObjects.EntrySet())
            {
                PdfObject xObject = entry.Value;

                if (xObject == null)
                {
                    continue;
                }

                if (xObject.IsIndirectReference())
                {
                    xObject = ((PdfIndirectReference)xObject).GetRefersTo();
                }

                if (!(xObject is PdfStream stream))
                {
                    continue;
                }

                if (PdfName.Image.Equals(stream.GetAsName(PdfName.Subtype)))
                {
                    ExtractImageColorFromPdfObject(stream.Get(PdfName.ColorSpace), colorList);
                }

                ExtractResourceColorSpaces(stream.GetAsDictionary(PdfName.Resources), colorList);
            }
        }

        private static void ExtractColorFromPdfObject(PdfObject obj, HashSet<string> colorList)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.IsIndirectReference())
            {
                ExtractColorFromPdfObject(((PdfIndirectReference)obj).GetRefersTo(), colorList);
                return;
            }

            if (obj.IsName())
            {
                var name = ((PdfName)obj).GetValue();
                switch (name)
                {
                    case "DeviceGray":
                    case "Gray":
                        colorList.Add("Gray");
                        break;
                    case "DeviceCMYK":
                    case "DeviceRGB":
                    case "Indexed":
                        break;
                    default:
                        colorList.Add(NormalizePdfColorName(name));
                        break;
                }
                return;
            }

            if (!obj.IsArray())
            {
                return;
            }

            var array = (PdfArray)obj;
            if (array.Size() == 0)
            {
                return;
            }

            var colorSpaceName = array.GetAsName(0)?.GetValue();
            if (string.IsNullOrWhiteSpace(colorSpaceName))
            {
                return;
            }

            switch (colorSpaceName)
            {
                case "Separation":
                    var separationName = array.GetAsName(1)?.GetValue();
                    if (!string.IsNullOrWhiteSpace(separationName))
                    {
                        colorList.Add(NormalizePdfColorName(separationName));
                    }
                    break;
                case "DeviceN":
                    var names = array.GetAsArray(1);
                    if (names != null)
                    {
                        for (int i = 0; i < names.Size(); i++)
                        {
                            var colorName = names.GetAsName(i)?.GetValue();
                            if (!string.IsNullOrWhiteSpace(colorName))
                            {
                                colorList.Add(NormalizePdfColorName(colorName));
                            }
                        }
                    }
                    break;
                case "ICCBased":
                    var profile = array.GetAsStream(1);
                    var components = profile?.GetAsNumber(PdfName.N)?.IntValue();
                    var iccProfileLabel = GetIccProfileLabel(components);
                    if (!string.IsNullOrWhiteSpace(iccProfileLabel))
                    {
                        colorList.Add(iccProfileLabel);
                    }
                    if (components == 1)
                    {
                        colorList.Add("Gray");
                    }
                    break;
                case "Indexed":
                    break;
                default:
                    ExtractColorFromPdfObject(array.Get(0), colorList);
                    break;
            }
        }

        private static void ExtractImageColorFromPdfObject(PdfObject obj, HashSet<string> colorList)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.IsIndirectReference())
            {
                ExtractImageColorFromPdfObject(((PdfIndirectReference)obj).GetRefersTo(), colorList);
                return;
            }

            if (obj.IsName())
            {
                var name = ((PdfName)obj).GetValue();
                switch (name)
                {
                    case "DeviceRGB":
                        colorList.Add("RGB");
                        break;
                    case "DeviceGray":
                    case "Gray":
                        colorList.Add("Gray");
                        break;
                    default:
                        ExtractColorFromPdfObject(obj, colorList);
                        break;
                }
                return;
            }

            if (obj.IsArray())
            {
                var array = (PdfArray)obj;
                if (array.Size() == 0)
                {
                    return;
                }

                var colorSpaceName = array.GetAsName(0)?.GetValue();
                if (string.Equals(colorSpaceName, "ICCBased", StringComparison.OrdinalIgnoreCase))
                {
                    var profile = array.GetAsStream(1);
                    var components = profile?.GetAsNumber(PdfName.N)?.IntValue();
                    var iccProfileLabel = GetIccProfileLabel(components);

                    if (!string.IsNullOrWhiteSpace(iccProfileLabel))
                    {
                        colorList.Add(iccProfileLabel);
                    }

                    if (components == 3)
                    {
                        colorList.Add("RGB");
                        return;
                    }

                    if (components == 1)
                    {
                        colorList.Add("Gray");
                        return;
                    }
                }
            }

            ExtractColorFromPdfObject(obj, colorList);
        }

        private static void GetPdfColorspace(IFileSystemInfoExt sfi)
        {
            var uc = new HashSet<string>();
            using (PdfReader reader = new PdfReader(sfi.FileInfo.FullName))
            using (PdfDocument pdfDoc = new PdfDocument(reader))
            {
                int numberOfPages = pdfDoc.GetNumberOfPages();
                Debug.WriteLine($"Кількість сторінок: {numberOfPages}");

                for (int i = 1; i <= numberOfPages; i++)
                {
                    List<string> pageColors = PdfColorExtractor.ExtractColorsFromPage(pdfDoc.GetPage(i));

                    foreach (string color in pageColors.OrderBy(s => s))
                    {
                        uc.Add(color);
                    }
                }
            }

            if (uc.Contains("CMYK") && uc.Contains("K"))
            {
                uc.Remove("K");
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

            sfi.UsedColors = NormalizeColors(uc);
        }

        private static void GetPdfColorspaceUnified(IFileSystemInfoExt sfi)
        {
            var allColors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var plateNames = ExtractPlateNamesFromMetadata(sfi.FileInfo.FullName).ToList();

            foreach (var color in plateNames)
            {
                allColors.Add(color);
            }

            var signaColors = new SignaColorExtractor().Extract(sfi.FileInfo.FullName);
            foreach (var color in signaColors)
            {
                allColors.Add(color);
            }

            GetColorspacePdf(sfi);
            if (sfi.UsedColors != null)
            {
                foreach (var color in sfi.UsedColors)
                {
                    allColors.Add(color);
                }
            }

            GetPdfColorspace(sfi);
            if (sfi.UsedColors != null)
            {
                foreach (var color in sfi.UsedColors)
                {
                    allColors.Add(color);
                }
            }

            if (allColors.Count > 0)
            {
                sfi.UsedColors = NormalizeColors(allColors);
                ApplyPlateNamesAuthority(sfi.UsedColors, plateNames);
            }
        }

        private static HashSet<string> NormalizeColors(IEnumerable<string> colors)
        {
            var normalized = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var color in colors.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                normalized.Add(NormalizePdfColorName(color));
            }

            bool hasC = normalized.Contains("Cyan");
            bool hasM = normalized.Contains("Magenta");
            bool hasY = normalized.Contains("Yellow");
            bool hasK = normalized.Contains("K") || normalized.Contains("Black") || normalized.Contains("Gray");

            if (normalized.Contains("CMYK"))
            {
                hasC = true;
                hasM = true;
                hasY = true;
                hasK = true;
            }

            foreach (var compactProcessColor in normalized.Where(IsCompactProcessColor).ToList())
            {
                if (compactProcessColor.IndexOf('C') >= 0) hasC = true;
                if (compactProcessColor.IndexOf('M') >= 0) hasM = true;
                if (compactProcessColor.IndexOf('Y') >= 0) hasY = true;
                if (compactProcessColor.IndexOf('K') >= 0) hasK = true;
            }

            normalized.Remove("CMYK");
            normalized.Remove("Cyan");
            normalized.Remove("Magenta");
            normalized.Remove("Yellow");
            normalized.Remove("K");
            normalized.Remove("Black");
            normalized.Remove("Gray");
            foreach (var compactProcessColor in normalized.Where(IsCompactProcessColor).ToList())
            {
                normalized.Remove(compactProcessColor);
            }

            var processColors = string.Concat(
                hasC ? "C" : string.Empty,
                hasM ? "M" : string.Empty,
                hasY ? "Y" : string.Empty,
                hasK ? "K" : string.Empty);

            if (!string.IsNullOrWhiteSpace(processColors))
            {
                normalized.Add(processColors);
            }

            return normalized;
        }

        private static bool IsCompactProcessColor(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            foreach (var ch in value)
            {
                if (ch != 'C' && ch != 'M' && ch != 'Y' && ch != 'K')
                {
                    return false;
                }
            }

            return true;
        }

        private static void ApplyPlateNamesAuthority(HashSet<string> colors, List<string> plateNames)
        {
            if (colors == null || plateNames == null || plateNames.Count == 0)
            {
                return;
            }

            var authoritativeProcessColor = BuildProcessColorFromPlateNames(plateNames);
            var compactProcessColors = colors.Where(IsCompactProcessColor).ToList();

            foreach (var compactProcessColor in compactProcessColors)
            {
                colors.Remove(compactProcessColor);
            }

            if (!string.IsNullOrWhiteSpace(authoritativeProcessColor))
            {
                colors.Add(authoritativeProcessColor);
            }
        }

        private static string BuildProcessColorFromPlateNames(IEnumerable<string> plateNames)
        {
            bool hasC = false;
            bool hasM = false;
            bool hasY = false;
            bool hasK = false;

            foreach (var plateName in plateNames.Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                switch (NormalizePdfColorName(plateName))
                {
                    case "Cyan":
                        hasC = true;
                        break;
                    case "Magenta":
                        hasM = true;
                        break;
                    case "Yellow":
                        hasY = true;
                        break;
                    case "K":
                    case "Black":
                    case "Gray":
                        hasK = true;
                        break;
                }
            }

            return string.Concat(
                hasC ? "C" : string.Empty,
                hasM ? "M" : string.Empty,
                hasY ? "Y" : string.Empty,
                hasK ? "K" : string.Empty);
        }

        private static IEnumerable<string> ExtractPlateNamesFromMetadata(string filePath)
        {
            if (!File.Exists(filePath))
            {
                yield break;
            }

            string content;
            try
            {
                content = Encoding.GetEncoding("iso-8859-1").GetString(File.ReadAllBytes(filePath));
            }
            catch
            {
                yield break;
            }

            var plateNamesMatch = Regex.Match(
                content,
                @"<xmpTPg:PlateNames>\s*<rdf:Seq>(.*?)</rdf:Seq>\s*</xmpTPg:PlateNames>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (!plateNamesMatch.Success)
            {
                yield break;
            }

            foreach (Match item in Regex.Matches(plateNamesMatch.Groups[1].Value, @"<rdf:li>(.*?)</rdf:li>", RegexOptions.Singleline | RegexOptions.IgnoreCase))
            {
                var value = item.Groups[1].Value?.Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    yield return value;
                }
            }
        }

        private static string GetIccProfileLabel(int? components)
        {
            switch (components)
            {
                case 4:
                    return "ICC Profile (CMYK)";
                case 3:
                    return "ICC Profile (RGB)";
                case 1:
                    return "ICC Profile (Gray)";
                default:
                    return null;
            }
        }

        private static string NormalizePdfColorName(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return color;
            }

            switch (color.Trim().TrimStart('/').Replace("#20", " "))
            {
                case "DeviceCMYK":
                case "Cmyk":
                    return "CMYK";
                case "DeviceRGB":
                case "CalRGB":
                case "Rgb":
                    return "RGB";
                case "DeviceGray":
                case "CalGray":
                case "Gray":
                    return "Gray";
                default:
                    return color.Trim().TrimStart('/').Replace("#20", " ");
            }
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
