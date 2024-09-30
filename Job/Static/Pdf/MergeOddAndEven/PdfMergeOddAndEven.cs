using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.MergeOddAndEven
{
    public sealed class PdfMergeOddAndEven
    {
        PdfMergeOddAndEvenParams _params;

        public PdfMergeOddAndEven(PdfMergeOddAndEvenParams param)
        {

            _params = param;
        }

        public void Run()
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();
                p.set_option("errorpolicy=return");

                int frontfile = p.open_pdi_document(_params.OddFile, "");
                if (frontfile == -1) throw new Exception("Error: " + p.get_errmsg());

                int backfile = p.open_pdi_document(_params.EvenFile, "");
                if (backfile == -1) throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(frontfile, "length:pages");

                string outfile = Path.Combine(Path.GetDirectoryName(_params.OddFile),
                    Path.GetFileNameWithoutExtension(_params.OddFile) + "_merged.pdf");

                if (p.begin_document(outfile, "") == -1) throw new Exception("Error: " + p.get_errmsg());

                for (int i = 0; i < page_count; i++)
                {
                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(frontfile, i + 1, "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);
                    p.end_page_ext("");

                    p.begin_page_ext(0, 0, "");

                    int pageback = p.open_pdi_page(backfile, i + 1, "cloneboxes");

                    p.fit_pdi_page(pageback, 0, 0, "cloneboxes");
                    p.close_pdi_page(pageback);

                    p.end_page_ext("");
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(frontfile);
                p.close_pdi_document(backfile);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfMergeOddAndEven", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }

    }
}
