using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static;
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
        FileInfo _fsi;
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
            uc_PreviewBrowserFile1.OnPageChanged += (s, e) => { Draw(); };
            DialogResult = DialogResult.Cancel;
        }

        public FormCreateBigovkaMarks(IFileSystemInfoExt fsi) : this()
        {
            _fsi = new FileInfo( fsi.FileInfo.FullName);
            
            cb_files.Items.Add(_fsi);
            cb_files.SelectedIndex = 0;
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

        void Draw()
        {

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

            uc_PreviewBrowserFile1.SetPrimitives( _primitives);
        }

        private void DrawVerticalBigovki()
        {
            
            var box = uc_PreviewBrowserFile1.GetCurrentPageInfo();

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
            var page_idx = uc_PreviewBrowserFile1.GetCurrentPageIdx();
            var box = uc_PreviewBrowserFile1.GetCurrentPageInfo();

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
                uc_PreviewBrowserFile1.Show(fsi.ToFileSystemInfoExt());
                Draw();
            }
        }


        private void btn_add_to_center_Click(object sender, EventArgs e)
        {
            // отримати поточну сторінку, дізнатися ширину сторінки і додати в текстбокс біговки половину ширини
            var box = uc_PreviewBrowserFile1.GetCurrentPageInfo();
            double half = 0;
            if (radioButtonHor.Checked)
            {
                half = box.Trimbox.wMM() / 2.0;
            }
            else
            {
                half = box.Trimbox.hMM() / 2.0;
            }
            textBoxBigovky.Text = half.ToString("F1", CultureInfo.InvariantCulture);
        }
    }
}
