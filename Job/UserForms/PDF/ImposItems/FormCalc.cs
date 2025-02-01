using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class FormCalc : Form
    {
        public double Result { get; private set; }
        public FormCalc()
        {
            InitializeComponent();
        }

        public FormCalc(decimal value):this()
        {
            tb_input.Text = value.ToString("N01").Replace(",",".");
        }

        private void btn_calc_Click(object sender, EventArgs e)
        {
            try
            {
                tb_result.Text = new DataTable().Compute(tb_input.Text, null).ToString();
                Result = double.Parse(tb_result.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("Невірний формат виразу");
            }
        }
    }
}
