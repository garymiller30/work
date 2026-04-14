using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.IO;

namespace JobSpace.Static.Pdf.Split
{
    [PdfTool("Розділити", "на обкладинку та блок",Icon = "split_cover_and_block",Order = 30)]
    public sealed class PdfSplitCoverAndBlock : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
                SplitCoverAndBlock(file.FullName);
        }

        public void SplitCoverAndBlock(string filePath)
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
                if (p.begin_document(outFile, "optimize=true") == -1) throw new Exception("Error: " + p.get_errmsg());

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
