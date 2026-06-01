using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.Commons; // Припускаю, що PdfHelper знаходиться тут
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual
{
    [PdfTool("Візуалізація", "Виштовхування при кріпленні на скобу", Icon = "visual_saddle_stitch_creep", Order = 20)]
    public class PDF_VisualSaddleStitchCreep : IPdfTool
    {
        decimal _thickness;

        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                using (var form = new FormVisualSaddleStitchCreep(file))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _thickness = form.SelectedPaperThickness;
                        return true;
                    }
                }
            }

            return false;
        }

        public void Execute(PdfJobContext context)
        {
            double paperThickness = (double) _thickness;

            foreach (var file in context.InputFiles)
            {
                string inputPath = file.FullName;
                string directory = Path.GetDirectoryName(inputPath);
                string fileNameOnly = Path.GetFileNameWithoutExtension(inputPath);
                string extension = Path.GetExtension(inputPath);
                
                // Формуємо шлях до нового файлу з суфіксом _prepared
                string targetfile = Path.Combine(directory, $"{fileNameOnly}_prepared{extension}");

                using (PDFlib p = new PDFlib())
                {
                    try
                    {
                        p.begin_document(targetfile, "optimize=true");
                        int doc = p.open_pdi_document(inputPath, "");
                        int pagecount = (int)p.pcos_get_number(doc, "length:pages");

                        for (int i = 1; i <= pagecount; i++)
                        {
                            var page_handle = p.open_pdi_page(doc, i, "");
                            // Використовуємо існуючий хелпер для отримання боксів сторінки
                            var boxes = PdfHelper.GetBoxes(p, doc, i - 1);

                            double W = boxes.Trim.width;

                            // Розрахунок зміщення (creep) для поточної сторінки
                            double x = CreepCalculator.GetCreepForPage(i, pagecount, paperThickness) * PdfHelper.mn;

                            // Коефіцієнт масштабування: контент має звузитися на величину x
                            double scaleX = (W - x) / W;

                            double x_ofs;
                            string optlist = $"scale={{{scaleX} 1}}";
                            if (i % 2 != 0) // Непарна сторінка: фіксуємо ліву сторону (0,0)
                            {
                                x_ofs = 0;
                            }
                            else // Парна сторінка: фіксуємо праву сторону
                            {
                                x_ofs = x;
                            }

                            // Створюємо нову сторінку того ж розміру (MediaBox)
                            p.begin_page_ext(boxes.Media.width, boxes.Media.height, "");

                            // Вставляємо вміст з трансформацією
                            p.fit_pdi_page(page_handle, x_ofs, 0, optlist);

                            // Встановлюємо TrimBox для нової сторінки (щоб зберегти оригінальні межі обрізки)
                            p.end_page_ext($"trimbox {{{boxes.Trim.left} {boxes.Trim.bottom} {boxes.Trim.width + boxes.Trim.left} {boxes.Trim.bottom + boxes.Trim.height}}}");

                            p.close_pdi_page(page_handle);
                        }

                        p.close_pdi_document(doc);
                        p.end_document("");
                    }
                    catch (PDFlibException e)
                    {
                        PdfHelper.LogException(e, "PDF_VisualSaddleStitchCreep");
                    }
                }
            }
        }
    }
}
