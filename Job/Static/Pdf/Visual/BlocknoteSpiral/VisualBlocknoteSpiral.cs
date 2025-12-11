using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.BlocknoteSpiral
{
    public sealed class VisualBlocknoteSpiral
    {
        SpiralSettings _spiralSettings;

        public VisualBlocknoteSpiral(SpiralSettings spiralSettings)
        {
            _spiralSettings = spiralSettings;
        }

        public void Run(string file)
        {
            string targetfile = Path.Combine(
                Path.GetDirectoryName(file),
                Path.GetFileNameWithoutExtension(file) + "_spiral" + Path.GetExtension(file)
                );

            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(targetfile, "optimize=true");
                int doc = p.open_pdi_document(file, "");
                double pagecount = p.pcos_get_number(doc, "length:pages");

                for (int i = 1; i <= pagecount; i++)
                {
                    var page_handle = p.open_pdi_page(doc, i, "");

                    var boxes = PdfHelper.GetBoxes(p,doc, page_handle);
                   
                    // Початок сторінки з оригінальними розмірами
                    
                    p.begin_page_ext(boxes.Media.width, boxes.Media.height, "");
                    
                    int l_print = p.define_layer("print", "");
                    int v_layer = p.define_layer("visual","");
                    
                    p.begin_layer(l_print);
                    // Відображення вмісту сторінки
                    p.fit_pdi_page(page_handle, 0, 0, "");
                    p.end_layer();

                    p.begin_layer(v_layer);
                    // Додавання спіралі
                    SpiralDrawer.DrawSpiral(p, boxes,i, _spiralSettings);
                    p.end_layer();

                    p.close_pdi_page(page_handle);
                    //p.end_page_ext("");
                    p.end_page_ext($"trimbox {{{boxes.Trim.left} {boxes.Trim.bottom} {boxes.Trim.width + boxes.Trim.left} {boxes.Trim.bottom + boxes.Trim.height}}}");
                }
                p.close_pdi_document(doc);
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "VisualBlocknoteSpiral");
            }
            finally
            {
                p?.Dispose();
            }

        }
    }
}
