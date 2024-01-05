using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.SplitOddAndEven
{
    public class PdfSplitOddAndEven
    {
        public void Run(string filePath)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                var outfile_basename = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(filePath, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                string outFile = $"{outfile_basename}_odd.pdf";
                if (p.begin_document(outFile, "") == -1) throw new Exception("Error: " + p.get_errmsg());

                //odd
                for (int i = 0; i < page_count; i += 2)
                {
                    p.begin_page_ext(0, 0, "");
                    int pagehdl = p.open_pdi_page(indoc, i + 1, "cloneboxes");
                    if (pagehdl == -1) throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);
                    p.end_page_ext("");

                }
                p.end_document("");

                outFile = $"{outfile_basename}_even.pdf";
                if (p.begin_document(outFile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                //even
                for (int i = 1; i <= page_count; i += 2)
                {
                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(indoc, i + 1, "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                }
                p.end_document("");


                p.close_pdi_document(indoc);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfSplitOddAndEven", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
