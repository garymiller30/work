using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Common
{
    //// Приклад використання:
    //public class Program
    //{
    //    public static void Main()
    //    {
    //        int totalPages = 32;
    //        double thickness = 0.12; // мм

    //        Console.WriteLine($"Загальне зміщення буклету: {CreepCalculator.GetTotalCreep(totalPages, thickness):F2} мм");

    //        int page = 11;
    //        double pageCreep = CreepCalculator.GetCreepForPage(page, totalPages, thickness);

    //        Console.WriteLine($"Зміщення для сторінки {page}: {pageCreep:F2} мм");

    //        // Виведемо зміщення для центрального розвороту (для перевірки)
    //        int centerPage = totalPages / 2;
    //        Console.WriteLine($"Зміщення для центру (стор. {centerPage}): {CreepCalculator.GetCreepForPage(centerPage, totalPages, thickness):F2} мм");
    //    }
    //}

    public class CreepCalculator
    {
        /// <summary>
     /// Розраховує загальне зміщення (creep) для найвнутрішнішого аркуша.
     /// </summary>
        public static double GetTotalCreep(int totalPages, double paperThickness)
        {
            if (totalPages % 4 != 0)
                throw new ArgumentException("Кількість сторінок має бути кратною 4.");

            int totalSheets = totalPages / 4;
            return (totalSheets - 1) * paperThickness;
        }

        /// <summary>
        /// Розраховує зміщення для конкретної сторінки (від 1 до totalPages).
        /// </summary>
        public static double GetCreepForPage(int pageNumber, int totalPages, double paperThickness)
        {
            if (pageNumber < 1 || pageNumber > totalPages)
                return 0;
                //throw new ArgumentOutOfRangeException(nameof(pageNumber), "Невірний номер сторінки.");

            // Визначаємо логічний номер аркуша (spread), рахуючи ззовні (від 1)
            // Для 32 сторінок: стор. 1-2 (арк. 1), стор. 15-16 (арк. 8)
            int sheetIndex;
            if (pageNumber <= totalPages / 2)
            {
                sheetIndex = (int)Math.Ceiling(pageNumber / 2.0);
            }
            else
            {
                // Для другої половини знайдемо парну сторінку з першої половини
                int mirroredPage = totalPages - pageNumber + 1;
                sheetIndex = (int)Math.Ceiling(mirroredPage / 2.0);
            }

            return (sheetIndex - 1) * paperThickness;
        }
    }
}
