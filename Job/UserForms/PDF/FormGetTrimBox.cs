using System;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;
using Interfaces;
using Job.Models;
using System.Collections.Generic;
using System.Linq;

namespace Job.UserForms
{


    public sealed partial class FormGetTrimBox : KryptonForm
    {
        private IFileSystemInfoExt InfoExt;
        public readonly TrimBoxResult Result = new TrimBoxResult();

        public FormGetTrimBox()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormGetTrimBox(IFileSystemInfoExt fileSystemInfoExt) : this()
        {
            InfoExt = fileSystemInfoExt;
            numericUpDownWidth.Value = InfoExt.Format.Width;
            numericUpDownHeight.Value = InfoExt.Format.Height;

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (byFormat.Checked)
            {
                Result.ResultType = TrimBoxResultEnum.byTrimbox;
                Result.TrimBox = new RectangleF(0, 0, (float)numericUpDownWidth.Value, (float)numericUpDownHeight.Value);
            }
            else if (radioButtonByBleeds.Checked)
            {
                Result.ResultType = TrimBoxResultEnum.byBleed;
                Result.Bleed = (double)numericUpDownByBleeds.Value;
            }
            else if (radioButtonBySpread.Checked)
            {
                Result.ResultType = TrimBoxResultEnum.bySpread;
                Result.Spread = new SpreadBox
                {
                    Bottom = (double)numericUpDownBottom.Value,
                    Inside = (double)numericUpDownInside.Value,
                    Top = (double)numericUpDownTop.Value,
                    Outside = (double)numericUpDownOutside.Value
                };
            }


        }

        private void numericUpDownWidth_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void radioButtonByBleeds_Click(object sender, EventArgs e)
        {
            numericUpDownByBleeds.Focus();
            numericUpDownByBleeds.Select(0, numericUpDownByBleeds.Text.Length);
        }

        private void radioButtonByFormat_Click(object sender, EventArgs e)
        {
            numericUpDownWidth.Focus();
            numericUpDownWidth.Select(0, numericUpDownWidth.Text.Length);
        }

        private void kryptonButtonBleed2_Click(object sender, EventArgs e)
        {
            SetBleed(2.0);
        }

        private void SetBleed(double value)
        {
            Result.ResultType = TrimBoxResultEnum.byBleed;
            Result.Bleed = value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void kryptonButtonBleed1_Click(object sender, EventArgs e)
        {
            SetBleed(1.0);
        }

        private void kryptonButtonBleed5_Click(object sender, EventArgs e)
        {
            SetBleed(5.0);
        }

        private void kryptonButtonBleed3_Click(object sender, EventArgs e)
        {
            SetBleed(3.0);
        }

        private void kryptonButtonExChange_Click(object sender, EventArgs e)
        {
            var temp = numericUpDownWidth.Value;
            numericUpDownWidth.Value = numericUpDownHeight.Value;
            numericUpDownHeight.Value = temp;

        }

        private void ucSelectStandartPageFormat1_PaperFormatChanged(object sender, Static.Pdf.Common.PaperFormat e)
        {
            numericUpDownWidth.Value = e.Width;
            numericUpDownHeight.Value = e.Height;
        }
    }
}
