using System;
using System.Windows.Forms;

namespace PluginWorkPrepress.Forms
{
    public partial class FormPay : Form
    {
        private PrepressProcess _process;
        public FormPay(PrepressProcess process)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _process = process;

            numericUpDown1.Value = process.Price - process.Pay;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            _process.AddPay(numericUpDown1.Value);
            Close();
        }

        private void numericUpDown1_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }
    }
}
