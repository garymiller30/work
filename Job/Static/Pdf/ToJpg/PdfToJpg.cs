using Ghostscript.NET.Rasterizer;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Dlg;
using SkiaSharp;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.ToJpg
{
    [PdfTool("", "Конвертувати PDF в JPG", Icon = "convert_to_jpg")]
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

                using var rasterizer = new GhostscriptRasterizer();
                using var fs = File.OpenRead(filePath);

                rasterizer.Open(fs);

                for (int page = 1; page <= rasterizer.PageCount; page++)
                {
                    var outFile = Path.Combine(directory,
                      $"{baseName}_page_{page:D3}.jpg");

                    using var bitmap = rasterizer.GetPage(_params.Dpi, page);
                    using var image = SKImage.FromBitmap(bitmap);
                    using var data = image.Encode(SKEncodedImageFormat.Jpeg,
                                                   (int)_params.Quality);

                    File.WriteAllBytes(outFile, data.ToArray());
                }

            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "PdfToJpg", $"Помилка конвертації PDF в JPG: {e.Message}");
            }
        }
    }
}
