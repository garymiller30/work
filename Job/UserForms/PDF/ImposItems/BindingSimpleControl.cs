using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding;
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
    public partial class BindingSimpleControl : UserControl, IBindControl
    {
        ControlBindParameters parameters;

        TemplatePageContainer variantNormal;
        TemplatePageContainer variantRotated;
        TemplatePageContainer variantMaxNormal;
        TemplatePageContainer variantMaxRotated;


        public BindingSimpleControl()
        {
            InitializeComponent();
        }

        public void SetControlBindParameters(ControlBindParameters p)
        {
            parameters = p;
            parameters.PropertyChanged += Parameters_PropertyChanged;
        }

        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void b_0_Click(object sender, EventArgs e)
        {
            parameters.Sheet.TemplatePageContainer = variantNormal.Copy() ;

            parameters.UpdateSheet();
        }

        private void b_90_Click(object sender, EventArgs e)
        {

            parameters.Sheet.TemplatePageContainer = variantRotated.Copy();

            parameters.UpdateSheet();
        }

        private void b_max_Click(object sender, EventArgs e)
        {
            parameters.Sheet.TemplatePageContainer = variantMaxNormal.Copy();

            parameters.UpdateSheet();
        }

        private void b_max_90_Click(object sender, EventArgs e)
        {
            parameters.Sheet.TemplatePageContainer = variantMaxRotated.Copy();
            parameters.UpdateSheet();

        }

        LooseBindingParameters CreateParameters()
        {
            LooseBindingParameters bindParam = new LooseBindingParameters();
            bindParam.Sheet = parameters.Sheet;
            bindParam.Sheet.MasterPage = parameters.MasterPage;
            bindParam.IsOneCut = cb_OneCut.Checked;
            bindParam.Xofs = (double)nud_Xofs.Value;
            bindParam.Yofs = (double)nud_Yofs.Value;
            bindParam.IsCenterHorizontal = cb_centerWidth.Checked;
            bindParam.IsCenterVertical = cb_centerHeight.Checked;

            return bindParam;
        }

        public void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages)
        {
            // скинути 
            pages.ForEach(p => p.IsAssumed = false);

            int maxIdx = 0;

            for (int i = 0; i < sheets.Count; i++)
            {
                foreach (var t_page in sheets[i].TemplatePageContainer.TemplatePages)
                {
                    if (t_page.MasterFrontIdx > 0)
                    {
                        var runListpageFrontIdx = maxIdx * i + t_page.MasterFrontIdx;
                        t_page.PrintFrontIdx = runListpageFrontIdx;
                        if (runListpageFrontIdx - 1 < pages.Count)
                            pages[runListpageFrontIdx - 1].IsAssumed = true;
                    }
                    else
                    {
                        t_page.PrintFrontIdx = 0;
                    }

                    if (t_page.MasterBackIdx > 0)
                    {
                        var runListpageBackIdx = maxIdx * i + t_page.MasterBackIdx;
                        t_page.PrintBackIdx = runListpageBackIdx;
                        if (runListpageBackIdx - 1 < pages.Count)
                            pages[runListpageBackIdx - 1].IsAssumed = true;
                    }
                    else
                    {
                        t_page.PrintBackIdx = 0;
                    }
                }

                maxIdx += sheets[i].TemplatePageContainer.GetMaxIdx();

            }
        }

        public void Calc()
        {
            if (parameters.Sheet == null || parameters.MasterPage == null) return;

            TemplatePageContainer sel;

            // Normal
            var par = CreateParameters();
            par.BindingPlace = BindingPlaceEnum.Normal;
            variantNormal = BindingService.Impos(par);
            label_0.Text = variantNormal.TemplatePages.Count().ToString();

            sel = variantNormal;

            //Rotated
            par.BindingPlace = BindingPlaceEnum.Rotated;
            variantRotated = BindingService.Impos(par);
            label_90.Text = variantRotated.TemplatePages.Count().ToString();

            if (sel.TemplatePages.Count() < variantRotated.TemplatePages.Count())
                sel = variantRotated;


            //Max Normal
            par.BindingPlace = BindingPlaceEnum.MaxNormal;
            variantMaxNormal = BindingService.Impos(par);
            label_max_0.Text = variantMaxNormal.TemplatePages.Count().ToString();

            if (sel.TemplatePages.Count() < variantMaxNormal.TemplatePages.Count())
                sel = variantMaxNormal;

            //Max Rotated
            par.BindingPlace = BindingPlaceEnum.MaxRotated;
            variantMaxRotated = BindingService.Impos(par);
            label_max_90.Text = variantMaxRotated.TemplatePages.Count().ToString();

            if (sel.TemplatePages.Count() < variantMaxRotated.TemplatePages.Count())
                sel = variantMaxRotated;

            parameters.Sheet.TemplatePageContainer = sel.Copy();

            parameters.UpdateSheet();
        }
    }
}
