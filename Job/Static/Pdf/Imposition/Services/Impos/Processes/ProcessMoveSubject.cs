using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessMoveSubject
    {
        public static void Left(TemplateSheet sheet, double ofs)
        {
            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.Front.X -= ofs;
                ProcessFixPageBackPosition.FixPosition(sheet, item);
            }
        }
        public static void Right(TemplateSheet sheet, double ofs)
        {
            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.Front.X += ofs;
                ProcessFixPageBackPosition.FixPosition(sheet, item);
            }
        }

        public static void Up(TemplateSheet sheet, double ofs)
        {
            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.Front.Y += ofs;
                ProcessFixPageBackPosition.FixPosition(sheet, item);
            }
        }

        public static void Down(TemplateSheet sheet, double ofs)
        {
            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.Front.Y -= ofs;
                ProcessFixPageBackPosition.FixPosition(sheet, item);
            }
        }

        public static void CenterVer(TemplateSheet sheet)
        {
            if (sheet?.TemplatePageContainer?.TemplatePages == null || !sheet.TemplatePageContainer.TemplatePages.Any())
                return;

            var subjectRect = ProcessSubject.GetSubjectRect(sheet, sheet.TemplatePageContainer);
            var printRect = ProcessSubject.GetSubjectRect(sheet);

            // Calculate the vertical offset needed to center the content
            var verticalOffset = (printRect.H - subjectRect.H) / 2;
            var adjustedY = subjectRect.Y1 - verticalOffset;

            // Adjust the Y position of each template page
            foreach (var page in sheet.TemplatePageContainer.TemplatePages)
            {
                page.Front.Y -= (adjustedY - sheet.SafeFields.Bottom);
                ProcessFixPageBackPosition.FixPosition(sheet, page);
            }
        }

        public static void CenterHor(TemplateSheet sheet)
        {
            if (sheet?.TemplatePageContainer?.TemplatePages == null || !sheet.TemplatePageContainer.TemplatePages.Any())
                return;

            try
            {
                // Calculate the bounding rectangle of the pages
                var subjectRect = ProcessSubject.GetSubjectRect(sheet, sheet.TemplatePageContainer);

                // Get the printable area of the sheet
                var printRect = ProcessSubject.GetSubjectRect(sheet);

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
