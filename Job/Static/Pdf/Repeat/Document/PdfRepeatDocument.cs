using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Dlg;
using PDFlib_dotnet;
using System;
using System.IO;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Repeat.Document
{
    [PdfTool("","Повторити документ (123-123-123)",Icon = "duplicate_document")]
    public class PdfRepeatDocument : IPdfTool
    {
        PdfRepeatDocumentParams _params;

        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormInputCountPages())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _params = new PdfRepeatDocumentParams { Count = form.CountPages };
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                Run(file.FullName);
            }
        }

        public void Run(string filePath)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                int indoc = p.open_pdi_document(filePath, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                string outfile = Path.Combine(Path.GetDirectoryName(filePath),$"{Path.GetFileNameWithoutExtension(filePath)}_doc_dup.pdf");
                if (p.begin_document(outfile, "optimize=true") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int i = 0; i < _params.Count; i++)
                {
                    for (int j = 0; j < page_count; j++)
                    {
                        p.begin_page_ext(0, 0, "");

                        int pagehdl = p.open_pdi_page(indoc, j + 1, "cloneboxes");
                        if (pagehdl == -1)
                            throw new Exception("Error: " + p.get_errmsg());
                        p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                        p.close_pdi_page(pagehdl);

                        p.end_page_ext("");
                    }
                }
                p.end_document("");

                p.close_pdi_document(indoc);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfRepeatDocument", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
