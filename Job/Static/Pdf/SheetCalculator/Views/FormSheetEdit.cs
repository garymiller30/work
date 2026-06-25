using System;
using System.Windows.Forms;
using System.Drawing;

namespace JobSpace.Static.Pdf.SheetCalculator.Views
{
    public class FormSheetEdit : Form
    {
        private TextBox txtName;
        private NumericUpDown numW;
        private NumericUpDown numH;
        private NumericUpDown numMl;
        private NumericUpDown numMr;
        private NumericUpDown numMt;
        private NumericUpDown numMb;
        private Button btnOk;
        private Button btnCancel;

        public string SheetName => txtName.Text;
        public double SheetWidth => (double)numW.Value;
        public double SheetHeight => (double)numH.Value;
        public double MarginLeft => (double)numMl.Value;
        public double MarginRight => (double)numMr.Value;
        public double MarginTop => (double)numMt.Value;
        public double MarginBottom => (double)numMb.Value;

        public FormSheetEdit(string title = "Лист", string name = "Лист SRA3", double w = 450, double h = 320, double ml = 10, double mr = 10, double mt = 10, double mb = 10)
        {
            Text = title;
            Size = new Size(300, 360);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(37, 37, 38);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9F);

            InitializeControls(name, w, h, ml, mr, mt, mb);
        }

        private void InitializeControls(string name, double w, double h, double ml, double mr, double mt, double mb)
        {
            var lblName = new Label { Text = "Назва:", Location = new Point(20, 20), Size = new Size(100, 20) };
            txtName = new TextBox { Text = name, Location = new Point(130, 18), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblW = new Label { Text = "Ширина (мм):", Location = new Point(20, 50), Size = new Size(100, 20) };
            numW = new NumericUpDown { Minimum = 10, Maximum = 10000, Value = (decimal)w, Location = new Point(130, 48), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblH = new Label { Text = "Висота (мм):", Location = new Point(20, 80), Size = new Size(100, 20) };
            numH = new NumericUpDown { Minimum = 10, Maximum = 10000, Value = (decimal)h, Location = new Point(130, 78), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblMl = new Label { Text = "Поле ліве (мм):", Location = new Point(20, 120), Size = new Size(100, 20) };
            numMl = new NumericUpDown { Minimum = 0, Maximum = 1000, Value = (decimal)ml, Location = new Point(130, 118), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblMr = new Label { Text = "Поле праве (мм):", Location = new Point(20, 150), Size = new Size(100, 20) };
            numMr = new NumericUpDown { Minimum = 0, Maximum = 1000, Value = (decimal)mr, Location = new Point(130, 148), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblMt = new Label { Text = "Поле верхнє (мм):", Location = new Point(20, 180), Size = new Size(100, 20) };
            numMt = new NumericUpDown { Minimum = 0, Maximum = 1000, Value = (decimal)mt, Location = new Point(130, 178), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            var lblMb = new Label { Text = "Поле нижнє (мм):", Location = new Point(20, 210), Size = new Size(100, 20) };
            numMb = new NumericUpDown { Minimum = 0, Maximum = 1000, Value = (decimal)mb, Location = new Point(130, 208), Size = new Size(130, 23), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };

            btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(70, 260),
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
                Location = new Point(170, 260),
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
                lblMl, numMl,
                lblMr, numMr,
                lblMt, numMt,
                lblMb, numMb,
                btnOk, btnCancel
            });

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
