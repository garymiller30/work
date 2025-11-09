using Amazon.Runtime.Internal.Util;
using Interfaces;
using Interfaces.PdfUtils;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Create.Falc;
using JobSpace.Static.Pdf.Imposition.Models;
using MongoDB.Bson.IO;
using PDFiumSharp;
using PDFiumSharp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualFalc : Form
    {
        float _zoomFactor = 1.0f;
        Control[] _parts;
        NumericUpDown[] _deltas;
        Label[] _labels;
        IFileSystemInfoExt _fsi;
        //bool _isShowing = false;
        private Pen _redLinePen = new Pen(Color.Red, 1);
        private Pen _whiteLinePen = new Pen(Color.White, 2);
        decimal[] partsDelta;
        Image document_prev;
        List<PdfPageInfo> boxes;

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
            pb_preview.Paint += Pb_preview_Paint;
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

            partsDelta = new decimal[cntPart];
            decimal delta = 0;
            for (int i = 0; i < cntPart; i++)
            {
                delta += _deltas[i].Value;
                partsDelta[i] = delta;
            }

            delta = partsDelta.Sum();
            decimal partWidth = (width + delta) / cntPart;
            for (int i = 0; i < cntPart; i++)
            {
                partsDelta[i] = Math.Round(partWidth - partsDelta[i], 2);
                _labels[i].Text = partsDelta[i].ToString();
            }

            pb_preview.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Recalc();
        }

        private void FormVisualFalc_Load(object sender, EventArgs e)
        {
            boxes = PdfHelper.GetPagesInfo(_fsi.FileInfo.FullName);
            nud_page_no.Maximum = boxes.Count;
            label_total_pages.Text = $"/ {boxes.Count}";
            GeneratePageImage(0);
        }

        private void GeneratePageImage(int pageIdx)
        {
            if (document_prev != null)
            {
                document_prev.Dispose();
            }
            Image pageImage = PdfHelper.RenderByTrimBox(_fsi, pageIdx, 150);
            document_prev = pageImage;
            pb_preview.Width = (int)(_fsi.Format.Width * pb_preview.DeviceDpi/ 25.4m)+1;
            pb_preview.Height = (int)(_fsi.Format.Height * pb_preview.DeviceDpi / 25.4m)+1;
        }

        private void Pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (partsDelta == null || e.Graphics == null || document_prev == null) return;

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            e.Graphics.ScaleTransform(_zoomFactor, _zoomFactor);
            e.Graphics.DrawImage(document_prev, 0, 0, (float)_fsi.Format.Width, (float)_fsi.Format.Height);
           
            float x = 0;

            if (cb_mirrored_parts.Checked && nud_page_no.Value % 2 == 0)
            {
                for (int i = 0; i < partsDelta.Length - 1; i++)
                {
                    x += (float)((double)partsDelta[i]);
                    g.DrawLine(_whiteLinePen, x, 0, x, (float)_fsi.Format.Height);
                    g.DrawLine(_redLinePen, x, 0, x, (float)_fsi.Format.Height);
                }
            }
            else
            {
                for (int i = partsDelta.Length - 1; i > 0; i--)
                {
                    x += (float)((double)partsDelta[i]);
                    g.DrawLine(_whiteLinePen, x, 0, x, (float)_fsi.Format.Height);
                    g.DrawLine(_redLinePen, x, 0, x, (float)_fsi.Format.Height);
                }

            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _zoomFactor = trackBar1.Value / 100.0f;
            pb_preview.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pb_preview.Invalidate();
        }

        private void nud_page_no_ValueChanged(object sender, EventArgs e)
        {
            GeneratePageImage((int)(nud_page_no.Value - 1));
            pb_preview.Invalidate();
        }

        private void btn_create_schema_Click(object sender, EventArgs e)
        {
            FalcSchemaParams param = new FalcSchemaParams()
            {
                Mirrored = cb_mirrored_parts.Checked,
                PartsWidth = partsDelta
            };

            new FalcSchema(param).Run(_fsi.FileInfo.FullName);
        }
    }
}
