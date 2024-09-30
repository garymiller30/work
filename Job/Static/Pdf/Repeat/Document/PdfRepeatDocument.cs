using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Repeat.Document
{
    public class PdfRepeatDocument
    {
        PdfRepeatDocumentParams _params;
        public PdfRepeatDocument(PdfRepeatDocumentParams param)
        {
            _params = param;
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

                string outfile = filePath + ".tmp";
                if (p.begin_document(outfile, "") == -1)
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

                File.Delete(filePath);
                File.Move(outfile, filePath);
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
