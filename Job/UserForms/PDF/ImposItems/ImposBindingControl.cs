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

        List<string> items = new List<string>(){"розкидати на лист"};

        public ImposBindingControl()
        {
            InitializeComponent();
            cb_SelectBindType.Items.AddRange(items.ToArray());
        }

        public void SetControlBindParameters(ControlBindParameters controlBindParameters)
        {
            parameters = controlBindParameters;
            parameters.PropertyChanged += Parameters_PropertyChanged;
        }

        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        private void cb_SelectBindType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_SelectBindType.SelectedIndex == 0)
            {
                //LooseBinding
                panelBindingControl.Controls.Clear();
                var control = new BindingSimpleControl();
                
                curBindControl = control;
                curBindControl.SetControlBindParameters(parameters);

                control.Dock = DockStyle.Fill;
                panelBindingControl.Controls.Add(control);
            }
        }
    }
}
