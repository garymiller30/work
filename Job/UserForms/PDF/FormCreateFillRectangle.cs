using JobSpace.Static.Pdf.Create;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormCreateFillRectangle : Form
    {

        MarkColor _markColor;
        public PdfCreateFillRectangleParams FillRectangleParams { get; set; } = new PdfCreateFillRectangleParams();
        public decimal PdfWidth { get; set; }
        public decimal PdfHeight { get; set; }

        public FormCreateFillRectangle()
        {
            InitializeComponent();
            uc_PdfColorSelector1.OnColorSelected += Uc_PdfColorSelector1_OnColorSelected;
            DialogResult = DialogResult.Cancel;
            
        }

        private void Uc_PdfColorSelector1_OnColorSelected(object sender, EventArgs e)
        {
            _markColor = uc_PdfColorSelector1.MarkColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_markColor == null) {
                MessageBox.Show("Потрібно вибрати колір плашки");
                return;
            }

            FillRectangleParams = new PdfCreateFillRectangleParams
            {
                Width = (double)numericUpDown1.Value,
                Height = (double)numericUpDown2.Value,
                Bleeds = (double)nud_bleed.Value,
                
                Color = _markColor,
            };

            DialogResult = DialogResult.OK;
            Close();
        }


        private void numericUpDown1_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }
    }
}
