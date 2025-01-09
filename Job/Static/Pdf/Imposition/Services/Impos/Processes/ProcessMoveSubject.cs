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
                item.X -= ofs;
            }
        }
        public static void Right(TemplateSheet sheet, double ofs)
        {
            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.X += ofs;
            }
        }

        public static void Up(TemplateSheet sheet, double ofs)
        {
            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.Y += ofs;
            }
        }

        public static void Down(TemplateSheet sheet, double ofs)
        {
            foreach (var item in sheet.TemplatePageContainer.TemplatePages)
            {
                item.Y -= ofs;
            }
        }
    }
}
