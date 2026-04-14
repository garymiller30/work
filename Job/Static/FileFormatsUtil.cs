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
        public const decimal Mn = (decimal)2.83465;


        private static readonly string[] Extension = { ".tif", ".tiff", ".pdf" };

        public static bool IsExistExtension(string ext)
        {
            var lowEx = ext.ToLower(CultureInfo.InvariantCulture);
            return Extension.FirstOrDefault(x => x.Equals(lowEx)) != null;
        }


        public static void GetFormat(IFileSystemInfoExt sfi)
        {
            var ext = sfi.FileInfo.Extension.ToLower(CultureInfo.InvariantCulture);

            switch (ext)
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
                case ".ai":
                case ".pdf":
                    GetPdf(sfi);
                    break;
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
            catch
            {
            }
        }

        

     

    }
}
