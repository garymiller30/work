using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ImageMagick;
using Interfaces;
using Interfaces.PdfUtils;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace JobSpace.Static
{


    public static class FileFormatsUtil
    {
        // Константа для конвертації точок (pt) у міліметри (mm)
        // 1 pt = 1/72 inch, 1 inch = 25.4 mm => 1 pt = 25.4 / 72 ≈ 0.35277 mm
        // Mn (2.83465) це фактично 72 / 25.4
        public const decimal Mn = 2.83465M;


        public static void GetFormat(IFileSystemInfoExt sfi)
        {
            var ext = sfi.FileInfo.Extension;

            switch (ext.ToLowerInvariant())
            {
                case ".psd":
                case ".eps":
                    GetPsd(sfi);
                    break;
                case ".jpg":
                case ".tif":
                case ".tiff":
                case ".png":
                    GetTif(sfi);
                    break;
                case ".heic":
                    GetHeic(sfi);
                    break;
                case ".ai":
                case ".pdf":
                    GetPdf(sfi);
                    break;
            }

        }

        private static void GetHeic(IFileSystemInfoExt sfi)
        {
            try
            {
                using (var image = new MagickImage(sfi.FileInfo.FullName))
                {
                    // 2. Get the width and height in pixels
                    int pixelWidth = (int)image.Width;
                    int pixelHeight = (int)image.Height;

                    // 3. Get the X and Y density (Resolution in Pixels Per Inch)
                    // Note: If your image reports DensityUnits.PixelsPerCentimeter, adjust the math accordingly
                    double dpiX = image.Density.X;
                    double dpiY = image.Density.Y;

                    if (dpiX > 0 && dpiY > 0)
                    {
                        sfi.Format = new FileFormat
                        {
                            Width = (decimal)((pixelWidth / dpiX) * 25.4),
                            Height = (decimal)((pixelHeight / dpiY) * 25.4),
                            Bleeds = (decimal)((dpiX + dpiY) / 2),
                        };
                    }
                    else
                    {
                        Console.WriteLine("DPI density not found in the HEIC metadata.");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "GetHeic", $"Error getting format for file {sfi.FileInfo.FullName}: {e.Message}");
            }
        }

        private static void GetPsd(IFileSystemInfoExt sfi)
        {
            try
            {
                MagickImageInfo info = new MagickImageInfo(sfi.FileInfo.FullName);

                sfi.Format = new FileFormat
                {
                    Width = info.Width * 25.4M / (decimal)info.Density.X,
                    Height = info.Height * 25.4M / (decimal)info.Density.Y,
                    Bleeds = (decimal)(info.Density.X + info.Density.Y) / 2,
                };
            }
            catch
            {
            }
        }

        private static void GetPdf(IFileSystemInfoExt sfi)
        {
            #region [USING PDFLIB]
            //var boxes = PdfHelper.GetPagesInfo(sfi.FileInfo.FullName);
            //if (boxes.Count == 0) return;
            //var box = boxes[0];
            //sfi.Format = new FileFormat
            //{
            //    Width = (decimal)box.Trimbox.wMM(),
            //    Height = (decimal)box.Trimbox.hMM(),
            //    Bleeds = (decimal)((box.Mediabox.wMM() - box.Trimbox.wMM()) / 2),
            //    cntPages = boxes.Count(),
            //};
            #endregion
            #region [USING ITEXT]
            Rectangle media = null;

            PdfReader pdfReader = null;
            int pages = 0;

            try
            {
                pdfReader = new PdfReader(sfi.FileInfo.FullName);
                pages = pdfReader.NumberOfPages;
                media = pdfReader.GetBoxSize(1, "media");
                var rect = pdfReader.GetBoxSize(1, "trim");
                pdfReader.Dispose();

                if (rect == null)
                {
                    sfi.Format = new FileFormat
                    {
                        Width = (decimal)media.Width / Mn,
                        Height = (decimal)media.Height / Mn,
                        Bleeds = 0,
                        cntPages = pages,
                    };
                }
                else
                {
                    sfi.Format = new FileFormat
                    {
                        Width = (decimal)rect.Width / Mn,
                        Height = (decimal)rect.Height / Mn,
                        Bleeds = (decimal)(media.Width - rect.Width) / 2 / Mn,
                        cntPages = pages,
                    };
                }
            }
            catch
            {
                if (media != null)
                {
                    sfi.Format = new FileFormat()
                    {
                        Width = (decimal)media.Width / Mn,
                        Height = (decimal)media.Height / Mn,
                        Bleeds = 0,
                        cntPages = pages,
                    };
                }
                else
                {
                    sfi.Format = new FileFormat();
                }
            }
            finally
            {
                pdfReader?.Dispose();
            }
            #endregion
        }

        private static void GetTif(IFileSystemInfoExt sfi)
        {
            try
            {
                using (var stream = new FileStream(sfi.FileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var tif = System.Drawing.Image.FromStream(stream, false, false))
                    {
                        var width = tif.PhysicalDimension.Width;
                        var height = tif.PhysicalDimension.Height;
                        var hresolution = tif.HorizontalResolution;
                        var vresolution = tif.VerticalResolution;

                        sfi.Format = new FileFormat
                        {
                            Width = (decimal)(width / (hresolution / 25.4F)),
                            Height = (decimal)(height / (vresolution / 25.4F)),
                            Bleeds = (decimal)hresolution,
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "GetTif", $"Error getting format for file {sfi.FileInfo.FullName}: {e.Message}");
            }
        }





    }
}
