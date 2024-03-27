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
    public sealed partial class FormSelectDpi : Form
    {

        public int Dpi { get; set; }
        public long Quality { get;set; }

        public FormSelectDpi()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Dpi = (int)numericUpDown1.Value;
            Quality = (long) numericUpDown2.Value;
            Close();
        }
    }
}
