using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Divide
{
    public class PdfDivider
    {
        PdfDividerParams _param;

        public PdfDivider(PdfDividerParams param)
        {
            _param = param;
        }

        public void Run(string filePath)
        {
            PDFlib p = null;


            try
            {
                p = new PDFlib();

                var outfile_basename = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
                int indoc = p.open_pdi_document(filePath, "");

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                if (_param.Mode == Common.DivideModeEnum.FixedCountPages)
                {
                    int outdoc_count = page_count / _param.FixedCountPages
                                   + (page_count % _param.FixedCountPages > 0 ? 1 : 0);

                    for (int outdoc_counter = 0, page = 0;
                    outdoc_counter < outdoc_count; outdoc_counter += 1)
                    {
                        String outfile = outfile_basename + "_" + (outdoc_counter + 1)
                                     + ".pdf";

                        p.begin_document(outfile, "optimize=true");

                        for (int i = 0; page < page_count && i < _param.FixedCountPages;
                        page += 1, i += 1)
                        {
                            p.begin_page_ext(0, 0, "");

                            int pagehdl = p.open_pdi_page(indoc, page + 1, "cloneboxes");

                            p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                            p.close_pdi_page(pagehdl);

                            p.end_page_ext("");
                        }
                        /* Close the current sub-document */
                        p.end_document("");
                    }
                }
                else
                {
                    if (page_count < _param.CustomCountPages.Sum()) throw new Exception("Error: кількість вказаних сторінок більша за кількість сторінок в документі");

                    int lastPage = _param.CustomCountPages.Sum();
                    int nextPage = 0;

                    var pagesCount = _param.CustomCountPages.ToList();

                    if (page_count > lastPage)
                    {
                        pagesCount.Add(page_count - lastPage);
                    }


                    for (int i = 0; i < pagesCount.Count; i++)
                    {
                        string outFile = $"{outfile_basename}_{i + 1}.pdf";

                        p.begin_document(outFile, "");

                        for (int j = 0; j < pagesCount[i]; j++)
                        {
                            p.begin_page_ext(0, 0, "");

                            int pagehdl = p.open_pdi_page(indoc, nextPage + 1, "cloneboxes");

                            nextPage++;

                            p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                            p.close_pdi_page(pagehdl);

                            p.end_page_ext("");
                        }
                        p.end_document("");
                    }
                }
                p.close_pdi_document(indoc);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "ScalePdf", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }



    }
}
