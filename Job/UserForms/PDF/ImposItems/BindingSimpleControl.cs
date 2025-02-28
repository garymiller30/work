using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise;
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
        protected ControlBindParameters parameters;

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
            ApplyTemplatePages(variantNormal);
        }

        private void b_90_Click(object sender, EventArgs e)
        {
            ApplyTemplatePages(variantRotated);
        }

        private void b_max_Click(object sender, EventArgs e)
        {
            ApplyTemplatePages(variantMaxNormal);
        }

        private void b_max_90_Click(object sender, EventArgs e)
        {
            ApplyTemplatePages(variantMaxRotated);
        }

        private void ApplyTemplatePages(TemplatePageContainer variant)
        {
            if (variant != null && parameters != null)
            {
                parameters.Sheet.TemplatePageContainer.SetTemplatePages(variant.TemplatePages);
                parameters.UpdatePreview();
            }
        }

        LooseBindingParameters CreateParameters()
        {
            LooseBindingParameters bindParam = new LooseBindingParameters();
            bindParam.Sheet = parameters.Sheet;
            bindParam.Sheet.MasterPage = parameters.MasterPage;
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
                    if (t_page.Front.MasterIdx > 0)
                    {
                        var runListpageFrontIdx = maxIdx + t_page.Front.MasterIdx;
                        t_page.Front.PrintIdx = runListpageFrontIdx;

                        int runPageIdx = runListpageFrontIdx - 1;

                        if (runPageIdx < pages.Count)
                        {
                            ImposRunPage runPage = pages[runPageIdx];

                            runPage.IsAssumed = true;
                            runPage.IsValidFormat = ValidateFormat(runPage, t_page);
                            t_page.Front.AssignedRunPage = runPage;
                        }
                    }
                    else
                    {
                        t_page.Front.PrintIdx = 0;
                    }

                    if (t_page.Back.MasterIdx > 0)
                    {
                        var runListpageBackIdx = maxIdx + t_page.Back.MasterIdx;
                        t_page.Back.PrintIdx = runListpageBackIdx;

                        int runPageIdx = (runListpageBackIdx - 1);

                        if (runPageIdx < pages.Count)
                        {
                            ImposRunPage runPage = pages[runPageIdx];

                            runPage.IsAssumed = true;
                            runPage.IsValidFormat = ValidateFormat(pages[runPageIdx], t_page);
                            t_page.Back.AssignedRunPage = runPage;
                        }
                    }
                    else
                    {
                        t_page.Back.PrintIdx = 0;
                    }
                }

                maxIdx += sheets[i].TemplatePageContainer.GetMaxIdx();

            }
        }

        protected bool ValidateFormat(ImposRunPage imposRunPage, TemplatePage page)
        {
            var pdf_page = PdfFileService.GetPage(parameters.PdfFiles, imposRunPage);
            if (pdf_page == null) return false;

            //check format with admission 0.5mm
            if (Math.Abs(pdf_page.Trim.W - page.W) > 0.5 || Math.Abs(pdf_page.Trim.H - page.H) > 0.5)
            {
                return false;
            }

            return true;
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

            parameters.Sheet.TemplatePageContainer.SetTemplatePages(sel.TemplatePages);

            parameters.UpdateSheet();
        }

        public void CheckRunListPages(List<PrintSheet> printSheets, List<ImposRunPage> pages)
        {
            pages.ForEach(p => p.IsAssumed = false);

            foreach (var sheet in printSheets)
            {
                foreach (var t_page in sheet.TemplatePageContainer.TemplatePages)
                {
                    if (t_page.Front.PrintIdx > 0)
                    {
                        if (t_page.Front.PrintIdx - 1 < pages.Count)
                        {
                            ImposRunPage runPage = pages[t_page.Front.PrintIdx - 1];

                            runPage.IsAssumed = true;
                            runPage.IsValidFormat = ValidateFormat(runPage, t_page);
                            t_page.Front.AssignedRunPage = runPage;
                        }
                    }
                    if (t_page.Back.PrintIdx > 0)
                    {
                        if (t_page.Back.PrintIdx - 1 < pages.Count)
                        {
                            ImposRunPage runPage = pages[t_page.Back.PrintIdx - 1];

                            runPage.IsAssumed = true;
                            runPage.IsValidFormat = ValidateFormat(runPage, t_page);
                            t_page.Back.AssignedRunPage = runPage;
                        }
                    }
                }
            }
        }

    }
}
