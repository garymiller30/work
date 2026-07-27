using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Add
{
    [PdfTool("Додати", "додати прямокутник (Cut)", Icon = "create_rectangle_cut", Order = 2,Description ="Додати прямокутник (cut) по обрізному формату до файлу")]
    public sealed class PdfAddCutRectangle : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            Parallel.ForEach(context.InputFiles, file =>
            {
                try
                {
                    ProcessFile(file.FullName);
                }
                catch (PDFlibException ex) {
                    PdfHelper.LogException(ex, $"Помилка при обробці файлу: {file.FullName}");
                }
            });
           
        }

        private void ProcessFile(string fullName)
        {
            using PDFlib p = new();
            int indoc = -1;

            try
            {
                indoc = p.open_pdi_document(fullName, "");
                if (indoc == -1)
                    throw new Exception($"Помилка відкриття PDF: {p.get_errmsg()}");

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                var outfile = GenerateOutputFile(fullName);
                p.begin_document(outfile, "optimize=true");

                var layer_print = p.define_layer(Common.Constants.PRINT_STRING, "");
                var layer_cut = p.define_layer(Common.Constants.CUT_STRING, "");
                
                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                for (int i = 1; i <= page_count; i++)
                {
                    ProcessPage(p, indoc, i, layer_print, layer_cut, gstate);
                }
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "PdfAddCutRectangle");
            }
            finally
            {
                if (indoc != -1) p.close_pdi_document(indoc);
            }

        }

        private void ProcessPage(PDFlib p, int indoc, int i, int layer_print, int layer_cut,int gstate)
        {
            int pagehdl = p.open_pdi_page(indoc, i, "cloneboxes");
            if (pagehdl == -1) return;

            try
            {
                var width = p.pcos_get_number(indoc, $"pages[{i-1}]/width");
                var height = p.pcos_get_number(indoc, $"pages[{i-1}]/height");

                Box trimbox = PdfHelper.GetTrimbox(p, indoc, i-1);

                p.begin_page_ext(width, height, "");
                p.begin_layer(layer_print);
                p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");

                p.begin_layer(layer_cut);
                DrawRectangle(p, width, height, trimbox,gstate);
                p.end_page_ext($"trimbox {{{trimbox.left} {trimbox.bottom} {trimbox.left + trimbox.width} {trimbox.height + trimbox.bottom}}}");
            }
            finally
            {
                // ГАРАНТОВАНО закриваємо handle сторінки
                if (pagehdl != -1) p.close_pdi_page(pagehdl);
            }
        }

        private void DrawRectangle(PDFlib p, double width, double height, Box trimbox, int gstate)
        {
            //int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
            p.set_gstate(gstate);

            p.setcolor("fillstroke", "cmyk", 0, 1, 1, 0);
            int spot = p.makespotcolor(Constants.CUT_STRING);

            p.setlinewidth(1.0);

            p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);

            double x = trimbox.left;
            double y = trimbox.bottom;
            double w = trimbox.width;
            double h = trimbox.height;

            p.rect(x, y, w, h);
            p.stroke();
            
        }

        private string GenerateOutputFile(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath) ?? string.Empty;
            string name = Path.GetFileNameWithoutExtension(filePath);
            return Path.Combine(dir, $"{name}+cut.pdf");
        }
    }
}
