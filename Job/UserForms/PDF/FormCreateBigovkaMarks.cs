using Interfaces;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Create.BigovkaMarks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF
{
    public partial class FormCreateBigovkaMarks : Form
    {
        float _zoomFactor = 1.0f;
        IFileSystemInfoExt _fsi;
        Bitmap _page_preview;
        List<PdfPageInfo> boxes_pages;
        Pen big_pen = new Pen(Color.Green, 0.6f);
        Pen white_pen = new Pen(Color.White, 1.0f);


        public CreateBigovkaMarksParams BigovkaMarksParams { get; set; } = new CreateBigovkaMarksParams();

        public FormCreateBigovkaMarks()
        {
            InitializeComponent();

            cb_mirrorEven.DataBindings.Add("Enabled", radioButtonHor, "Checked");

            InitUi();
            DialogResult = DialogResult.Cancel;
        }

        public FormCreateBigovkaMarks(IFileSystemInfoExt fsi) : this()
        {
            _fsi = fsi;
            boxes_pages = PdfHelper.GetPagesInfo(_fsi.FileInfo.FullName);
            nud_page_number.Maximum = boxes_pages.Count;
            label_total_pages.Text = $"/{boxes_pages.Count}";
            GetPagePreview(0);
        }

        private void GetPagePreview(int page_idx)
        {
            if (_page_preview != null) _page_preview.Dispose();

            _page_preview = PdfHelper.RenderByTrimBox(_fsi, page_idx);
            var box = boxes_pages[page_idx];
            pb_preview.Width = (int)(box.Trimbox.wMM() * pb_preview.DeviceDpi / 25.4d) + 1;
            pb_preview.Height = (int)(box.Trimbox.hMM() * pb_preview.DeviceDpi / 25.4d) + 1;

            pb_preview.Invalidate();
        }

        private void InitUi()
        {
            cb_mirrorEven.Checked = BigovkaMarksParams.MirrorEven;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {

            if (CreateParameters())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else { MessageBox.Show("Перевір біговки"); return; }

        }

        private bool CreateParameters()
        {
            BigovkaMarksParams.Direction = radioButtonHor.Checked ? Static.Pdf.Common.DirectionEnum.Horizontal : Static.Pdf.Common.DirectionEnum.Vertical;
            BigovkaMarksParams.Bleed = (double)numBleed.Value;
            BigovkaMarksParams.Length = (double)numLen.Value;
            BigovkaMarksParams.DistanceFromTrim = (double)numDistanse.Value;
            BigovkaMarksParams.Color.C = (double)numC.Value;
            BigovkaMarksParams.Color.M = (double)numM.Value;
            BigovkaMarksParams.Color.Y = (double)numY.Value;
            BigovkaMarksParams.Color.K = (double)numK.Value;
            BigovkaMarksParams.MirrorEven = cb_mirrorEven.Checked;

            string[] bigovki = textBoxBigovky.Text.Trim(' ').Split(' ');

            BigovkaMarksParams.Bigovki = new double[bigovki.Length];

            for (int i = 0; i < bigovki.Length; i++)
            {
                if (!double.TryParse(bigovki[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
                {
                    return false;
                }
                else
                {
                    BigovkaMarksParams.Bigovki[i] = result;
                }
            }
            return true;
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

        private void numDistanse_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (_page_preview == null) return;
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            e.Graphics.ScaleTransform(_zoomFactor, _zoomFactor);

            int page_idx = (int)(nud_page_number.Value - 1);
            var box = boxes_pages[page_idx];

            e.Graphics.DrawImage(_page_preview, 0, 0, (float)box.Trimbox.wMM(), (float)box.Trimbox.hMM());

            DrawBigovki(e.Graphics);
        }

        private void DrawBigovki(Graphics g)
        {
            if (CreateParameters() == false) return;

            switch (BigovkaMarksParams.Direction)
            {
                case Static.Pdf.Common.DirectionEnum.Horizontal:
                    DrawHorizontalBigovki(g);
                    break;
                case Static.Pdf.Common.DirectionEnum.Vertical:
                    DrawVerticalBigovki(g);
                    break;
                default:
                    break;
            }
        }

        private void DrawVerticalBigovki(Graphics g)
        {
            var page_idx = (int)(nud_page_number.Value - 1);
            var box = boxes_pages[page_idx];

            float x_start = 0;
            float x_end = (float)box.Trimbox.wMM();
            float y_start = (float)box.Trimbox.hMM();

            for (int i = 0; i < BigovkaMarksParams.Bigovki.Length; i++)
            {
                y_start -= (float)BigovkaMarksParams.Bigovki[i];
                g.DrawLine(white_pen, x_start, y_start, x_end, y_start);
                g.DrawLine(big_pen, x_start, y_start, x_end, y_start);
            }
        }

        private void DrawHorizontalBigovki(Graphics g)
        {
            var page_idx = (int)(nud_page_number.Value - 1);
            var box = boxes_pages[page_idx];

            bool isEven = (page_idx + 1) % 2 == 0;
            float x = 0;
            float y_start = 0;
            float y_end = (float)box.Trimbox.hMM();

            if (BigovkaMarksParams.MirrorEven && page_idx %2 == 1)
            {
                for (int i = BigovkaMarksParams.Bigovki.Length -1; i >=0 ; i--)
                {
                    x += (float)BigovkaMarksParams.Bigovki[i];
                    g.DrawLine(white_pen, (float)box.Trimbox.wMM() - x, y_start, (float)box.Trimbox.wMM() - x, y_end);
                    g.DrawLine(big_pen, (float)box.Trimbox.wMM() - x, y_start, (float)box.Trimbox.wMM() - x, y_end);
                }
            }
            else
            {
                for (int i = 0; i < BigovkaMarksParams.Bigovki.Length; i++)
                {
                    x += (float)BigovkaMarksParams.Bigovki[i];
                    g.DrawLine(white_pen, x, y_start, x, y_end);
                    g.DrawLine(big_pen, x, y_start, x, y_end);
                }
            }
        }

        private void nud_page_number_ValueChanged(object sender, EventArgs e)
        {
            GetPagePreview((int)(nud_page_number.Value - 1));
            pb_preview.Invalidate();
        }

        private void textBoxBigovky_TextChanged(object sender, EventArgs e)
        {
            if (CreateParameters() == true) pb_preview.Invalidate();
        }

        private void radioButtonHor_Click(object sender, EventArgs e)
        {
            pb_preview.Invalidate();
        }

        private void cb_mirrorEven_CheckedChanged(object sender, EventArgs e)
        {
            pb_preview.Invalidate();
        }
    }
}
