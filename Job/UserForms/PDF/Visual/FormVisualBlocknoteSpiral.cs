using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualBlocknoteSpiral : Form
    {
        public SpiralSettings SpiralSettings { get; set; } = new SpiralSettings();
        FileInfo _fsi;

        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();

        public FormVisualBlocknoteSpiral()
        {
            InitializeComponent();
            SetDefaults();
            cb_files.DisplayMember = "Name";

            uc_PreviewBrowserFile1.OnPageChanged += (s, pageIdx) =>
            {
                Redraw();
            };

            uc_SelectSpiralControl1.OnSpiralChanged += OnSpiralChanged;
            DialogResult = DialogResult.Cancel;
        }

        private void OnSpiralChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        public FormVisualBlocknoteSpiral(IFileSystemInfoExt fsi) : this()
        {
            _fsi = new FileInfo( fsi.FileInfo.FullName);
            cb_files.Items.Add(_fsi);
            cb_files.SelectedIndex = 0;
        }

        public FormVisualBlocknoteSpiral(List<IFileSystemInfoExt> fsis):this()
        {
            foreach (var fsi in fsis)
            {
                FileInfo fi = new FileInfo( fsi.FileInfo.FullName);
                cb_files.Items.Add(fi);
            }
            if (cb_files.Items.Count > 0)
            {
                cb_files.SelectedIndex = 0;
            }
        }

       

        private void SetDefaults()
        {
            // використовуємо DescriptionAttribute для відображення назв елементів enum у комбобоксі

            var placeValues = Enum.GetValues(typeof(SpiralPlaceEnum)).Cast<SpiralPlaceEnum>();
            foreach (var val in placeValues)
            {
                var memInfo = typeof(SpiralPlaceEnum).GetMember(val.ToString());
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                string description = descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : val.ToString();
                cb_place.Items.Add(description);
            }
            cb_place.SelectedIndex = 0;

            
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            SpiralSettings.SpiralPlace = (SpiralPlaceEnum)cb_place.SelectedIndex;
            SpiralSettings.SpiralFile = uc_SelectSpiralControl1.GetSelectedSpiralFilePath();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DrawRectangle()
        {
            if (!cb_rect.Checked) return;
            float rectX = (float)nud_rect_x.Value;
            float rectY = (float)nud_rect_y.Value;
            float rectW = (float)nud_rect_w.Value;
            float rectH = (float)nud_rect_h.Value;

            _primitives.Add( new ScreenFillRectangle( new SolidBrush( System.Drawing.Color.FromArgb(64,255,0,0)), rectX, rectY, rectW, rectH));

        }

        private void DrawSpiral()
        {

            var spiralPreview = uc_SelectSpiralControl1.GetSpiralBitmap();
            if (spiralPreview == null) return;

            PdfPageInfo pageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();
            if (pageInfo == null) return;

            var spiralPdfInfo = uc_SelectSpiralControl1.GetSpiralPdfInfo();

            SpiralPlaceEnum place = (SpiralPlaceEnum)cb_place.SelectedIndex;
            int curPageIdx = uc_PreviewBrowserFile1.GetCurrentPageIdx();

            _primitives.AddRange(Static.Pdf.Visual.Commons.Screen.DrawSpiral.Draw(spiralPreview, spiralPdfInfo, place,pageInfo,curPageIdx));
        }

        void Redraw()
        {
            
            _primitives = new List<IScreenPrimitive>();
            DrawSpiral();
            DrawRectangle();
            uc_PreviewBrowserFile1.SetPrimitives(_primitives);
        }

        private void cb_place_SelectedIndexChanged(object sender, EventArgs e)
        {
            Redraw();
        }


        private void cb_rect_CheckedChanged(object sender, EventArgs e)
        {
            panel_rect_params.Enabled = cb_rect.Checked;
            Redraw();
        }

        private void cb_files_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cb_files.SelectedItem is FileInfo fsi)
            {
                _fsi = fsi;
               uc_PreviewBrowserFile1.Show(fsi.ToFileSystemInfoExt());
               Redraw();
            }
        }

        private void bnt_top_left_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = 0;
            nud_rect_y.Value = 0;
            Redraw();
        }

        private void btn_top_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)(((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.wMM() - nud_rect_w.Value)/2);
            nud_rect_y.Value = 0;
            Redraw();
        }

        private void bnt_top_right_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.wMM() - nud_rect_w.Value);
            nud_rect_y.Value = 0;
            Redraw();
        }

        private void btn_left_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = 0;
            nud_rect_y.Value = (decimal)(((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.hMM() - nud_rect_h.Value) / 2);
            Redraw();
        }

        private void btn_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)(((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.wMM() - nud_rect_w.Value) / 2);
            nud_rect_y.Value = (decimal)(((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.hMM() - nud_rect_h.Value) / 2);
            Redraw();
        }

        private void btn_right_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.wMM() - nud_rect_w.Value);
            nud_rect_y.Value = (decimal)(((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.hMM() - nud_rect_h.Value) / 2);
            Redraw();
        }

        private void bnt_bottom_left_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = 0;
            nud_rect_y.Value = (decimal)((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.hMM() - nud_rect_h.Value);
            Redraw();
        }

        private void btn_bottom_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)(((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.wMM() - nud_rect_w.Value) / 2);
            nud_rect_y.Value = (decimal)((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.hMM() - nud_rect_h.Value);
            Redraw();
        }

        private void btn_bottom_right_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.wMM() - nud_rect_w.Value);
            nud_rect_y.Value = (decimal)((decimal)uc_PreviewBrowserFile1.GetCurrentPageInfo().Trimbox.hMM() - nud_rect_h.Value);
            Redraw();
        }
    }
}
