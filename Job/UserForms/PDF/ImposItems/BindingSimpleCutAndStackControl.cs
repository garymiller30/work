using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class BindingSimpleCutAndStackControl : BindingSimpleControl, IBindControl
    {
        public BindingSimpleCutAndStackControl() : base()
        {
            InitializeComponent();
        }
        new public void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages)
        {
            // скинути 
            pages.ForEach(p => p.IsAssumed = false);

            for (int i = 0; i < sheets.Count; i++)
            {
                int sides = sheets[i].SheetPlaceType == TemplateSheetPlaceType.SingleSide ? 1 : 2;

                foreach (var t_page in sheets[i].TemplatePageContainer.TemplatePages)
                {
                    var pageIdx = i*sides  + (t_page.MasterFrontIdx - 1) * sheets.Count;
                    t_page.PrintFrontIdx = pageIdx + 1;
                    if (pageIdx < pages.Count) pages[pageIdx].IsAssumed = true;

                    if (t_page.MasterBackIdx > 0)
                    {
                        pageIdx++;
                        t_page.PrintBackIdx = pageIdx + 1;
                        if (pageIdx < pages.Count) pages[pageIdx].IsAssumed = true;
                    }
                    else
                    {
                        t_page.PrintBackIdx = 0;
                    }
                    //if (t_page.MasterFrontIdx > 0)
                    //{
                    //    var pageIdx = i + (t_page.MasterFrontIdx - 1) * sheets.Count;
                    //    t_page.PrintFrontIdx = pageIdx+1;
                    //    if (pageIdx < pages.Count)
                    //        pages[pageIdx].IsAssumed = true;
                    //}
                    //else
                    //{
                    //    t_page.PrintFrontIdx = 0;
                    //}

                    //if (t_page.MasterBackIdx > 0)
                    //{
                    //    var pageIdx = i + (t_page.MasterBackIdx - 1) * sheets.Count;
                    //    t_page.PrintBackIdx = pageIdx+1;
                    //    if (pageIdx < pages.Count)
                    //        pages[pageIdx].IsAssumed = true;
                    //}
                    //else
                    //{
                    //    t_page.PrintBackIdx = 0;
                    //}

                }

            }
        }
    }
}
