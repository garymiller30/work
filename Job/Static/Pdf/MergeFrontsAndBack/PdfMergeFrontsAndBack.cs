using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Dlg;
using PDFlib_dotnet;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.MergeFrontsAndBack
{
    [PdfTool("З'єднати", "Лице + Зворот",Description ="З'єднати файли де лице - різні сторінки, а зворот - один для всіх",Icon = "merge_front_and_back")]
    public sealed class PdfMergeFrontsAndBack : IPdfTool
    {
        PdfMergeFrontsAndBackParams _params;

        public bool Configure(PdfJobContext context)
        {

            if (context.InputFiles == null || context.InputFiles.Count <2)
            {
                return false;
            }

            using (var form = new FormSelectBackFile(context.InputFiles))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _params = new PdfMergeFrontsAndBackParams
                    {
                        BackFile = form.Back,
                    };
                    _params.FrontsFiles = context.InputFiles.Select(x=>x.FullName).Except(new[] { _params.BackFile, }, StringComparer.OrdinalIgnoreCase).ToArray();
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            MergeFrontsAndBack();
        }

        public void MergeFrontsAndBack()
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
