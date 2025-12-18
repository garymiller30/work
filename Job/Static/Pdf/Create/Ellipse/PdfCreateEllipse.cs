using JobSpace.Static.Pdf.Common;
using MongoDB.Driver;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.Ellipse
{
    public sealed class PdfCreateEllipse
    {
        public void Run(string filePath)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                int indoc = p.open_pdi_document(filePath, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                var dir = Path.GetDirectoryName(filePath);
                var filename = Path.GetFileNameWithoutExtension(filePath);
                var outfile = Path.Combine(dir, filename + "_ellipse.pdf");
                if (p.begin_document(outfile, "optimize=true") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

               
                int pagehdl = p.open_pdi_page(indoc, 1, "");
                if (pagehdl == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                var width = p.pcos_get_number(indoc, $"pages[{pagehdl}]/width");
                var height = p.pcos_get_number(indoc, $"pages[{pagehdl}]/height");

                Box trimbox = PdfHelper.GetTrimbox(p,indoc, 0);

                p.begin_page_ext(width, height, "");

                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                p.set_gstate(gstate);

                p.setcolor("fillstroke", "cmyk", 0.79, 0, 0.44, 0.21);
                int spot = p.makespotcolor("ProofColor");

                p.setlinewidth(1.0);

                p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
                p.ellipse(width / 2, height / 2, trimbox.width / 2, trimbox.height / 2);
                p.stroke();

                p.close_pdi_page(pagehdl);
                p.end_page_ext($"trimbox {{{trimbox.left} {trimbox.bottom} {trimbox.left + trimbox.width} {trimbox.height + trimbox.bottom}}}");
                p.end_document("");
                p.close_pdi_document(indoc);
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfCreateEllipse", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
