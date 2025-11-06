using Amazon.Runtime.Internal.Util;
using Interfaces;
using Interfaces.PdfUtils;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualFalc : Form
    {
        Control[] _parts;
        NumericUpDown[] _deltas;
        Label[] _labels;
        IFileSystemInfoExt _fsi;

        public FormVisualFalc(IFileSystemInfoExt fsi)
        {
            _fsi = fsi;
            InitializeComponent();

            nud_width.Value = _fsi.Format.Width;
            _parts = new Control[] { gb_p1, gb_p2, gb_p3, gb_p4, gb_p5, gb_p6, gb_p7, gb_p8, gb_p9, gb_p10 };
            _deltas = new NumericUpDown[] {numericUpDown1, numericUpDown2, numericUpDown3, numericUpDown4, numericUpDown5,
                numericUpDown6, numericUpDown7, numericUpDown8, numericUpDown9, numericUpDown10 };
            _labels = new Label[] { label1, label2, label3, label4, label5,
                label6, label7, label8, label9, label10 };

            cb_cnt_falc.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 1; i < _parts.Length; i++)
            {
                _parts[i].Enabled = (i <= cb_cnt_falc.SelectedIndex + 1);
            }
            Recalc();
        }

        private void Recalc()
        {
            int cntPart = cb_cnt_falc.SelectedIndex + 2;
            decimal width = nud_width.Value;

            decimal[] partsDelta = new decimal[cntPart];
            decimal delta = 0;
            for (int i = 0; i < cntPart; i++)
            {
                delta+= _deltas[i].Value;
                partsDelta[i] = delta;
            }

            delta = partsDelta.Sum();
            decimal partWidth = (width + delta) / cntPart;
            for (int i = 0; i < cntPart; i++)
            {
                _labels[i].Text = Math.Round(partWidth - partsDelta[i], 2).ToString();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Recalc();
        }
    }
}
