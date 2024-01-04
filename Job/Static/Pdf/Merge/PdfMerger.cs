using PDFlib_dotnet;
using PDFManipulate.Shema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Job.Static.Pdf.Merge
{
    public class PdfMerger
    {
        List<string> files = new List<string>();

        public PdfMerger(IEnumerable<string> filesPath)
        {
            files.AddRange(filesPath);
        }

        public void Run()
        {
            string fileName = Path.Combine(Path.GetDirectoryName(files[0]), $"{Path.GetFileNameWithoutExtension(files[0])}_merged.pdf");

            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(fileName, "");

                foreach (string file in files)
                {
                    var indoc = p.open_pdi_document(file, "");
                    var pagecount = p.pcos_get_number(indoc, "length:pages");

                    for (int i = 1; i <= pagecount; i++)
                    {
                        var page = p.open_pdi_page(indoc, i, "cloneboxes");
                        p.begin_page_ext(0, 0, "");
                        p.fit_pdi_page(page, 0, 0, "cloneboxes");
                        p.close_pdi_page(page);
                        p.end_page_ext("");
                    }
                    p.close_pdi_document(indoc);
                }

                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "ScalePdf", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally { p?.Dispose(); }
        }
    }
}
