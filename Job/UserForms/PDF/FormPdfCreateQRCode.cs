using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormPdfCreateQRCode : Form
    {
        QRCodeGenerator qrGenerator;

        string _target_folder;

        public FormPdfCreateQRCode()
        {
            InitializeComponent();
            qrGenerator = new QRCodeGenerator();
        }

        public void SetTargetFolder(string target_folder)
        {
            _target_folder = target_folder;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GeneratePreview();
        }


        void GeneratePreview()
        {
            string str = textBox1.Text;
            if (string.IsNullOrEmpty(str))
                return;

             QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
             Bitmap qrCodeImage = new QRCode(qrCodeData).GetGraphic(20);
             pictureBox1.Image = qrCodeImage;
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter the text to encode in the QR code.");
                return;
            }
            CreatePdfWithQRCode(textBox1.Text);
        }

        private void CreatePdfWithQRCode(string text)
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            PdfByteQRCode qrCode = new PdfByteQRCode(qrCodeData);
            byte[] qrCodeAsPdfByteArr = qrCode.GetGraphic(20);
            string pdfText = System.Text.Encoding.ASCII.GetString(qrCodeAsPdfByteArr);
            pdfText = pdfText.Replace("1 1 1 rg", "0 0 0 0 k");
            pdfText = pdfText.Replace("0 0 0 rg", "0 0 0 1 k");

            // Зберегаємо PDF-файл у вказаній папці. Якщо такий файл вже існує, додаємо суфікс до імені файлу.
            string filePath = System.IO.Path.Combine(_target_folder, "qrcode.pdf");
            int fileIndex = 1;
            while (System.IO.File.Exists(filePath))
            {
                filePath = System.IO.Path.Combine(_target_folder, $"qrcode_{fileIndex}.pdf");
                fileIndex++;
            }
            File.WriteAllText(filePath, pdfText);
        }
    }
}
