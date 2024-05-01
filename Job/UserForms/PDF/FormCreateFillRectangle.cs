using Job.Fasades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Job.UserForms.PDF
{
    public partial class FormCreateFillRectangle : Form
    {
        const string PANTONE_PATH = "ColorTables";
        List<Models.ColorTable> pantone_tables = new List<Models.ColorTable>();


        public Models.PdfColorResult PdfColorResult { get; set; } = new Models.PdfColorResult();
        public decimal PdfWidth { get; set; }
        public decimal PdfHeight { get; set; }

        public FormCreateFillRectangle()
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

        private void comboBoxListTables_SelectedIndexChanged(object sender, EventArgs e)
        {


            var table = pantone_tables[comboBoxListTables.SelectedIndex];

            comboBoxListColor.DataSource = table.Color;
            comboBoxListColor.DisplayMember = "_Name";
        }

        private void radioButtonCMYK_CheckedChanged(object sender, EventArgs e)
        {
            EnableCMYK(radioButtonCMYK.Checked);
            EnableSpots(!radioButtonCMYK.Checked);
        }

        private void EnableCMYK(bool enable)
        {
            numC.Enabled = enable;
            numM.Enabled = enable;
            numY.Enabled = enable;
            numK.Enabled = enable;
        }
        private void EnableSpots(bool enable)
        {
            comboBoxListColor.Enabled = enable;
            comboBoxListTables.Enabled = enable;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            PdfColorResult.IsSpot = radioButtonSpot.Checked;

            if (radioButtonCMYK.Checked)
            {
                PdfColorResult.C = numC.Value;
                PdfColorResult.M = numM.Value;
                PdfColorResult.Y = numY.Value;
                PdfColorResult.K = numK.Value;
            }
            else
            {
                var pantone = comboBoxListColor.SelectedItem as Models.Color;
                var table = comboBoxListTables.SelectedItem as Models.ColorTable;

                if (pantone == null) return;

                PdfColorResult.Lab = pantone._Lab;
                PdfColorResult.Name = $"{table._Prefix} {pantone._Name}";
            }

            PdfWidth = numericUpDown1.Value;
            PdfHeight = numericUpDown2.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void comboBoxListColor_SelectedIndexChanged(object sender, EventArgs e)
        {

            var pantone = comboBoxListColor.SelectedItem as Models.Color;

            if (pantone == null)
            {
                panelColor.BackColor = default;
            }

            else
            {

                var str = pantone._Lab.Split(' ');

                var l = float.Parse(str[0], System.Globalization.CultureInfo.InvariantCulture);
                var a = float.Parse(str[1], System.Globalization.CultureInfo.InvariantCulture);
                var b = float.Parse(str[2], System.Globalization.CultureInfo.InvariantCulture);

                var rgb = LabToRGB(l, a, b);

                panelColor.BackColor = Color.FromArgb(rgb.R, rgb.G, rgb.B);
            }

        }

        (int R, int G, int B) LabToRGB(double L, double a, double b)
        {
            // First, convert Lab to XYZ
            double Y = (L + 16.0) / 116.0;
            double X = a / 500.0 + Y;
            double Z = Y - b / 200.0;

            X = 95.047 * ((X > 0.206897) ? X * X * X : (X - 16.0 / 116.0) / 7.787);
            Y = 100.000 * ((Y > 0.206897) ? Y * Y * Y : (Y - 16.0 / 116.0) / 7.787);
            Z = 108.883 * ((Z > 0.206897) ? Z * Z * Z : (Z - 16.0 / 116.0) / 7.787);

            // Then, convert XYZ to RGB
            X /= 100.0;
            Y /= 100.0;
            Z /= 100.0;

            double R = X * 3.2406 + Y * -1.5372 + Z * -0.4986;
            double G = X * -0.9689 + Y * 1.8758 + Z * 0.0415;
            double B = X * 0.0557 + Y * -0.2040 + Z * 1.0570;

            R = (R > 0.0031308) ? (1.055 * Math.Pow(R, 1.0 / 2.4) - 0.055) : 12.92 * R;
            G = (G > 0.0031308) ? (1.055 * Math.Pow(G, 1.0 / 2.4) - 0.055) : 12.92 * G;
            B = (B > 0.0031308) ? (1.055 * Math.Pow(B, 1.0 / 2.4) - 0.055) : 12.92 * B;

            R = Math.Max(0, Math.Min(255, R * 255.0));
            G = Math.Max(0, Math.Min(255, G * 255.0));
            B = Math.Max(0, Math.Min(255, B * 255.0));

            return ((int)R, (int)G, (int)B);
        }
    }
}
