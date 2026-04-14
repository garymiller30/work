using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class PageGroupsService
    {
        public static PageGroup CreateGroup(int id)
        {
            var group = new PageGroup();
            group.Id = id;
            return group;
        }

        public static void DistributeHor(TemplateSheet sheet, bool ignoreSheetFields, List<PageGroup> e)
        {
            var tc = sheet.TemplatePageContainer;

            //отримати ширину всіх груп, додати, від ширини задрукованої області листа відняти, поділити на кількість груп

            foreach (var group in e)
            {
                group.Pages = (tc.TemplatePages.Where(x => x.Group == group.Id)).ToList();
                group.Rectangle = ProcessSubject.GetSubjectRect(sheet, group.Pages);
            }

            double totalWidth = e.Sum(x => x.Rectangle.W);
            RectangleD sheetRect = ProcessSubject.GetSubjectRect(sheet, ignoreSheetFields);

            double delta = (sheetRect.W - totalWidth) / (e.Count * 2);


            double ofs = delta + sheetRect.Left;

            foreach (var group in e)
            {

                double x_ofs = group.Rectangle.X1 - ofs;

                foreach (var page in group.Pages)
                {
                    page.Front.X -= x_ofs;
                    ProcessFixPageBackPosition.FixPosition(sheet, page);
                }

                ofs += group.Rectangle.W + delta * 2;
            }
        }

        public static void DistributeVer(TemplateSheet sheet, bool ignoreSheetFields, List<PageGroup> e)
        {
            var tc = sheet.TemplatePageContainer;

            //отримати ширину всіх груп, додати, від ширини задрукованої області листа відняти, поділити на кількість груп

            foreach (var group in e)
            {
                group.Pages = (tc.TemplatePages.Where(x => x.Group == group.Id)).ToList();
                group.Rectangle = ProcessSubject.GetSubjectRect(sheet, group.Pages);
            }

            double totalHeight = e.Sum(x => x.Rectangle.H);
            RectangleD sheetRect = ProcessSubject.GetSubjectRect(sheet, ignoreSheetFields);

            double delta = (sheetRect.H - totalHeight) / (e.Count * 2);


            double ofs = delta + sheetRect.Bottom;

            foreach (var group in e)
            {

                double y_ofs = group.Rectangle.Y1 - ofs;

                foreach (var page in group.Pages)
                {
                    page.Front.Y -= y_ofs;
                    ProcessFixPageBackPosition.FixPosition(sheet, page);
                }

                ofs += group.Rectangle.H + delta * 2;
            }
        }

        public static void DeleteGroups(TemplateSheet sheet, List<PageGroup> e)
        {
            foreach (var group in e)
            {
               sheet.TemplatePageContainer.TemplatePages.Where(x=>x.Group == group.Id).ToList().ForEach(x => x.Group = 0);
            }
        }
    }
}
