using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormCreateBarcodeCode128 : Form
    {
        public string Barcode => tb_barcode.Text.Trim();

        public FormCreateBarcodeCode128()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public static bool IsValidCode128(string code)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            return code.All(c => c >= 0 && c <= 127);
        }


        private void btn_create_Click(object sender, EventArgs e)
        {
            if (IsValidCode128(tb_barcode.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Невірний код Code128. Код повинен містити лише символи ASCII (0-127).", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
