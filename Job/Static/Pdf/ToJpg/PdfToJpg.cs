using Ghostscript.NET.Rasterizer;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Dlg;
using JobSpace.Static.Pdf.Imposition.Models;
using SkiaSharp;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.ToJpg
{
    [PdfTool("","Конвертувати PDF в JPG",Icon = "convert_to_jpg")]
    public sealed class PdfToJpg : IPdfTool
    {
        PdfToJpgParams _params;
        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormSelectDpi())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _params = new PdfToJpgParams { Dpi = form.Dpi, Quality = form.Quality };
                    return true;
                }
            }
            return false;
        }
        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
                ToJpg(file.FullName);
        }

      

        public void ToJpg(string filePath)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);
                string baseName = Path.GetFileNameWithoutExtension(filePath);

                using (var rasterizer = new GhostscriptRasterizer())
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    rasterizer.Open(fs);

                    int pageCount = rasterizer.PageCount;

                    for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
                    {
                        string outputPath = Path.Combine(
                            directory,
                            $"{baseName}_page_{pageNumber:D3}.jpg");

                        using (SKBitmap bitmap = rasterizer.GetPage(_params.Dpi, pageNumber))
                        {
                            using (SKImage image = SKImage.FromBitmap(bitmap))
                            using (SKData data = image.Encode(SKEncodedImageFormat.Jpeg, (int)_params.Quality))
                            using (FileStream outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                data.SaveTo(outStream);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string output = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".log");
                File.WriteAllText(output, e.Message);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

       
    }
}
