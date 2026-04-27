using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using PDFManipulate.Forms;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.ExtractPages
{
    [PdfTool("","Витягти сторінки",Icon = "extract_page",Description = "Витягти сторінки з PDF документа")]
    public sealed class PdfExtractPages : IPdfTool
    {
        PdfExtractPagesParams _params;
      
        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormSelectCountPages())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {

                    _params = new PdfExtractPagesParams
                    {
                        Pages = form.Pages,
                    };
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                ExtractPages(file.FullName);
            }
        }

        public void ExtractPages(string filePath)
        {
            using ( PDFlib p = new PDFlib())
            {
                try
                {


                    var outfile_basename = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));

                    p.set_option("errorpolicy=return");

                    int indoc = p.open_pdi_document(filePath, "");

                    if (indoc == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                    int maxPage = _params.Pages.Max();
                    if (maxPage > page_count) throw new Exception("Error: Номер сторінки більший за кількість сторінок в документі");

                    var pagesCount = _params.Pages.ToList();

                    for (int i = 0; i < pagesCount.Count; i++)
                    {
                        string outFile = $"{outfile_basename}_{pagesCount[i]}.pdf";

                        p.begin_document(outFile, "optimize=true");
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
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "PdfExtractPages");
                }
            }
        }
    }
}
