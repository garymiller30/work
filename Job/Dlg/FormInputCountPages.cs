using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.Dlg
{
    public partial class FormInputCountPages : Form
    {
        public int CountPages { get;set; } = 2;

        public FormInputCountPages()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CountPages = (int)numericUpDown1.Value;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
