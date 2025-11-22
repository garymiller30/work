using Interfaces;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Create.BigovkaMarks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF
{
    public partial class FormCreateBigovkaMarks : Form
    {
        float _zoomFactor = 1.0f;
        FileInfo _fsi;
        Bitmap _page_preview;
        List<PdfPageInfo> boxes_pages;
        Pen big_pen = new Pen(Color.Green, 0.6f);
        Pen white_pen = new Pen(Color.White, 1.0f);


        public CreateBigovkaMarksParams BigovkaMarksParams { get; set; } = new CreateBigovkaMarksParams();

        public FormCreateBigovkaMarks()
        {
            InitializeComponent();
            cb_files.DisplayMember = "Name";
            cb_mirrorEven.DataBindings.Add("Enabled", radioButtonHor, "Checked");

            InitUi();
            DialogResult = DialogResult.Cancel;
        }

        public FormCreateBigovkaMarks(IFileSystemInfoExt fsi) : this()
        {
            _fsi = new FileInfo( fsi.FileInfo.FullName);
            
            cb_files.Items.Add(_fsi);
            cb_files.SelectedIndex = 0;
            
            boxes_pages = PdfHelper.GetPagesInfo(_fsi.FullName);
            nud_page_number.Maximum = boxes_pages.Count;
            label_total_pages.Text = $"/{boxes_pages.Count}";
            GetPagePreview(0);
        }

        public FormCreateBigovkaMarks(List<IFileSystemInfoExt> infoExts): this()
        {
            foreach (var fsi in infoExts)
            {
                FileInfo fi = new FileInfo(fsi.FileInfo.FullName);
                cb_files.Items.Add(fi);
            }
            if (cb_files.Items.Count > 0)
            {
                cb_files.SelectedIndex = 0;
            }
        }

        private void GetPagePreview(int page_idx)
        {
            if (_page_preview != null) _page_preview.Dispose();

            _page_preview = PdfHelper.RenderByTrimBox(_fsi, page_idx);
            var box = boxes_pages[page_idx];

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

        private void cb_files_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_files.SelectedItem is FileInfo fsi)
            {
                _fsi = fsi;
                GetFilePreview();
                pb_preview.Invalidate();
            }
        }

        private void GetFilePreview()
        {
            boxes_pages = PdfHelper.GetPagesInfo(_fsi.FullName);
            label_total_pages.Text = $"/ {boxes_pages.Count}";
            nud_page_number.Maximum = boxes_pages.Count;
            GetPdfPagePreview((int)nud_page_number.Value);
        }

        private void GetPdfPagePreview(int page)
        {
            if (_page_preview != null) _page_preview.Dispose();
            _page_preview = PdfHelper.RenderByTrimBox(_fsi, page - 1);

            var box = boxes_pages[page - 1];

            UpdatePreviewLayout();
        }

        private bool UpdatePreviewLayout()
        {
            if (_page_preview == null) return false;

            var box = boxes_pages[(int)(nud_page_number.Value - 1)];

            float pageWmm = (float)box.Trimbox.wMM();
            float pageHmm = (float)box.Trimbox.hMM();

            float dpi = pb_preview.DeviceDpi;

            // розмір сторінки у пікселях
            float pageWpx = pageWmm * dpi / 25.4f;
            float pageHpx = pageHmm * dpi / 25.4f;

            if (cb_fit_to_panel.Checked)
            {
                float availW = pb_preview.Parent.Width;
                float availH = pb_preview.Parent.Height;

                // масштаб
                float scaleX = availW / pageWpx;
                float scaleY = availH / pageHpx;

                _zoomFactor = Math.Min(scaleX, scaleY);

                // нові розміри PictureBox
                int newW = (int)(pageWpx * _zoomFactor) - 1;
                int newH = (int)(pageHpx * _zoomFactor) - 1;

                pb_preview.Width = newW;
                pb_preview.Height = newH;

                //// центроване позиціонування
                //pb_preview.Left = (pb_preview.Parent.ClientSize.Width - newW) / 2;
                //pb_preview.Top = (pb_preview.Parent.ClientSize.Height - newH) / 2;
            }
            else
            {
                // масштаб 1:1
                _zoomFactor = 1.0f;

                int newW = (int)pageWpx;
                int newH = (int)pageHpx;

                pb_preview.Width = newW;
                pb_preview.Height = newH;

                pb_preview.Left = 0;
                pb_preview.Top = 0;
            }

            pb_preview.Invalidate();
            return true;
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            UpdatePreviewLayout();
        }
    }
}
