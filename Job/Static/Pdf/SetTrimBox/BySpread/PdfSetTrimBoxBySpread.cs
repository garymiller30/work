using Job.Static.Pdf.Common;
using Job.Static.Pdf.Scale;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.SetTrimBox.BySpread
{
    public sealed class PdfSetTrimBoxBySpread : SetTrimBoxBase
    {
        PdfSetTrimBoxBySpreadParams _params;
        public PdfSetTrimBoxBySpread(PdfSetTrimBoxBySpreadParams param)
        {
            _params = param;
        }

        public void Run(string filePath)
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
                    if (page == -1) throw new Exception("Error: " + p.get_errmsg());

                    Box media = new Box();
                    
                    media.GetMediabox(p, indoc, page);

                    double x,y,w,h;

                    if (pageno % 2  == 0) // парна
                    {
                        x = _params.Outside * PdfScaler.mn;
                        w = media.width - _params.Inside * PdfScaler.mn;
                    }
                    else //непарна
                    {
                        x = _params.Inside * PdfScaler.mn;
                        w = media.width - _params.Outside * PdfScaler.mn;
                    }

                    y = _params.Bottom * PdfScaler.mn;
                    h = media.height - _params.Top * PdfScaler.mn;

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
