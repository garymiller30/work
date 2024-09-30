using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.EmptyPdfTemplateWithCount
{
    public sealed class PdfCreateEmptyPdfTemplateWithCount
    {
        public void Run(string pathTo,EmptyTemplate template)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();
                var filename = $"{template.Width}x{template.Height}";

                for (int i = 1; i <= template.Multiplier; i++)
                {
                    int fileCount = i;

                    string outfile;
                    do
                    {
                        outfile = Path.Combine(pathTo, $"{filename}_{fileCount}#{template.Count}.pdf");
                        fileCount++;
                    } while (File.Exists(outfile));

                    if (p.begin_document(outfile, "") == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    Box trimbox = new Box();
                    trimbox.CreateCustomBox(template.Width, template.Height, 3);

                    var (width, height) = trimbox.GetMediaBox();

                    p.begin_page_ext(width, height, "");

                    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");

                    p.set_gstate(gstate);
                    p.setcolor("fillstroke", "cmyk", 0.79, 0, 0.44, 0.21);

                    int spot = p.makespotcolor("ProofColor");

                    p.setlinewidth(1.0);

                    /* Red rectangle */
                    p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
                    p.rect(trimbox.left, trimbox.bottom, trimbox.width, trimbox.height);
                    p.stroke();

                    p.end_page_ext($"trimbox {{{trimbox.left} {trimbox.bottom} {trimbox.left + trimbox.width} {trimbox.height + trimbox.bottom}}}");
                    p.end_document("");
                }

            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "PdfCreateEmptyPdfTemplateWithCount");
            }
            finally
            {
                p?.Dispose();
            }
        }

    }
}
