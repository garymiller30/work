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
    public partial class MarksControl : UserControl
    {
        TemplateSheet _sheet;
        public MarksControl()
        {
            InitializeComponent();
        }

        public void SetSheet(TemplateSheet e)
        {
            _sheet = e;
        }
    }
}
