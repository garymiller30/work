using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.MergeFrontsAndBack
{
    public sealed class PdfMergeFrontsAndBack
    {
        PdfMergeFrontsAndBackParams _params;
        public PdfMergeFrontsAndBack(PdfMergeFrontsAndBackParams param)
        {
            _params = param;
        }

        public void Run()
        {
            foreach (string file in _params.FrontsFiles)
            {
                ProcessSingleFile(file);
            }
        }

        void ProcessSingleFile(string frontFilePath)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                int frontfile = p.open_pdi_document(frontFilePath, "");

                if (frontfile == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int backfile = p.open_pdi_document(_params.BackFile, "");
                if (backfile == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(frontfile, "length:pages");

                string outfile = Path.Combine(Path.GetDirectoryName(frontFilePath),
                    Path.GetFileNameWithoutExtension(frontFilePath) + "_merged.pdf");

                if (p.begin_document(outfile, "optimize=true") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

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

                    int pageback = p.open_pdi_page(backfile, 1, "cloneboxes");

                    p.fit_pdi_page(pageback, 0, 0, "cloneboxes");
                    p.close_pdi_page(pageback);

                    p.end_page_ext("");
                }

                p.end_document("");
               
                p.close_pdi_document(frontfile);
                p.close_pdi_document(backfile);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfMergeFrontsAndBack", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }

    }
}
