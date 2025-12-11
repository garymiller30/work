using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Create.BigovkaMarks;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();

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

            Draw();
        }

        private void InitUi()
        {
            cb_mirrorEven.Checked = BigovkaMarksParams.MirrorEven;
            uc_PreviewControl1.FitToScreen = cb_fit_to_panel.Checked;
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

        void Draw()
        {
            if (_page_preview == null) return;
           
            var box = boxes_pages[(int)(nud_page_number.Value - 1)];

            uc_PreviewControl1.SetImage(_page_preview, (float)box.Trimbox.wMM(), (float)box.Trimbox.hMM());

            DrawBigovki();
        }


        private void DrawBigovki()
        {
            _primitives.Clear();

            if (CreateParameters() == false) return;

            switch (BigovkaMarksParams.Direction)
            {
                case Static.Pdf.Common.DirectionEnum.Horizontal:
                    DrawHorizontalBigovki();
                    break;
                case Static.Pdf.Common.DirectionEnum.Vertical:
                    DrawVerticalBigovki();
                    break;
                default:
                    break;
            }

            uc_PreviewControl1.Primitives = _primitives;
        }

        private void DrawVerticalBigovki()
        {
            var page_idx = (int)(nud_page_number.Value - 1);
            var box = boxes_pages[page_idx];

            float x_start = 0;
            float x_end = (float)box.Trimbox.wMM();
            float y_start = (float)box.Trimbox.hMM();

            for (int i = 0; i < BigovkaMarksParams.Bigovki.Length; i++)
            {
                y_start -= (float)BigovkaMarksParams.Bigovki[i];
                _primitives.Add(new ScreenLine(white_pen, x_start, y_start, x_end, y_start));
                _primitives.Add(new ScreenLine(big_pen, x_start, y_start, x_end, y_start));
            }
        }

        private void DrawHorizontalBigovki()
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

                    _primitives.Add(new ScreenLine(white_pen, (float)box.Trimbox.wMM() - x, y_start, (float)box.Trimbox.wMM() - x, y_end));
                    _primitives.Add(new ScreenLine(big_pen, (float)box.Trimbox.wMM() - x, y_start, (float)box.Trimbox.wMM() - x, y_end));
                }
            }
            else
            {
                for (int i = 0; i < BigovkaMarksParams.Bigovki.Length; i++)
                {
                    x += (float)BigovkaMarksParams.Bigovki[i];
                    _primitives.Add(new ScreenLine(white_pen, x, y_start, x, y_end));
                    _primitives.Add(new ScreenLine(big_pen, x, y_start, x, y_end));
                }
            }
        }

        private void nud_page_number_ValueChanged(object sender, EventArgs e)
        {
            GetPagePreview((int)(nud_page_number.Value - 1));
        }

        private void textBoxBigovky_TextChanged(object sender, EventArgs e)
        {
            if (CreateParameters() == true) Draw();
        }

        private void radioButtonHor_Click(object sender, EventArgs e)
        {
            Draw();
        }

        private void cb_mirrorEven_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void cb_files_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_files.SelectedItem is FileInfo fsi)
            {
                _fsi = fsi;
                GetFilePreview();
                Draw();
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
        }

        private void cb_fit_to_panel_CheckedChanged(object sender, EventArgs e)
        {
            uc_PreviewControl1.FitToScreen = cb_fit_to_panel.Checked;
            if (!cb_fit_to_panel.Checked)
            {
                uc_PreviewControl1.SetZoomFactor(1.0f);
            }
        }
    }
}
