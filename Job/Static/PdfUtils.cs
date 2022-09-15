using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Ghostscript.NET.Rasterizer;
using ImageMagick;
using Interfaces;
using Interfaces.PdfUtils;
using PDFlib_dotnet;

namespace Job.Static
{
    public static class PdfUtils
    {
        

        public static void GetColorspaces(IFileSystemInfoExt sfi)
        {
            var ext = sfi.FileInfo.Extension.ToLower();

            switch (ext)
            {
                case ".pdf":
                    GetColorspacePdf(sfi);
                    break;
                case ".psd":
                case ".jpeg":
                case ".jpg":
                case ".tif":
                case ".tiff":
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
              Logger.Log.Error(null, "GetColorspaceImage",e.Message);
            }
           // Read from file
           

            //Console.WriteLine(info.Width);
            //Console.WriteLine(info.Height);
            //Console.WriteLine(info.ColorSpace);
            //Console.WriteLine(info.Format);
            //Console.WriteLine(info.Density.X);
            //Console.WriteLine(info.Density.Y);
            //Console.WriteLine(info.Density.Units);
        }

        private static void GetColorspacePdf(IFileSystemInfoExt sfi)
        {
            var colorList = new List<string>();
            if (File.Exists(sfi.FileInfo.FullName))
            {
                var p = new PDFlib();
                try
                {
                    _ = p.begin_document("", "");

                    var doc = p.open_pdi_document(sfi.FileInfo.FullName, "");

                    var pagecount = p.pcos_get_number(doc, "length:pages"); // узнаем количество страниц

                    for (int i = 1; i <= pagecount; i++)
                    {
                        var page = p.open_pdi_page(doc, i, ""); // открываем страницу
                        colorList.AddRange(PrintColorspaces(p, doc, page));
                    }

                }
                catch (Exception e)
                {
                    Logger.Log.Error(null, "GetColorspacePdf", e.Message);
                }
                finally
                {
                    p.Dispose();
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

        private static List<string> PrintColorspaces(PDFlib p,int doc, int page)
        {
            var colorList = new List<string>();

            int colorspacecount = (int)p.pcos_get_number(doc, $"length:pages[{page}]/colorspaces");

            if (colorspacecount > 0)
            {
               

                for (int i = 0; i < colorspacecount; i++)
                {
                    var opt = $"pages[{page}]/colorspaces[{i}]";
                    //colorList.Add(opt);
                    //Debug.WriteLine("main:" + opt);
                    colorList.AddRange(PrintColorspace(p,doc,i, opt));
                }

                //var s = _colorList.Except(_ignoreColorList);
                //string output = string.Empty;
                //if (s.Any()) output = s.Aggregate((c, n) => c + ", " + n);

                //Console.Write(output);

            }

            return colorList;
        }

        private static List<string> PrintColorspace(PDFlib p, int doc,int page, string colorSpacePath)
        {

            var colorlist = new List<string>();

            var colorspace = p.pcos_get_string(doc, $"{colorSpacePath}/name");

            //colorlist.Add(colorspace);

            
            //Debug.WriteLine($"ColorSpace name: {colorspace}");

            if (colorspace.Equals("Separation"))
            {
                var colorant = p.pcos_get_string(doc, $"{colorSpacePath}/colorantname");
                colorlist.Add(colorant);
                //Debug.WriteLine($"  ColorrantName: {colorant}");
                //if (!_colorList.Contains(colorant)) _colorList.Add(colorant);

            //    int alternateid = (int)_p.pcos_get_number(_doc, colorSpacePath + "/alternateid");
            //    PrintColorspace("colorspaces[" + alternateid + "]");
            }
            else if (colorspace.Equals("DeviceN"))
            {
                var colorantcount = (int)p.pcos_get_number(doc, $"length:{colorSpacePath}/colorantnames");

            //    Debug.WriteLine($"Count colorant: {colorantcount}");

                for (var j = 0; j < colorantcount; j += 1)
                {
                    var colorname = p.pcos_get_string(doc, $"{colorSpacePath}/colorantnames[{j}]");
                    colorlist.Add(colorname);
            //        Debug.WriteLine($"   Color name: {colorname}");

            //        if (!_colorList.Contains(colorname)) _colorList.Add(colorname);

                }
            //    var alternateid = (int)_p.pcos_get_number(_doc, colorSpacePath + "/alternateid");
            //    PrintColorspace("colorspaces[" + alternateid + "]");
            }
            else
            {
                colorlist.Add(colorspace);
            }

            return colorlist;


        }

        public static void SetTrimBoxWidth(string fullName, object newValue)
        {
            if (float.TryParse(newValue.ToString(), out float res))
            {
                if (File.Exists(fullName) && Path.GetExtension(fullName).ToLower().Equals(".pdf"))
                {
                    _setTrimBox(fullName, res);
                }
            }
            else
            {
                throw new Exception("PdfUtils->SetTrimBoxWidth: width not decimal or file not exist");
            }
        }

        internal static void SetTrimBoxHeight(string fullName, object newValue)
        {
           
        }


        static  void _setTrimBox(string fileName, float width)
        {

        }

        public static void PdfToJpg(string fileName,int dpi)
        {
            try
            {
                using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
                {
                    byte[] buffer = File.ReadAllBytes(fileName);
                    MemoryStream ms = new MemoryStream(buffer);
                    rasterizer.Open(ms);

                    for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                    {
                        string output = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + $"_{pageNumber}.jpg");
                        string pageFilePath = output;

                        var img = rasterizer.GetPage(dpi, pageNumber);
                        img.Save(pageFilePath, ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception e)
            {
                string output = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".log");
                File.WriteAllText(output, e.Message);
            }
        }
    }
}
