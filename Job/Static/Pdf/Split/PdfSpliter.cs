using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.UserForms;
using PDFlib_dotnet;
using System.IO;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Split
{
    [PdfTool("Розділити", "розвороти", Description = "Розділити розвороти сторінок", Icon = "split_razvorot", Order = 30)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public sealed class PdfSpliter : IPdfTool
    {
        PdfSplitterParams _param;


        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormPdfSplitterParams())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _param = form.Params;
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
            string targetFile = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}_splitted.pdf");

            using (PDFlib p = new PDFlib())
            {
                try
                {
                    p.begin_document(targetFile, "optimize=true");
                    {
                        var indoc = p.open_pdi_document(filePath, "");
                        var endpage = (int)p.pcos_get_number(indoc, "length:pages");
                        int to = _param.To > 0 ? _param.To : endpage;
                        if (to > endpage)
                        {
                            to = endpage;
                        }

                        if (_param.SaddleStitchOrder)
                        {
                            SplitSaddleStitchOrder(p, indoc, endpage, to);
                        }
                        else
                        {
                            SplitSourceOrder(p, indoc, endpage, to);
                        }
                        p.close_pdi_document(indoc);
                    }
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    Logger.Log.Error(null, "PdfSpliter", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
                }

            }


        }

        private void SplitSourceOrder(PDFlib p, int indoc, int endpage, int to)
        {
            for (var pageno = 1; pageno <= endpage; pageno++)
            {
                if (pageno >= _param.From && pageno <= to)
                {
                    AddSplitPage(p, indoc, pageno, true);
                    AddSplitPage(p, indoc, pageno, false);
                }
                else
                {
                    ClonePage(p, indoc, pageno);
                }
            }
        }

        private void SplitSaddleStitchOrder(PDFlib p, int indoc, int endpage, int to)
        {
            if (_param.From > to)
            {
                for (var pageno = 1; pageno <= endpage; pageno++)
                {
                    ClonePage(p, indoc, pageno);
                }

                return;
            }

            for (var pageno = 1; pageno < _param.From; pageno++)
            {
                ClonePage(p, indoc, pageno);
            }

            int spreadCount = to - _param.From + 1;
            int pagesCount = spreadCount * 2;

            for (var outputPage = 1; outputPage <= pagesCount; outputPage++)
            {
                int spreadNumber = outputPage <= spreadCount
                    ? outputPage
                    : pagesCount - outputPage + 1;

                bool leftPage = outputPage % 2 == 0;
                int sourcePage = _param.From + spreadNumber - 1;

                AddSplitPage(p, indoc, sourcePage, leftPage);
            }

            for (var pageno = to + 1; pageno <= endpage; pageno++)
            {
                ClonePage(p, indoc, pageno);
            }
        }

        private void AddSplitPage(PDFlib p, int indoc, int pageno, bool leftPage)
        {
            int page = p.open_pdi_page(indoc, pageno, "");
            try
            {
                Box box = PdfHelper.GetTrimbox(p, indoc, pageno - 1);

                double bleed = _param.Bleed * PdfHelper.mn;

                double pageW = box.width / 2 + bleed;
                double pageH = box.height + bleed * 2;

                double trim_left = leftPage ? bleed : 0;
                double trim_bottom = bleed;
                double trim_right = leftPage ? pageW : box.width / 2;
                double trim_top = trim_bottom + box.height;

                p.begin_page_ext(
                    pageW,
                    pageH,
                    $"trimbox={{{trim_left} {trim_bottom} {trim_right} {trim_top}}}");

                double pageX = bleed - box.left;
                double pageY = bleed - box.bottom;

                if (!leftPage)
                {
                    pageX -= pageW;
                }

                p.fit_pdi_page(page, pageX, pageY, "");
                p.end_page_ext("");
            }
            finally
            {
                p.close_pdi_page(page);
            }
        }

        private void ClonePage(PDFlib p, int indoc, int pageno)
        {
            int page = p.open_pdi_page(indoc, pageno, "cloneboxes");
            try
            {
                p.begin_page_ext(0, 0, "");
                p.fit_pdi_page(page, 0, 0, "cloneboxes");
                p.end_page_ext("");
            }
            finally
            {
                p.close_pdi_page(page);
            }
        }
    }
}
