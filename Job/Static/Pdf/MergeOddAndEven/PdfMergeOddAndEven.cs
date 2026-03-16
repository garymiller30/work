using Interfaces.FileBrowser;
using Interfaces.Plugins;
using PDFlib_dotnet;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.MergeOddAndEven
{
    [PdfTool("З'єднати", "парні і непарні сторінки",Description ="З'єднати парні і непарні сторінки в один документ. Має бути вибрано два файли і один з них мати в імені 'odd' чи 'even'",Icon = "merge_odd_even")]
    public sealed class PdfMergeOddAndEven : IPdfTool
    {
        PdfMergeOddAndEvenParams _params;

        public bool Configure(PdfJobContext context)
        {
            if (context.InputFiles.Count != 2)
            {
                MessageBox.Show(
                    "Файлів має бути два! В одному непарні сторінки, а в іншому - парні",
                    "Альо!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            var files = context.InputFiles.Select(f => f.FileInfo).ToArray();

            var even = files.FirstOrDefault(f =>
                f.Name.ToLowerInvariant().Contains("even"));

            var odd = files.FirstOrDefault(f =>
                f.Name.ToLowerInvariant().Contains("odd"));

            if (even == null)
            {
                even = odd == files[0] ? files[1] : files[0];
            }

            if (odd == null)
            {
                odd = even == files[0] ? files[1] : files[0];
            }

            _params = new PdfMergeOddAndEvenParams
            {
                EvenFile = even.FullName,
                OddFile = odd.FullName
            };

            return true;
        }

        public void Execute(PdfJobContext context)
        {
            MergeOddAndEven();
        }

        public void MergeOddAndEven()
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();
                p.set_option("errorpolicy=return");

                int frontfile = p.open_pdi_document(_params.OddFile, "");
                if (frontfile == -1) throw new Exception("Error: " + p.get_errmsg());

                int backfile = p.open_pdi_document(_params.EvenFile, "");
                if (backfile == -1) throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(frontfile, "length:pages");

                string outfile = Path.Combine(Path.GetDirectoryName(_params.OddFile),
                    Path.GetFileNameWithoutExtension(_params.OddFile) + "_merged.pdf");

                if (p.begin_document(outfile, "optimize=true") == -1) throw new Exception("Error: " + p.get_errmsg());

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

                    int pageback = p.open_pdi_page(backfile, i + 1, "cloneboxes");

                    p.fit_pdi_page(pageback, 0, 0, "cloneboxes");
                    p.close_pdi_page(pageback);

                    p.end_page_ext("");
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(frontfile);
                p.close_pdi_document(backfile);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfMergeOddAndEven", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }

    }
}
