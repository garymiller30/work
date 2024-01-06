using iTextSharp.text.pdf;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.RepeatPages
{
    public sealed class PdfRepeatPages
    {
        PdfRepeatPagesParams _params;
        public PdfRepeatPages(PdfRepeatPagesParams param)
        {

            _params = param;

        }

        public void Run(string filePath)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(filePath, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");
                int outdoc_count = page_count * _params.Count;

                String outfile = filePath + ".tmp";
                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int i = 0; i < page_count; i++)
                {
                    int pagehdl = p.open_pdi_page(indoc, i + 1, "cloneboxes");
                    for (int j = 0; j < _params.Count; j++)
                    {
                        p.begin_page_ext(0, 0, "");
                        if (pagehdl == -1) throw new Exception("Error: " + p.get_errmsg());
                        p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                        p.end_page_ext("");
                    }
                    p.close_pdi_page(pagehdl);
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(indoc);

                File.Delete(filePath);
                File.Move(outfile, filePath);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfRepeatPages", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
