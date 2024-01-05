using Job.Static.Pdf.Common;
using Job.Static.Pdf.Scale;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.Static.Pdf.SetTrimBox.ByBleed
{
    public sealed class PdfSetTrimBoxByBleed
    {
        PdfSetTrimBoxByBleedParams _params;

        public PdfSetTrimBoxByBleed(PdfSetTrimBoxByBleedParams param)
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

                    Box media = new Box();
                    media.GetMediabox(p,indoc,page);

                    double bleed = _params.Bleed * PdfScaler.mn;

                    double x = bleed;
                    double y = bleed;
                    double w = media.width - bleed;
                    double h = media.height - bleed;

                    if (page == -1) throw new Exception("Error: " + p.get_errmsg());

                    p.begin_page_ext(0,0,"");
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

        void RewriteFile(string source,string target)
        {
            while (true)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(source, target, overwrite: true);
                    break;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(e.Message, @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        break;
                    }
                }
            }
        }
    }
}
