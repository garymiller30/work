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
            if (sheet?.TemplatePageContainer?.TemplatePages == null || !sheet.TemplatePageContainer.TemplatePages.Any())
                return;

            try
            {
                // Calculate the bounding rectangle of the pages
                var subjectRect = sheet.TemplatePageContainer.GetSubjectRectFront();

                // Get the printable area of the sheet
                var printRect = sheet.GetPrintRect();

                // Calculate the horizontal offset needed to center the content
                var horizontalOffset = (printRect.W - subjectRect.W) / 2;

                // Calculate the new X offset
                var newXOffset = subjectRect.X1 - horizontalOffset;

                // Adjust the X position of each page to center them
                foreach (var page in sheet.TemplatePageContainer.TemplatePages)
                {
                    page.Front.X -= newXOffset - sheet.SafeFields.Left;
                    ProcessFixPageBackPosition.FixPosition(sheet, page);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred while centering the sheet: {ex.Message}");
            }
        }
    }
}
