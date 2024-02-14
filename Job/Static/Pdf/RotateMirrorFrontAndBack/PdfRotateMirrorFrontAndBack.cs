using Job.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.RotateMirrorFrontAndBack
{
    public class PdfRotateMirrorFrontAndBack
    {
        public void Run(string filePath)
        {
            PDFlib p = null;

            bool back = false;
            string[] angles = new string[2] { "west", "east" };

            try
            {
                p = new PDFlib();

                var outputFile = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}_90grad{Path.GetExtension(filePath)}");
                p.begin_document(outputFile, "");

                int indoc = p.open_pdi_document(filePath, "");
                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                var endpage = (int)p.pcos_get_number(indoc, "length:pages");


                for (var pageno = 1; pageno <= endpage; pageno++)
                {
                    var page = p.open_pdi_page(indoc, pageno, "");
                    var width = p.pcos_get_number(indoc, $"pages[{pageno - 1}]/width");
                    var height = p.pcos_get_number(indoc, $"pages[{pageno - 1}]/height");
                    
                    var trimbox = PdfHelper.GetTrimbox(p,indoc, pageno-1);

                    if (back)
                    {
                        trimbox.RotateCounerClockWise90deg(new Box() { width = width, height = height });
                    }
                    else
                    {
                        trimbox.RotateClockWise90deg();
                    }

                    p.begin_page_ext(height, width, "");
                    p.fit_pdi_page(page, 0, 0, $"adjustpage orientate={angles[back ? 1 : 0]}");
                    p.end_page_ext($"trimbox {{{trimbox.left} {trimbox.bottom} {trimbox.left + trimbox.width} {trimbox.height + trimbox.bottom}}}");
                    p.close_pdi_page(page);

                    back = !back;
                }

                p.end_document("");

            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "PdfRotateMirrorFrontAndBack");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
