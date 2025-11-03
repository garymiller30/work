using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Merge
{
    public sealed class MergeBlockBy3Months
    {
        public void Run(string file)
        {
            string targetfile = Path.Combine(
                Path.GetDirectoryName(file),
                $"{Path.GetFileNameWithoutExtension(file)}_3months{Path.GetExtension(file)}");

            PDFlib p = new PDFlib();

            try
            {
                p.begin_document(targetfile, "optimize=true");
                int indoc = p.open_pdi_document(file, "");
                double pagecount = p.pcos_get_number(indoc, "length:pages");

                if (pagecount != 14) throw new PDFlibException(-1, "MergeBlockBy3Months", "The document must contain 14 pages.");

                // буде 12 сторінок в документі по 3 місяці кожна: зверху - попередній місяць, посередині - поточний місяць, знизу - наступний місяць
                // кожна сторінка по висоті = 1/3 від висоти оригінальної сторінки
                for (int i = 1; i <= 12; i++)
                {
                    var page1_handle = p.open_pdi_page(indoc, i, "");
                    var page2_handle = p.open_pdi_page(indoc, i+1, "");
                    var page3_handle = p.open_pdi_page(indoc, i+2, "");

                    // Отримання розмірів сторінок
                    double width1 = p.info_pdi_page(page1_handle, "pagewidth", ""); // Ширина сторінки 1
                    double height1 = p.info_pdi_page(page1_handle, "pageheight", ""); // Висота сторінки 1

                    double width2 = p.info_pdi_page(page2_handle, "pagewidth", "");
                    double height2 = p.info_pdi_page(page2_handle, "pageheight", "");

                    double width3 = p.info_pdi_page(page3_handle, "pagewidth", "");
                    double height3 = p.info_pdi_page(page3_handle, "pageheight", "");

                    // Визначення розмірів вихідної сторінки
                    // Нова ширина - максимальна з трьох сторінок
                    double new_width = Math.Max(width1, Math.Max(width2, width3));

                    // Нова висота - сума висот усіх трьох сторінок
                    double new_height = height1 + height2 + height3;


                    // 5. Початок вихідної сторінки (з достатньою висотою)
                    // Використовуємо begin_page_ext для встановлення розмірів сторінки [13]
                    p.begin_page_ext(new_width, new_height, "");
                    // верхня частина - попередній місяць
                    p.fit_pdi_page(page1_handle, 0, height3 + height2, "");
                    // середня частина - поточний місяць
                    p.fit_pdi_page(page2_handle, 0, height3, "");
                    // нижня частина - наступний місяць
                    p.fit_pdi_page(page3_handle, 0, 0, "");
                    p.end_page_ext("");

                    p.close_pdi_page(page1_handle);
                    p.close_pdi_page(page2_handle);
                    p.close_pdi_page(page3_handle);
                }

                p.close_pdi_document(indoc);
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "MergeBlockBy3Months");
            }
            finally { p?.Dispose(); }
        }
    }
}
