using JobSpace.Static.Pdf.Create.BigovkaMarks;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormCreateBigovkaMarks : Form
    {

        public CreateBigovkaMarksParams BigovkaMarksParams { get; set; } = new CreateBigovkaMarksParams();

        public FormCreateBigovkaMarks()
        {
            InitializeComponent();

            cb_mirrorEven.DataBindings.Add("Enabled", radioButtonHor,"Checked");

            InitUi();
            DialogResult = DialogResult.Cancel;
        }

        private void InitUi()
        {
            cb_mirrorEven.Checked = BigovkaMarksParams.MirrorEven;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            
            if (CreateParameters())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else { MessageBox.Show("Перевір біговки"); return; }
            
        }

        private bool CreateParameters()
        {
            BigovkaMarksParams.Direction = radioButtonHor.Checked ? Static.Pdf.Common.DirectionEnum.Horizontal : Static.Pdf.Common.DirectionEnum.Vertical;
            BigovkaMarksParams.Bleed = (double)numBleed.Value;
            BigovkaMarksParams.Length = (double)numLen.Value;
            BigovkaMarksParams.DistanceFromTrim = (double)numDistanse.Value;
            BigovkaMarksParams.Color.C = (double)numC.Value;
            BigovkaMarksParams.Color.M = (double)numM.Value;
            BigovkaMarksParams.Color.Y = (double)numY.Value;
            BigovkaMarksParams.Color.K = (double)numK.Value;
            BigovkaMarksParams.MirrorEven = cb_mirrorEven.Checked;

            string[]bigovki = textBoxBigovky.Text.Trim(' ').Split(' ');

            BigovkaMarksParams.Bigovki = new double[bigovki.Length];

            for (int i = 0; i < bigovki.Length; i++)
            {
                if (!double.TryParse(bigovki[i], NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture, out double result))
                {
                    return false;
                }
                else
                {
                    BigovkaMarksParams.Bigovki[i] = result;
                }
            }
            return true;
        }

        private void cb_c_CheckedChanged(object sender, EventArgs e)
        {
            numC.Value = cb_c.Checked ? 100 : 0;
        }

        private void cb_m_CheckedChanged(object sender, EventArgs e)
        {
            numM.Value = cb_m.Checked ? 100 : 0;
        }

        private void cb_y_CheckedChanged(object sender, EventArgs e)
        {
            numY.Value = cb_y.Checked ? 100 : 0;
        }

        private void cb_b_CheckedChanged(object sender, EventArgs e)
        {
            numK.Value = cb_b.Checked ? 100 : 0;
        }

        private void numDistanse_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown) sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }
    }
}
