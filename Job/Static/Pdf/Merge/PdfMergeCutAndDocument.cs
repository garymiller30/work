using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Merge
{
    [PdfTool("З'єднати", "контур ножа і документ", Description = "файл з назвою `cut` накладається на кожну сторінку документа", Icon = "merge_cut_to_doc")]
    public class PdfMergeCutAndDocument : IPdfTool
    {
        IFileSystemInfoExt cutFile;
        List<IFileSystemInfoExt> doc;

        public bool Configure(PdfJobContext context)
        {
            // перевірити, чи є в списку файл, що містить cut в назві
            cutFile = context.InputFiles.FirstOrDefault(x => x.Name.IndexOf("cut", StringComparison.InvariantCultureIgnoreCase) != -1);
            if (cutFile == null) { return false; }
            doc = context.InputFiles.Where(x => x != cutFile).ToList();
            if (doc.Count() == 0) return false;
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in doc)
            {
                AddCut(file);
            }
        }

        private void AddCut(IFileSystemInfoExt file)
        {
            string target = Path.Combine(Path.GetDirectoryName(file.FullName),$"{Path.GetFileNameWithoutExtension(file.FullName)}+cut.pdf");

            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(target, "optimize=true");
                int doc = p.open_pdi_document(file.FullName, "");
                double pagecount = p.pcos_get_number(doc, "length:pages");

                int l_print = p.define_layer("print", "");
                int v_layer = p.define_layer("cut", "");

                int doc_cut = p.open_pdi_document(cutFile.FullName,"");
                int p_cut = p.open_pdi_page(doc_cut,1,"");

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
                    p.fit_pdi_page(p_cut,0,0,"");


                    p.close_pdi_page(page_handle);
                    p.end_layer();
                    p.end_page_ext($"trimbox {{{boxes.Trim.left} {boxes.Trim.bottom} {boxes.Trim.width + boxes.Trim.left} {boxes.Trim.bottom + boxes.Trim.height}}}");
                }
                p.close_pdi_document(doc);
                p.close_pdi_page(p_cut);
                p.close_pdi_document(doc_cut);
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
