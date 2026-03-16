using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.UserForms.PDF;
using PDFlib_dotnet;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Merge
{
    [PdfTool("З'єднати", "вибрані файли в один",Description ="З'єднати вибрані файли в один PDF",Icon ="merge")]
    public class PdfMerger : IPdfTool
    {
        List<string> files = new List<string>();

        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormList(context.InputFiles))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    files = form.ConvertFiles.Select(x=>x.FullName).ToList();
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            string fileName = Path.Combine(Path.GetDirectoryName(files[0]), $"{Path.GetFileNameWithoutExtension(files[0])}_merged.pdf");

            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(fileName, "optimize=true");

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
