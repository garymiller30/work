using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UC
{


    public partial class UcSelectStandartPageFormat : UserControl
    {

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

        public event EventHandler<PaperFormat> PaperFormatChanged = delegate { };


        public UcSelectStandartPageFormat()
        {
            InitializeComponent();

            kryptonComboBoxNumber.SelectedIndexChanged += KryptonComboBoxNumber_SelectedIndexChanged;
            kryptonComboBox_Serial.SelectedIndexChanged += KryptonComboBox_Serial_SelectedIndexChanged;
            kryptonComboBox_Serial.DataSource = PaperStandarts.ToArray();
            kryptonComboBox_Serial.DisplayMember = "Key";
        }

        private void KryptonComboBox_Serial_SelectedIndexChanged(object sender, EventArgs e)
        {
            kryptonComboBoxNumber.DataSource = ((KeyValuePair<string, List<PaperFormat>>)kryptonComboBox_Serial.SelectedItem).Value;
            kryptonComboBoxNumber.DisplayMember = "Number";
        }

        private void KryptonComboBoxNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (PaperFormat)kryptonComboBoxNumber.SelectedItem;

            PaperFormatChanged?.Invoke(this, item);
            //numericUpDownWidth.Value = item.Width;
            //numericUpDownHeight.Value = item.Height;
        }
    }
}
