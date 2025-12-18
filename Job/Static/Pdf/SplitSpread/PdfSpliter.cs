using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System.IO;

namespace JobSpace.Static.Pdf.SplitSpread
{
    public sealed class PdfSpliter
    {
        PdfSplitterParams _param;

        public PdfSpliter(PdfSplitterParams param)
        {
            _param = param;
        }

        public void Run(string filePath)
        {
            PDFlib p = null;
            try
            {
                string targetFile = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}_splitted.pdf");
                p = new PDFlib();
                p.begin_document(targetFile, "optimize=true");
                {
                    var indoc = p.open_pdi_document(filePath, "");
                    var endpage = (int)p.pcos_get_number(indoc, "length:pages");

                    for (var pageno = 1; pageno <= endpage; pageno++)
                    {
                        int page;
                        int to = _param.To > 0 ? _param.To : endpage;

                        if (pageno >= _param.From && pageno <= to)
                        {
                            page = p.open_pdi_page(indoc, pageno, "");
                            {
                                Box box = PdfHelper.GetTrimbox(p, indoc, pageno - 1);

                                double bleed = _param.Bleed * PdfHelper.mn;

                                double pageW = box.width / 2 + bleed;
                                double pageH = box.height + bleed * 2;

                                double trim_left = bleed;
                                double trim_bottom = bleed;
                                double trim_right = pageW;
                                double trim_top = trim_bottom + box.height;

                                // left page
                                p.begin_page_ext(
                                pageW,
                                pageH,
                                $"trimbox={{{trim_left} {trim_bottom} {trim_right} {trim_top}}}");

                                double pageX = bleed - box.left;
                                double pageY = bleed - box.bottom;
                           
                                p.fit_pdi_page(page, pageX, pageY, "");
                                p.end_page_ext("");

                                // right page
                                trim_right = box.width / 2;

                                p.begin_page_ext(
                                pageW,
                                pageH,
                                $"trimbox={{{0} {trim_bottom} {trim_right} {trim_top}}}");

                                pageX -= pageW;

                                p.fit_pdi_page(page, pageX, pageY, "");
                                p.end_page_ext("");
                            }
                        }
                        else
                        {
                            page = p.open_pdi_page(indoc, pageno, "cloneboxes");
                            p.begin_page_ext(0, 0, "");
                            p.fit_pdi_page(page, 0, 0, "cloneboxes");
                            p.end_page_ext("");
                        }
                        p.close_pdi_page(page);
                    }
                    p.close_pdi_document(indoc);
                }
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfSpliter", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }

        }
    }
}
