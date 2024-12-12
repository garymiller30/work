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
            if (parameters.Sheet == null || parameters.MasterPage == null) return;
            var par = CreateParameters();
            par.BindingPlace = BindingPlaceEnum.Normal;
            parameters.Sheet.TemplatePageContainer = BindingService.Impos(par);

            parameters.Sheet = parameters.Sheet;
        }

        private void b_90_Click(object sender, EventArgs e)
        {
            if (parameters.Sheet == null || parameters.MasterPage == null) return;
            var par = CreateParameters();
            par.BindingPlace = BindingPlaceEnum.Rotated;
            parameters.Sheet.TemplatePageContainer = BindingService.Impos(par);

            parameters.Sheet = parameters.Sheet;
        }

        private void b_max_Click(object sender, EventArgs e)
        {
            if (parameters.Sheet == null || parameters.MasterPage == null) return;
            
            var par = CreateParameters();
            par.BindingPlace = BindingPlaceEnum.Max;
            parameters.Sheet.TemplatePageContainer = BindingService.Impos(par);

            parameters.Sheet = parameters.Sheet;
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
    }
}
