using Google.Apis.Gmail.v1.Data;
using JobSpace.Models;
using JobSpace.Static;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace JobSpace.UserForms.PDF
{
    public partial class Form_PdfColorSelector : Form
    {
        const string PANTONE_PATH = "db\\ColorTables";
        List<Models.ColorTable> pantone_tables = new List<Models.ColorTable>();
        public MarkColor MarkColor { get; internal set; }
        public Form_PdfColorSelector()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            LoadPantones();

        }

        void LoadPantones()
        {
            var files = Directory.EnumerateFiles(PANTONE_PATH, "*.json");

            foreach (var file in files)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var data = JsonSerializer.Deserialize<Models.PantoneTable>(json);
                    pantone_tables.Add(data.ColorTable);
                }
                catch
                {

                }
            }
            comboBoxListTables.DataSource = pantone_tables;
            comboBoxListTables.DisplayMember = "_Name";
        }

        private void Form_PdfColorSelector_Load(object sender, EventArgs e)
        {

        }

        private void bnt_OK_Click(object sender, EventArgs e)
        {
            MarkColor = new MarkColor();

            if (radioButtonCMYK.Checked)
            {
                MarkColor.C = (double)numC.Value;
                MarkColor.M = (double)numM.Value;
                MarkColor.Y = (double)numY.Value;
                MarkColor.K = (double)numK.Value;
            }
            else
            {
                var pantone = comboBoxListColor.SelectedItem as Models.Color;
                var table = comboBoxListTables.SelectedItem as Models.ColorTable;

                if (pantone == null) return;

                MarkColor.ColorType = ColorTypeEnum.LAB;
                MarkColor.IsSpot = true;

                string[] labstr = pantone._Lab.Split(' ');

                MarkColor.l = double.Parse(labstr[0], System.Globalization.CultureInfo.InvariantCulture);
                MarkColor.a = double.Parse(labstr[1], System.Globalization.CultureInfo.InvariantCulture);
                MarkColor.b = double.Parse(labstr[2], System.Globalization.CultureInfo.InvariantCulture);

                MarkColor.Name = $"{table._Prefix} {pantone._Name}";
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void comboBoxListTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            var table = pantone_tables[comboBoxListTables.SelectedIndex];

            comboBoxListColor.DataSource = table.Color;
            comboBoxListColor.DisplayMember = "_Name";
        }

        private void radioButtonCMYK_CheckedChanged(object sender, EventArgs e)
        {
            gb_cmyk.Enabled = radioButtonCMYK.Checked;
            gb_spot.Enabled = radioButtonSpot.Checked;
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

        private void numericUpDown1_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void comboBoxListColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pantone = comboBoxListColor.SelectedItem as Models.Color;

            if (pantone == null)
            {
                panel_spot.BackColor = default;
            }

            else
            {

                var str = pantone._Lab.Split(' ');

                var l = float.Parse(str[0], System.Globalization.CultureInfo.InvariantCulture);
                var a = float.Parse(str[1], System.Globalization.CultureInfo.InvariantCulture);
                var b = float.Parse(str[2], System.Globalization.CultureInfo.InvariantCulture);

                var rgb = ColorUtil.LabToRGB(l, a, b);

                panel_spot.BackColor = System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B);
            }
        }

        private void numC_ValueChanged(object sender, EventArgs e)
        {
            SetCMYKPanelColor();
        }

        private void SetCMYKPanelColor()
        {
            var color = ColorUtil.CMYKToRGB(numC.Value, numM.Value, numY.Value, numK.Value);
            panel_cmyk.BackColor = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
        }
    }
}
