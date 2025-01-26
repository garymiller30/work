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
    }
}
