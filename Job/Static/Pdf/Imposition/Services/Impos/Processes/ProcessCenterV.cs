using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessCenterV
    {
        /// <summary>
        /// центруємо по вертикалі
        /// </summary>
        public static void Center(TemplateSheet sheet)
        {
            if (sheet == null) return;
            if (!sheet.TemplatePageContainer.TemplatePages.Any()) return;

            var rect = sheet.TemplatePageContainer.GetSubjectRectFront();

            var sheetPrintRect = sheet.GetPrintRect();

            var delta = (sheetPrintRect.H - rect.H)/2 - sheet.SafeFields.Bottom;

            var y_ofs = rect.Y1 - delta;

            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.Y -= y_ofs;
            }

        }
    }
}
