using Job.Static.Pdf.Common;
using Job.Static.Pdf.Scale;
using System;
using System.Windows.Forms;

namespace Job.UserForms
{
    public partial class FormSelectPdfNewSize : Form
    {

        public PdfScaleParams Params { get; set; } = new PdfScaleParams();

        public FormSelectPdfNewSize()
        {
            InitializeComponent();
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
    }
}
