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
                    var pageIdx = i * sides + (t_page.Front.MasterIdx - 1) * sheets.Count;
                    t_page.Front.PrintIdx = pageIdx + 1;
                    if (pageIdx < pages.Count)
                    {
                        pages[pageIdx].IsAssumed = true;
                        pages[pageIdx].IsValidFormat = ValidateFormat(pages[pageIdx], t_page);
                        t_page.Front.AssignedRunPage = pages[pageIdx];
                    }


                    if (t_page.Back.MasterIdx > 0)
                    {
                        pageIdx++;
                        t_page.Back.PrintIdx = pageIdx + 1;
                        if (pageIdx < pages.Count)
                        {
                            pages[pageIdx].IsAssumed = true;
                            pages[pageIdx].IsValidFormat = ValidateFormat(pages[pageIdx], t_page);
                            t_page.Back.AssignedRunPage = pages[pageIdx];
                        }
                    }
                    else
                    {
                        t_page.Back.PrintIdx = 0;
                    }
                }
            }
        }
    }
}
