using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessCenterH
    {
        /// <summary>
        /// центруємо по горизонталі
        /// </summary>
        /// <param name="sheet"></param>
        public static void Center(TemplateSheet sheet)
        {
            if (sheet == null) return;
            if (!sheet.TemplatePageContainer.TemplatePages.Any()) return;

            // Вираховуємо розмір блоку сторінок
            // для цього знаходимо мінімальну X координату і максимальну X координату
            var rect = sheet.TemplatePageContainer.GetSubjectRectFront();

            var sheetPrintRect = sheet.GetPrintRect();

            var delta = (sheetPrintRect.W - rect.W) / 2;

            var x_ofs = rect.X1 - delta;

            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.X -= x_ofs - sheet.SafeFields.Left;
            }
        }
    }
}
