using System;
using System.Windows.Forms;
using System.Drawing;

namespace JobSpace.Static.Pdf.SheetCalculator.Views
{
    public class FormProductEdit : Form
    {
        private TextBox txtName;
        private NumericUpDown numW;
        private NumericUpDown numH;
        private NumericUpDown numCirc;
        private NumericUpDown numTech;
        private Button btnOk;
        private Button btnCancel;

        public new string ProductName => txtName.Text;
        public double ProductWidth => (double)numW.Value;
        public double ProductHeight => (double)numH.Value;
        public int RequiredCirculation => (int)numCirc.Value;
        public double TechMargin => (double)numTech.Value;

        public FormProductEdit(string title = "Виріб", string name = "Візитка", double w = 90, double h = 50, int circ = 1000, double tech = 2)
        {
            Text = title;
            Size = new Size(300, 270);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(37, 37, 38);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9F);

            InitializeControls(name, w, h, circ, tech);
        }

        private void InitializeControls(string name, double w, double h, int circ, double tech)
        {
            var lblName = new Label { Text = "Назва:", Location = new Point(20, 20), Size = new Size(100, 20) };
            txtName = new TextBox { Text = name, Location = new Point(130, 18), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblW = new Label { Text = "Ширина (мм):", Location = new Point(20, 50), Size = new Size(100, 20) };
            numW = new NumericUpDown { Minimum = 1, Maximum = 10000, Value = (decimal)w, Location = new Point(130, 48), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblH = new Label { Text = "Висота (мм):", Location = new Point(20, 80), Size = new Size(100, 20) };
            numH = new NumericUpDown { Minimum = 1, Maximum = 10000, Value = (decimal)h, Location = new Point(130, 78), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblCirc = new Label { Text = "Потрібно (шт):", Location = new Point(20, 110), Size = new Size(100, 20) };
            numCirc = new NumericUpDown { Minimum = 1, Maximum = 10000000, Value = circ, Location = new Point(130, 108), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblTech = new Label { Text = "Техн. відступ (мм):", Location = new Point(20, 140), Size = new Size(100, 20) };
            numTech = new NumericUpDown { DecimalPlaces = 1, Minimum = 0, Maximum = 100, Value = (decimal)tech, Location = new Point(130, 138), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(70, 190),
                Size = new Size(90, 28),
                BackColor = Color.FromArgb(62, 62, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;

            btnCancel = new Button
            {
                Text = "Скасувати",
                DialogResult = DialogResult.Cancel,
                Location = new Point(170, 190),
                Size = new Size(90, 28),
                BackColor = Color.FromArgb(62, 62, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            Controls.AddRange(new Control[] {
                lblName, txtName,
                lblW, numW,
                lblH, numH,
                lblCirc, numCirc,
                lblTech, numTech,
                btnOk, btnCancel
            });

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
