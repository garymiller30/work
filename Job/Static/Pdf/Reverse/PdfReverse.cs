using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.IO;

namespace JobSpace.Static.Pdf.Reverse
{
    [PdfTool("","Сторінки в зворотньому напрямку",Icon = "reverse_pages")]
    public sealed class PdfReverse : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                Reverse(file.FullName);
            }
        }

        public void Reverse(string filePath)
        {
            using ( PDFlib p = new PDFlib())
            {
                try
                {
                    var outfile = Path.Combine(Path.GetDirectoryName(filePath),
                        Path.GetFileNameWithoutExtension(filePath) + "_reverse.pdf");

                    int indoc = p.open_pdi_document(filePath, "");

                    if (indoc == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                    if (p.begin_document(outfile, "optimize=true") == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    /* Loop over all subsequent pages in reverse order */
                    for (var pageno = page_count; pageno > 0; pageno--)
                    {
                        p.begin_page_ext(0, 0, "");

                        int pagehdl = p.open_pdi_page(indoc, pageno, "cloneboxes");
                        p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                        p.close_pdi_page(pagehdl);

                        p.end_page_ext("");
                    }

                    p.end_document("");
                    p.close_pdi_document(indoc);
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "PdfReverse");
                }
            }
        }
    }
}
