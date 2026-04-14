using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class BindingSimpleSpreadControl: UserControl, IBindControl
    {
        public BindingSimpleSpreadControl()
        {
            InitializeComponent();
        }

        public void Calc()
        {
            
        }

        public void CheckRunListPages(List<PrintSheet> printSheets, List<ImposRunPage> imposRunPages)
        {
           
        }

        public void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages)
        {
            
        }
        public void SetControlBindParameters(GlobalImposParameters imposParam)
        {
           
        }
    }
}
