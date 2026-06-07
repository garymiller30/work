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
        public const decimal Mn = 2.83465M;
        private const decimal MmPerInch = 25.4m;

        public static void GetFormat(IFileSystemInfoExt sfi)
        {
            if (sfi == null || sfi.FileInfo == null)
                return;

            var ext = sfi.FileInfo.Extension?.ToLowerInvariant() ?? string.Empty;

            Action<IFileSystemInfoExt>? action = ext switch
            {
                ".psd" or ".eps" => GetPsd,
                ".jpg" or ".tif" or ".tiff" or ".png" => GetTif,
                ".heic" => GetHeic,
                ".ai" or ".pdf" => GetPdf,
                _ => null
            };

            action?.Invoke(sfi);
        }

        private static void GetHeic(IFileSystemInfoExt sfi)
        {
            if (sfi == null || string.IsNullOrEmpty(sfi.FileInfo?.FullName))
                return;

            try
            {
                using var image = new MagickImage(sfi.FileInfo.FullName);

                var (pixelWidth, pixelHeight) = ((int)image.Width, (int)image.Height);

                // Density may be nullable on some Magick types; guard it.
                var density = image.Density;
                if (density != null && density.X > 0 && density.Y > 0)
                {
                    decimal resX = (decimal)density.X;
                    decimal resY = (decimal)density.Y;

                    sfi.Format = new()
                    {
                        Width = (pixelWidth / resX) * MmPerInch,
                        Height = (pixelHeight / resY) * MmPerInch,
                        Bleeds = (resX + resY) / 2m
                    };
                }
                else
                {
                    Console.WriteLine("DPI density not found in the HEIC metadata.");
                    // leave Format unset or set to empty struct
                    sfi.Format = new FileFormat();
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, nameof(GetHeic), $"Error getting format for file {sfi.FullName}: {e.Message}");
            }
        }

        private static void GetPsd(IFileSystemInfoExt sfi)
        {
            if (sfi == null || string.IsNullOrEmpty(sfi.FileInfo?.FullName))
                return;

            try
            {
                MagickImageInfo info = new MagickImageInfo(sfi.FileInfo.FullName);

                var density = info.Density;
                if (density != null && density.X > 0 && density.Y > 0)
                {
                    sfi.Format = new FileFormat
                    {
                        Width = info.Width * MmPerInch / (decimal)density.X,
                        Height = info.Height * MmPerInch / (decimal)density.Y,
                        Bleeds = (decimal)(density.X + density.Y) / 2m,
                    };
                }
                else
                {
                    // Can't compute without density; provide empty format to avoid null deref
                    sfi.Format = new FileFormat();
                }
            }
            catch
            {
                sfi.Format = new FileFormat();
            }
        }

        private static void GetPdf(IFileSystemInfoExt sfi)
        {
            if (sfi == null || string.IsNullOrEmpty(sfi.FileInfo?.FullName))
                return;

            Rectangle? media = null;
            PdfReader? pdfReader = null;
            int pages = 0;

            try
            {
                pdfReader = new PdfReader(sfi.FileInfo.FullName);
                pages = pdfReader.NumberOfPages;
                media = pdfReader.GetBoxSize(1, "media");
                var rect = pdfReader.GetBoxSize(1, "trim");

                if (rect == null)
                {
                    if (media != null)
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
                        sfi.Format = new FileFormat();
                    }
                }
                else
                {
                    // media can be null; fallback to rect.Width when computing bleeds
                    decimal bleedWidth = 0;
                    if (media != null)
                        bleedWidth = (decimal)(media.Width - rect.Width) / 2m / Mn;

                    sfi.Format = new FileFormat
                    {
                        Width = (decimal)rect.Width / Mn,
                        Height = (decimal)rect.Height / Mn,
                        Bleeds = bleedWidth,
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
        }

        private static void GetTif(IFileSystemInfoExt sfi)
        {
            if (sfi == null || string.IsNullOrEmpty(sfi.FileInfo?.FullName))
                return;

            try
            {
                using (var stream = new FileStream(sfi.FileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var tif = System.Drawing.Image.FromStream(stream, false, false))
                    {
                        var width = (decimal)tif.PhysicalDimension.Width;
                        var height = (decimal)tif.PhysicalDimension.Height;
                        var hresolution = (decimal)tif.HorizontalResolution;
                        var vresolution = (decimal)tif.VerticalResolution;

                        sfi.Format = new FileFormat
                        {
                            Width = (width / (hresolution / MmPerInch)),
                            Height = (height / (vresolution / MmPerInch)),
                            Bleeds = hresolution,
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, nameof(GetTif), $"Error getting format for file {sfi.FullName}: {e.Message}");
            }
        }
    }
}
