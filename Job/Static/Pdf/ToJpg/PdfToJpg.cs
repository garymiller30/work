using Ghostscript.NET.Rasterizer;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Dlg;
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
                using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
                {
                    byte[] buffer = File.ReadAllBytes(filePath);
                    MemoryStream ms = new MemoryStream(buffer);
                    rasterizer.Open(ms);

                    for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                    {
                        string output = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + $"_{pageNumber}.jpg");
                        string pageFilePath = output;

                        var img = rasterizer.GetPage(_params.Dpi, pageNumber);

                        ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
                        EncoderParameters encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, _params.Quality);
                        img.Save(pageFilePath, jpegCodec, encoderParameters);
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
