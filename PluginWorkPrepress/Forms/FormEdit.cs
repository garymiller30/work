using System;
using System.Windows.Forms;
using System.Data;

namespace PluginWorkPrepress.Forms
{
    public partial class FormEdit : Form
    {
        private PrepressProcess _process;

        public FormEdit(PrepressProcess process)
        {
            InitializeComponent();
            _process = process;
            DialogResult = DialogResult.Cancel;

            textBox1.Text = process.Name;
            numericUpDownPrice.Value = process.Price;
            numericUpDownPreprice.Value = process.PrePrice;

        }

        private void numericUpDownPrice_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0,((NumericUpDown)sender).Text.Length);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {

            _process.Price = numericUpDownPrice.Value;
            _process.Name = textBox1.Text;
            _process.PrePrice = numericUpDownPreprice.Value;

            Close();
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            try
            {
                var val = decimal.Parse( new DataTable().Compute(textBox1.Text,null).ToString());
                numericUpDownPrice.Value = val;
            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }
        }
    }
}
