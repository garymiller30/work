using Job.Static.Pdf.Common;
using PDFlib_dotnet;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

                    string fileExt = Path.GetExtension(file);

                    if (fileExt.Equals(".pdf", System.StringComparison.InvariantCultureIgnoreCase))
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
                    else
                    {
                        p.begin_page_ext(0, 0, "");
                        int image = p.load_image("auto", file, "honoriccprofile=false ignoremask=true");
                        p.fit_image(image, 0, 0, "adjustpage");
                        p.close_image(image);
                        p.end_page_ext("");
                    }
                }

                p.end_document("");
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "PdfMerger");
            }
            finally { p?.Dispose(); }
        }
    }
}
