using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити","Еліпс (Cut)",Icon = "create_ellipse_cut",Order = 10)]
    public sealed class PdfCreateCutEllipse : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                CreateCutEllipse(file.FullName);
            }
        }

        public void CreateCutEllipse(string filePath)
        {
            using (PDFlib p = new PDFlib())
            {
                try
                {
                    int indoc = p.open_pdi_document(filePath, "");

                    if (indoc == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                    var dir = Path.GetDirectoryName(filePath);
                    var filename = Path.GetFileNameWithoutExtension(filePath);
                    var outfile = Path.Combine(dir, filename + "+cut.pdf");

                    p.begin_document(outfile, "optimize=true");

                    var layer_print = p.define_layer("print", "");
                    var layer_cut = p.define_layer("cut", "");

                    for (int i = 1; i <= page_count; i++)
                    {
                        int pagehdl = p.open_pdi_page(indoc, i, "cloneboxes");

                        var width = p.pcos_get_number(indoc, $"pages[{pagehdl}]/width");
                        var height = p.pcos_get_number(indoc, $"pages[{pagehdl}]/height");

                        Box trimbox = PdfHelper.GetTrimbox(p, indoc, 0);

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
                        p.ellipse(width / 2, height / 2, trimbox.width / 2, trimbox.height / 2);
                        p.stroke();

                        p.close_pdi_page(pagehdl);
                        p.end_layer();
                        p.end_page_ext($"trimbox {{{trimbox.left} {trimbox.bottom} {trimbox.left + trimbox.width} {trimbox.height + trimbox.bottom}}}");
                    }
                    p.end_document("");
                    p.close_pdi_document(indoc);
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "PdfCreateEllipse");
                }
            }
        }
    }
}
