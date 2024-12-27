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
            if(sheet?.TemplatePageContainer?.TemplatePages == null || !sheet.TemplatePageContainer.TemplatePages.Any())
                return;

            var subjectRect = sheet.TemplatePageContainer.GetSubjectRectFront();
            var printRect = sheet.GetPrintRect();

            // Calculate the vertical offset needed to center the content
            var verticalOffset = (printRect.H - subjectRect.H) / 2;
            var adjustedY = subjectRect.Y1 - verticalOffset;

            // Adjust the Y position of each template page
            foreach (var page in sheet.TemplatePageContainer.TemplatePages)
            {
                page.Y -= (adjustedY - sheet.SafeFields.Bottom);
            }
        }
    }
}
