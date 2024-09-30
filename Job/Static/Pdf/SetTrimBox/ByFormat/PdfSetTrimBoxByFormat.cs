using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Convert;
using JobSpace.Static.Pdf.Scale;
using PDFlib_dotnet;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.SetTrimBox.ByFormat
{
    public sealed class PdfSetTrimBoxByFormat : SetTrimBoxBase
    {
        PdfSetTrimBoxByFormatParams _params;

        public PdfSetTrimBoxByFormat(PdfSetTrimBoxByFormatParams param)
        {
            _params = param;
        }

        public void Run(string filePath)
        {
            string fileExt = Path.GetExtension(filePath);
            if (string.Equals(fileExt, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                SetTrimToPdf(filePath);
            }
            else
            {
                SetTrimToImage(filePath);
            }




        }

        private void SetTrimToImage(string filePath)
        {
            
            var convert = new PdfConvert(
                new PdfConvertParams
                {
                    TrimBox = {
                            width = _params.Width  * PdfScaler.mn,
                            height = _params.Height *  PdfScaler.mn
                    }
                });
            convert.Run(filePath);
        }

        void SetTrimToPdf(string filePath)
        {
            var tmpFile = Path.GetTempFileName();

            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.begin_document(tmpFile, "");

                var indoc = p.open_pdi_document(filePath, "");
                var endpage = (int)p.pcos_get_number(indoc, "length:pages");

                for (var pageno = 1; pageno <= endpage; pageno++)
                {
                    var page = p.open_pdi_page(indoc, pageno, "");

                    Box media = new Box();
                    media.GetMediabox(p, indoc, page);

                    double paramW = _params.Width * PdfScaler.mn;
                    double paramH = _params.Height * PdfScaler.mn;

                    double bleedX = (media.width - paramW) / 2;
                    double bleedY = (media.height - paramH) / 2;

                    double x = bleedX;
                    double y = bleedY;
                    double w = media.width - bleedX;
                    double h = media.height - bleedY;

                    if (page == -1) throw new Exception("Error: " + p.get_errmsg());

                    p.begin_page_ext(0, 0, "");
                    p.fit_pdi_page(page, 0, 0, "adjustpage");
                    p.end_page_ext($"trimbox {{{x} {y} {w} {h}}}");

                    p.close_pdi_page(page);
                }
                p.close_pdi_document(indoc);
                p.end_document("");

                RewriteFile(tmpFile, filePath);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfSetTrimBoxByBleed", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
                File.Delete(tmpFile);
            }
        }
    }


}
