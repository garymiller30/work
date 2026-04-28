using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Visual.BlocknoteSpiral
{
    [PdfTool("Візуалізація","Пружина",Icon = "visual_spiral",Order = 20)]
    public sealed class VisualBlocknoteSpiral : IPdfTool
    {
        SpiralSettings _spiralSettings;

        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file == null)
            {
                return false;
            }


            using (var form = new FormVisualBlocknoteSpiral(context.InputFiles))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _spiralSettings = form.SpiralSettings;
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                CreateBlocknoteSpiral(file.FullName);
            }
        }

        public void CreateBlocknoteSpiral(string file)
        {
            string targetfile = Path.Combine(
                Path.GetDirectoryName(file),
                Path.GetFileNameWithoutExtension(file) + "_spiral" + Path.GetExtension(file)
                );

            using (PDFlib p = new PDFlib())
            {
                try
                {
                    p.begin_document(targetfile, "optimize=true");
                    int doc = p.open_pdi_document(file, "");
                    double pagecount = p.pcos_get_number(doc, "length:pages");

                    int l_print = p.define_layer("print", "");
                    int v_layer = p.define_layer("ProofColor", "");

                    for (int i = 1; i <= pagecount; i++)
                    {
                        var page_handle = p.open_pdi_page(doc, i, "");

                        var boxes = PdfHelper.GetBoxes(p, doc, i - 1);

                        // Початок сторінки з оригінальними розмірами

                        p.begin_page_ext(boxes.Media.width, boxes.Media.height, "");

                        p.begin_layer(l_print);
                        // Відображення вмісту сторінки
                        p.fit_pdi_page(page_handle, 0, 0, "");


                        p.begin_layer(v_layer);
                        // Додавання спіралі
                        SpiralDrawer.DrawSpiral(p, boxes, i, _spiralSettings);

                        p.close_pdi_page(page_handle);
                        //p.end_page_ext("");
                        p.end_layer();
                        p.end_page_ext($"trimbox {{{boxes.Trim.left} {boxes.Trim.bottom} {boxes.Trim.width + boxes.Trim.left} {boxes.Trim.bottom + boxes.Trim.height}}}");
                    }
                    p.close_pdi_document(doc);
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "VisualBlocknoteSpiral");
                }
            }
        }
    }
}
