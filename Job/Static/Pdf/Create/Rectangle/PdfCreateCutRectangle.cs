using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.Rectangle
{
    public sealed class PdfCreateCutRectangle
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
                var outfile = Path.Combine(dir, filename + "+cut.pdf");
                if (p.begin_document(outfile, "optimize=true") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int pagehdl = p.open_pdi_page(indoc, 1, "cloneboxes");
                if (pagehdl == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                var width = p.pcos_get_number(indoc, $"pages[{pagehdl}]/width");
                var height = p.pcos_get_number(indoc, $"pages[{pagehdl}]/height");

                Box trimbox = PdfHelper.GetTrimbox(p, indoc, 0);

                var layer_print = p.define_layer("print", "");
                var layer_cut = p.define_layer("cut", "");

                p.begin_page_ext(0, 0, "");
                p.begin_layer(layer_print);
                p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");

                p.begin_layer(layer_cut);
                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                p.set_gstate(gstate);

                p.setcolor("fillstroke", "cmyk", 0, 1, 1, 0);
                int spot = p.makespotcolor("cut");

                p.setlinewidth(1.0);

                p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);

                double x = trimbox.left;
                double y = trimbox.bottom;
                double w = trimbox.width;
                double h = trimbox.height;

                p.rect(x, y, w, h);
                p.stroke();

                p.close_pdi_page(pagehdl);
                p.end_layer();
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
