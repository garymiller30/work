using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
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
        public string CodeValue
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

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
            CreateQRCodeIText(textBox1.Text);
        }

        private void CreateQRCodeIText(string text)
        {
            string filePath = System.IO.Path.Combine(_target_folder, "qrcode.pdf");
            int fileIndex = 1;
            while (System.IO.File.Exists(filePath))
            {
                filePath = System.IO.Path.Combine(_target_folder, $"qrcode_{fileIndex}.pdf");
                fileIndex++;
            }

            PdfWriter writer = new PdfWriter(filePath);
            PdfDocument pdf = new PdfDocument(writer);

            BarcodeQRCode qr = new BarcodeQRCode(text);
            var form = qr.CreateFormXObject(
               new DeviceCmyk(0, 0, 0, 100),  // QR колір (100% K)
               pdf
           );

            float width = form.GetWidth();
            float height = form.GetHeight();

            var page = pdf.AddNewPage(new PageSize(width, height));
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.AddXObjectAt(form, 0, 0);
            pdf.Close();

        }
    }
}
