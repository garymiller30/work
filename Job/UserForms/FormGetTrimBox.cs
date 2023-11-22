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
        class PaperFormat
        {
            public int Number { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        private IFileSystemInfoExt InfoExt;

         readonly Dictionary<string, List<PaperFormat>> PaperStandarts = new Dictionary<string, List<PaperFormat>>(3, StringComparer.Ordinal){
            {"A",new List<PaperFormat>(10){
                new PaperFormat(){Number=0,Width=841,Height=1189},
                new PaperFormat(){Number=1,Width=594,Height=841},
                new PaperFormat(){Number=2,Width=420,Height=594},
                new PaperFormat(){Number=3,Width=297,Height=420},
                new PaperFormat(){Number=4,Width=210,Height=297},
                new PaperFormat(){Number=5,Width=148,Height=210},
                new PaperFormat(){Number=6,Width=105,Height=148},
                new PaperFormat(){Number=7,Width=74,Height=105},
                new PaperFormat(){Number=8,Width=52,Height=74},
                new PaperFormat(){Number=9,Width=37,Height=52},
                new PaperFormat(){Number=10,Width=26,Height=37},
                } },
            {"B",new List<PaperFormat>(10){
                new PaperFormat(){Number=0,Width=1000,Height=1414},
                new PaperFormat(){Number=1,Width=707,Height=1000},
                new PaperFormat(){Number=2,Width=500,Height=707},
                new PaperFormat(){Number=3,Width=353,Height=500},
                new PaperFormat(){Number=4,Width=250,Height=353},
                new PaperFormat(){Number=5,Width=176,Height=250},
                new PaperFormat(){Number=6,Width=125,Height=176},
                new PaperFormat(){Number=7,Width=88,Height=125},
                new PaperFormat(){Number=8,Width=62,Height=88},
                new PaperFormat(){Number=9,Width=44,Height=62},
                new PaperFormat(){Number=10,Width=31,Height=44},
                } },
            {"C",new List<PaperFormat>(10){
                new PaperFormat(){Number=0,Width=917,Height=1297},
                new PaperFormat(){Number=1,Width=648,Height=917},
                new PaperFormat(){Number=2,Width=458,Height=648},
                new PaperFormat(){Number=3,Width=324,Height=458},
                new PaperFormat(){Number=4,Width=229,Height=324},
                new PaperFormat(){Number=5,Width=162,Height=229},
                new PaperFormat(){Number=6,Width=114,Height=162},
                new PaperFormat(){Number=7,Width=81,Height=114},
                new PaperFormat(){Number=8,Width=57,Height=81},
                new PaperFormat(){Number=9,Width=40,Height=57},
                new PaperFormat(){Number=10,Width=28,Height=40},
                } }

            };

        public readonly TrimBoxResult Result = new TrimBoxResult();

        public FormGetTrimBox()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            kryptonComboBoxNumber.SelectedIndexChanged += KryptonComboBoxNumber_SelectedIndexChanged;
            kryptonComboBox_Serial.SelectedIndexChanged += KryptonComboBox_Serial_SelectedIndexChanged;
            kryptonComboBox_Serial.DataSource = PaperStandarts.ToArray();
            kryptonComboBox_Serial.DisplayMember = "Key";

            
        }

        private void KryptonComboBoxNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (PaperFormat) kryptonComboBoxNumber.SelectedItem;

            numericUpDownWidth.Value = item.Width;
            numericUpDownHeight.Value = item.Height;
        }

        private void KryptonComboBox_Serial_SelectedIndexChanged(object sender, EventArgs e)
        {
            kryptonComboBoxNumber.DataSource = ((KeyValuePair<string,List<PaperFormat>>) kryptonComboBox_Serial.SelectedItem).Value;
            kryptonComboBoxNumber.DisplayMember = "Number";
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

      
    }
}
