using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.ExtractPages
{
    public sealed class PdfExtractPages
    {
        PdfExtractPagesParams _params;

        public PdfExtractPages(PdfExtractPagesParams param)
        {
            _params = param;
        }

        public void Run(string filePath)
        {
            PDFlib p = null;

#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                p = new PDFlib();

                var outfile_basename = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(filePath, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                //if (page_count < customCountPages.Sum()) throw new Exception("Error: кількість вказаних сторінок більша за кількість сторінок в документі");

                int maxPage = _params.Pages.Max();
                if (maxPage > page_count) throw new Exception("Error: Номер сторінки більший за кількість сторінок в документі");

                var pagesCount = _params.Pages.ToList();

                for (int i = 0; i < pagesCount.Count; i++)
                {
                    string outFile = $"{outfile_basename}_{pagesCount[i]}.pdf";

                    if (p.begin_document(outFile, "") == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(indoc, pagesCount[i], "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                    p.end_document("");
                }

                p.close_pdi_document(indoc);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
#pragma warning restore CS0168 // Variable is declared but never used
        }
    }
}
