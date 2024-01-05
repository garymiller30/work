using Job.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.SplitCoverAndBlock
{
    public sealed class PdfSplitCoverAndBlock
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

                if (page_count < 5) throw new Exception("Error: count pages less than 5");

                int[] cover = new int[4] { 1, 2, page_count - 1, page_count };

                string outFile = $"{outfile_basename}_cover.pdf";
                if (p.begin_document(outFile, "") == -1) throw new Exception("Error: " + p.get_errmsg());

                foreach (int i in cover)
                {
                    p.begin_page_ext(0, 0, "");
                    int pagehdl = p.open_pdi_page(indoc, i, "cloneboxes");
                    if (pagehdl == -1) throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);
                    p.end_page_ext("");
                }
                p.end_document("");

                outFile = $"{outfile_basename}_block.pdf";
                if (p.begin_document(outFile, "") == -1) throw new Exception("Error: " + p.get_errmsg());

                for (int i = 3; i <= page_count - 2; i++)
                {
                    p.begin_page_ext(0, 0, "");
                    int pagehdl = p.open_pdi_page(indoc, i, "cloneboxes");
                    if (pagehdl == -1) throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);
                    p.end_page_ext("");
                }
                p.end_document("");
                p.close_pdi_document(indoc);
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "PdfSplitCoverAndBlock");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
