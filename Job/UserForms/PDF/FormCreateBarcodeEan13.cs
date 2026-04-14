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
    public partial class FormCreateBarcodeEan13 : Form
    {

        public string Barcode => tb_barcode.Text.Trim();

        public FormCreateBarcodeEan13()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        bool IsValidEan13(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            if (code.Length != 13 || !code.All(char.IsDigit))
                return false;

            int sum = 0;

            for (int i = 0; i < 12; i++)
            {
                int digit = code[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int check = (10 - (sum % 10)) % 10;

            return check == (code[12] - '0');
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            if (IsValidEan13(tb_barcode.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Невірний код EAN13. Код повинен містити 13 цифр, де остання є контрольним числом.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
