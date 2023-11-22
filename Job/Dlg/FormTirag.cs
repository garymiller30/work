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
    public sealed partial class FormTirag : Form
    {
        public int Tirag => (int)numericUpDown1.Value;

        public FormTirag()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void numericUpDown1_Enter(object sender, EventArgs e)
        {
            numericUpDown1.Select(0, numericUpDown1.Text.Length);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
