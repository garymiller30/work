using ImageMagick;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Scale;
using System;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public partial class FormSelectPdfNewSize : Form
    {

        public PdfScaleParams Params { get; set; } = new PdfScaleParams();

        public FormSelectPdfNewSize()
        {
            InitializeComponent();
            comboBoxWorH.SelectedIndex = 0;
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Params.ScaleVariant = radioButtonNonProportial.Checked ? ScaleVariantEnum.NonProportial : ScaleVariantEnum.Proportial;
            Params.ScaleBy = radioButtonMediabox.Checked ? ScaleByEnum.Mediabox : ScaleByEnum.TrimBox;

            Params.TargetSize.Width = (double)numericUpDownWidth.Value;
            Params.TargetSize.Height = (double)numericUpDownHeight.Value;
            Params.TargetSize.Bleed = (double)numericUpDownBleed.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void numericUpDownWidth_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void ucSelectStandartPageFormat1_PaperFormatChanged(object sender, PaperFormat e)
        {
            numericUpDownWidth.Value = e.Width;
            numericUpDownHeight.Value = e.Height;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var tmp = numericUpDownWidth.Value;
            numericUpDownWidth.Value = numericUpDownHeight.Value;
            numericUpDownHeight.Value = tmp;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            labelWorH.Text = comboBoxWorH.SelectedIndex == 0 ? "висота": "ширина";
            Calc();
        }

        private void buttonGarazd_Click(object sender, EventArgs e)
        {
            var res = Calc();
            numericUpDownWidth.Value = res.Item1;
            numericUpDownHeight.Value = res.Item2;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Calc();
        }

        Tuple<decimal, decimal> Calc()
        {
            decimal val = numericUpDownVal.Value;
            var width = numericUpDownWidth.Value;
            var height = numericUpDownHeight.Value;

            decimal percent;

            if (comboBoxWorH.SelectedIndex == 0)
            {
                percent = width / val;
                decimal height_scaled = height / percent;
                textBox1.Text = height_scaled.ToString("N01");
                return Tuple.Create(val, height_scaled);
            }
            else
            {
                percent = height / val;
                decimal width_scaled = width / percent;
                textBox1.Text = width_scaled.ToString("N01");
                return Tuple.Create(width_scaled, val);
            }

        }
    }
}
