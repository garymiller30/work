using Ghostscript.NET.Rasterizer;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.ToJpg
{
    public sealed class PdfToJpg
    {
        PdfToJpgParams _params;

        public PdfToJpg(PdfToJpgParams param)
        {
            _params = param;
        }

        public void Run(string filePath)
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
