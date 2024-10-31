using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class MarkColorControl : UserControl
    {
        TextMark _mark;



        public MarkColorControl()
        {
            InitializeComponent();

        }

        public void SetMark(TextMark mark)
        {
            _mark = mark;
            var c = _mark.Color;

            cb_isOverprint.Checked = c.IsOverprint;
            cb_isSpot.Checked = c.IsSpot;

            nud_c.Value = (decimal)c.C;
            nud_m.Value = (decimal)c.M;
            nud_y.Value = (decimal)c.Y;
            nud_k.Value = (decimal)c.K;

            tb_name.Text = c.Name;

            tb_name.DataBindings.Add("Enabled", cb_isSpot, "Checked");
            btn_selectPantone.DataBindings.Add("Enabled", cb_isSpot, "Checked");
        }

        public void UpdateMark()
        {
            var c = _mark.Color;
            c.IsOverprint = cb_isOverprint.Checked;
            c.IsSpot = cb_isSpot.Checked;
            c.Name = tb_name.Text;
            c.C = (double)nud_c.Value;
            c.M = (double)nud_m.Value;
            c.Y = (double)nud_y.Value;
            c.K = (double)nud_k.Value;

        }

        private void btn_selectPantone_Click(object sender, EventArgs e)
        {
            using (var form = new FormSelectSpotColor())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    tb_name.Text = form.SelectedSpotColor.Name;
                    var str = form.SelectedSpotColor.Lab.Split(' ');

                    var l = float.Parse(str[0], System.Globalization.CultureInfo.InvariantCulture);
                    var a = float.Parse(str[1], System.Globalization.CultureInfo.InvariantCulture);
                    var b = float.Parse(str[2], System.Globalization.CultureInfo.InvariantCulture);
                    var (C, M, Y, K) = LABtoCMYK(l, a, b);

                    nud_c.Value = (decimal)C;
                    nud_m.Value = (decimal)M;
                    nud_y.Value = (decimal)Y;
                    nud_k.Value = (decimal)K;
                }
            }
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

            return (ClampCmyk(_C*100), ClampCmyk(_M*100), ClampCmyk(_Y*100), ClampCmyk(_K * 100));

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

        private void nud_c_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0,( (NumericUpDown)sender).Text.Length);
        }
    }

}
