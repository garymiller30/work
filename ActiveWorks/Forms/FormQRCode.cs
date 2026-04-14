using QRCoder;
using System;
using System.IO;
using System.Windows.Forms;

namespace ActiveWorks.Forms
{
    public partial class FormQRCode : Form
    {
        public FormQRCode()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    CreateQr(textBox1.Text, saveFileDialog1.FileName);
                    Close();
                }

            }
        }

        private void CreateQr(string data, string fileName)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            PostscriptQRCode qrCode = new PostscriptQRCode(qrCodeData);
            string qrCodeAsPostscript = qrCode.GetGraphic(20, true);

            File.WriteAllText(fileName, qrCodeAsPostscript);
        }
    }
}
