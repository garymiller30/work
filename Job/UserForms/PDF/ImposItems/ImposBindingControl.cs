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
    public partial class ImposBindingControl : UserControl
    {
        ControlBindParameters parameters;
        //TemplateSheet sheet;
        IBindControl curBindControl;

        List<string> items = new List<string>(){
            "розкидати на лист",
            "наскрізна нумерація"
            };

        public ImposBindingControl()
        {
            InitializeComponent();
            
            
        }

        public void SetControlBindParameters(ControlBindParameters controlBindParameters)
        {
            parameters = controlBindParameters;
            parameters.PropertyChanged += Parameters_PropertyChanged;
            
            cb_SelectBindType.Items.AddRange(items.ToArray());
            cb_SelectBindType.SelectedIndex = 0;
        }

        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Sheet" || parameters.Sheet == null || parameters.Sheet.TemplatePageContainer.TemplatePages.Count > 0) return;
            
            Calc();
        }

        private void cb_SelectBindType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (parameters == null) return;
            panelBindingControl.Controls.Clear();
            int idx = cb_SelectBindType.SelectedIndex;

            if (idx == 0)
            {
                //LooseBinding
                curBindControl = new BindingSimpleControl();
                
            }
            else if (idx == 1)
            {
                // наскрізна нумерація
                curBindControl = new BindingSimpleCutAndStackControl();
            }

            curBindControl.SetControlBindParameters(parameters);

            ((UserControl)curBindControl).Dock = DockStyle.Fill;
            panelBindingControl.Controls.Add((UserControl)curBindControl);
        }

        public void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages)
        {
            if (curBindControl == null) return;

            curBindControl.RearangePages(sheets,pages);
        }

        private void b_calc_Click(object sender, EventArgs e)
        {
            Calc();
        }

        private void Calc()
        {
            if (curBindControl == null) return;
            curBindControl.Calc();
        }

        internal void CheckRunListPages(List<PrintSheet> printSheets, List<ImposRunPage> imposRunPages)
        {
            if (curBindControl == null) return;

            curBindControl.CheckRunListPages(printSheets, imposRunPages);
        }
    }
}
