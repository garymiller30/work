using JobSpace.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormSelectSpotColor : Form
    {
        const string PANTONE_PATH = "ColorTables";
        public PdfColorResult SelectedSpotColor { get;set;} = new PdfColorResult();
        List<Models.ColorTable> pantone_tables = new List<Models.ColorTable>();
        public FormSelectSpotColor()
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

                panelColor.BackColor = System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B);
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

        double ClampCmyk(double value)
        {
            if (value < 0 || double.IsNaN(value))
            {
                value = 0;
            }
            if (value > 100)
            {
                value = 100;
            }

            return value;
        }

        (double C, double M, double Y, double K) LABtoCMYK(double l, double a, double b)
        {
            // Conversion from LAB to XYZ
            double Y = (l + 16) / 116;
            double X = a / 500 + Y;
            double Z = Y - b / 200;

            X = 0.95047 * ((X * X * X > 0.008856) ? X * X * X : (X - 16 / 116) / 7.787);
            Y = 1.00000 * ((Y * Y * Y > 0.008856) ? Y * Y * Y : (Y - 16 / 116) / 7.787);
            Z = 1.08883 * ((Z * Z * Z > 0.008856) ? Z * Z * Z : (Z - 16 / 116) / 7.787);

            // Conversion from XYZ to RGB
            double R = X * 3.2406 + Y * -1.5372 + Z * -0.4986;
            double G = X * -0.9689 + Y * 1.8758 + Z * 0.0415;
            double B = X * 0.0557 + Y * -0.2040 + Z * 1.0570;

            R = (R > 0.0031308) ? 1.055 * Math.Pow(R, 1 / 2.4) - 0.055 : 12.92 * R;
            G = (G > 0.0031308) ? 1.055 * Math.Pow(G, 1 / 2.4) - 0.055 : 12.92 * G;
            B = (B > 0.0031308) ? 1.055 * Math.Pow(B, 1 / 2.4) - 0.055 : 12.92 * B;

            double _K = 1 - Math.Max(R, Math.Max(G, B));
            double _C = (1 - R - _K) / (1 - _K);
            double _M = (1 - G - _K) / (1 - _K);
            double _Y = (1 - B - _K) / (1 - _K);

            return (ClampCmyk(_C * 100), ClampCmyk(_M * 100), ClampCmyk(_Y * 100), ClampCmyk(_K * 100));

        }


        private void button1_Click(object sender, EventArgs e)
        {
            var pantone = comboBoxListColor.SelectedItem as Models.Color;
            var table = comboBoxListTables.SelectedItem as Models.ColorTable;
            SelectedSpotColor.Lab = pantone._Lab;
            SelectedSpotColor.Name = $"{table._Prefix} {pantone._Name}";

            var str = SelectedSpotColor.Lab.Split(' ');

            var l = float.Parse(str[0], System.Globalization.CultureInfo.InvariantCulture);
            var a = float.Parse(str[1], System.Globalization.CultureInfo.InvariantCulture);
            var b = float.Parse(str[2], System.Globalization.CultureInfo.InvariantCulture);
            var (C, M, Y, K) = LABtoCMYK(l, a, b);

            SelectedSpotColor.C = (decimal)C;
            SelectedSpotColor.M = (decimal)M;
            SelectedSpotColor.Y = (decimal)Y;
            SelectedSpotColor.K = (decimal)K;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
