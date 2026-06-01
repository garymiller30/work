using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.Commons; // Припускаю, що PdfHelper знаходиться тут
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
        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                using (var form = new FormSpace.UserForms.PDF.Visual.FormVisualSaddleStitchCreep(file))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Execute(PdfJobContext context)
        {
            // Завантажуємо товщини паперу (аналогічно формі)
            var thicknesses = SaveAndLoad.LoadPaperThicknesses();
            // Використовуємо першу доступну товщину як базову для розрахунку, 
            // оскільки в Execute немає UI для вибору конкретної товщини
            double paperThickness = thicknesses.Count > 0 ? (double)thicknesses.Values.First() : 0;

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
                        int pagecount = p.pcos_get_number(doc, "length:pages");

                        for (int i = 1; i <= pagecount; i++)
                        {
                            var page_handle = p.open_pdi_page(doc, i, "");
                            // Використовуємо існуючий хелпер для отримання боксів сторінки
                            var boxes = PdfHelper.GetBoxes(p, doc, i - 1);

                            double W = boxes.Trim.width;
                            double H = boxes.Trim.height;
                            
                            // Розрахунок зміщення (creep) для поточної сторінки
                            double x = CreepCalculator.GetCreepForPage(i, pagecount, paperThickness);

                            // Коефіцієнт масштабування: контент має звузитися на величину x
                            double scaleX = (W - x) / W;
                            
                            string optlist = "";

                            if (i % 2 != 0) // Непарна сторінка: фіксуємо ліву сторону (0,0)
                            {
                                // Масштабуємо по X, Y не змінюється. Трансляція 0,0 за замовчуванням.
                                optlist = $"scale={scaleX},1";
                            }
                            else // Парна сторінка: фіксуємо праву сторону
                            {
                                // Щоб права сторона залишилася на місці (W), після масштабування 
                                // контент треба зсунути вправо на величину x.
                                optlist = $"scale={scaleX},1,translate={x},0";
                            }

                            // Створюємо нову сторінку того ж розміру (MediaBox)
                            p.begin_page_ext(boxes.Media.width, boxes.Media.height, "");
                            
                            p.begin_layer("print", "");
                            // Вставляємо вміст з трансформацією
                            p.fit_pdi_page(page_handle, 0, 0, optlist);
                            p.end_layer();

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
