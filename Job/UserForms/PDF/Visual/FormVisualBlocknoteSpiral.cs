using ImageMagick.Drawing;
using Interfaces;
using JobSpace.Models;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualBlocknoteSpiral : Form
    {
        public SpiralSettings SpiralSettings { get; set; } = new SpiralSettings();
        FileInfo _fsi;
        Bitmap _page_preview;
        Bitmap _spiralPreview;
        List<PdfPageInfo> boxes_pages;
        PdfPageInfo _spiralBox;
        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();


        public FormVisualBlocknoteSpiral()
        {
            InitializeComponent();
            SetDefaults();
            cb_files.DisplayMember = "Name";
            DialogResult = DialogResult.Cancel;
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

        private void GetFilePreview()
        {
            boxes_pages = PdfHelper.GetPagesInfo(_fsi.FullName);
            label_total_pages.Text = $"/ {boxes_pages.Count}";
            nud_page_number.Maximum = boxes_pages.Count;
            GetPdfPagePreview((int)nud_page_number.Value);
        }

        private void GetSpiralPreview()
        {
            if (cb_spiral_files.SelectedItem is FileInfo fi)
            {
                _spiralBox = PdfHelper.GetPageInfo(fi.FullName);
                if (_spiralPreview != null) _spiralPreview.Dispose();
                _spiralPreview = PdfHelper.RenderByTrimBox(fi, 0);
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

            string spiralFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", "spirals");

            var files = Directory.GetFiles(spiralFolderPath, "*.pdf");

            cb_spiral_files.DisplayMember = "Name";

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);

                cb_spiral_files.Items.Add(fi);
            }
            if (cb_spiral_files.Items.Count > 0)
            {
                cb_spiral_files.SelectedIndex = 0;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            SpiralSettings.SpiralPlace = (SpiralPlaceEnum)cb_place.SelectedIndex;
            SpiralSettings.SpiralFile = (cb_spiral_files.SelectedItem as FileInfo).FullName;
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
            if (_spiralPreview == null) return;

            PdfPageInfo pageInfo = boxes_pages[(int)(nud_page_number.Value - 1)];
            SpiralPlaceEnum place = (SpiralPlaceEnum)cb_place.SelectedIndex;
            int curPageIdx = (int)(nud_page_number.Value - 1);

            switch (place)
            {
                case SpiralPlaceEnum.top:
                    DrawSpiralTop(pageInfo);
                    break;
                case SpiralPlaceEnum.bottom:
                    DrawSpiralBottom(pageInfo);
                    break;
                case SpiralPlaceEnum.left:
                    DrawSpiralLeft(pageInfo);
                    break;
                case SpiralPlaceEnum.right:
                    DrawSpiralRight(pageInfo);
                    break;
                case SpiralPlaceEnum.spread_horizontal:
                    if (curPageIdx % 2 == 0)
                        DrawSpiralLeft(pageInfo);
                    else
                        DrawSpiralRight(pageInfo);
                    break;
                case SpiralPlaceEnum.spread_vertical:
                    if (curPageIdx % 2 == 0)
                        DrawSpiralTop(pageInfo);
                    else
                        DrawSpiralBottom( pageInfo);
                    break;
                case SpiralPlaceEnum.top_bottom:
                    DrawSpiralTop(pageInfo);
                    DrawSpiralBottom( pageInfo);
                    break;

                case SpiralPlaceEnum.left_right:
                    DrawSpiralLeft(pageInfo);
                    DrawSpiralRight(pageInfo);
                    break;
                default:
                    break;
            }
        }

        private void DrawSpiralRight( PdfPageInfo pi)
        {
            double spiralWidth = _spiralBox.Trimbox.wMM();
            double spiralHeight = _spiralBox.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.hMM() / spiralWidth);
            double x = pi.Trimbox.wMM() - spiralHeight;
            double y = (pi.Trimbox.hMM() - (spiralWidth * cntHoles)) / 2;

            // розвернути _spiralPreview на 90 градусів
            Bitmap rotatedSpiral = new Bitmap(_spiralPreview);
            rotatedSpiral.RotateFlip(RotateFlipType.Rotate90FlipNone);
           
            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralWidth;
                _primitives.Add(new ScreenImage(rotatedSpiral, (float)holeX, (float)holeY, (float)spiralHeight, (float)spiralWidth));
            }

            rotatedSpiral.Dispose();
        }

        private void DrawSpiralLeft( PdfPageInfo pi)
        {
            double spiralWidth = _spiralBox.Trimbox.wMM();
            double spiralHeight = _spiralBox.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.hMM() / spiralWidth);
            double x = 0;
            double y = (pi.Trimbox.hMM() - (spiralWidth * cntHoles)) / 2;

            // розвернути _spiralPreview на -90 градусів
            Bitmap rotatedSpiral = new Bitmap(_spiralPreview);
            rotatedSpiral.RotateFlip(RotateFlipType.Rotate270FlipNone);

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralWidth;

                _primitives.Add(new ScreenImage(rotatedSpiral, (float)holeX, (float)holeY, (float)spiralHeight, (float)spiralWidth));
            }

            rotatedSpiral.Dispose();
        }

        private void DrawSpiralBottom(PdfPageInfo pi)
        {
            double spiralWidth = _spiralBox.Trimbox.wMM();
            double spiralHeight = _spiralBox.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.wMM() / spiralWidth);
            double x = (pi.Trimbox.wMM() - (spiralWidth * cntHoles)) / 2;
            double y = pi.Trimbox.hMM() - spiralHeight;

            // розвернути _spiralPreview на 180 градусів
            Bitmap rotatedSpiral = new Bitmap(_spiralPreview);
            rotatedSpiral.RotateFlip(RotateFlipType.Rotate180FlipNone);

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x + i * spiralWidth;
                double holeY = y;

                _primitives.Add(new ScreenImage(rotatedSpiral, (float)holeX, (float)holeY, (float)spiralWidth, (float)spiralHeight));
            }

            rotatedSpiral.Dispose();

        }

        private void DrawSpiralTop(PdfPageInfo pi)
        {
            double spiralWidth = _spiralBox.Trimbox.wMM();
            double spiralHeight = _spiralBox.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.wMM() / spiralWidth);
            double x = (pi.Trimbox.wMM() - (spiralWidth * cntHoles)) / 2;
            double y = 0;

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x + i * spiralWidth;
                double holeY = y;

                _primitives.Add(new ScreenImage(_spiralPreview, (float)holeX, (float)holeY, (float)spiralWidth, (float)spiralHeight));
            }
        }

        private void nud_page_number_ValueChanged(object sender, EventArgs e)
        {
            GetPdfPagePreview((int)nud_page_number.Value);
        }

        private void GetPdfPagePreview(int page)
        {
            if (_page_preview != null) _page_preview.Dispose();
            _page_preview = PdfHelper.RenderByTrimBox(_fsi, page - 1);

            var box = boxes_pages[page - 1];

            Redraw();
        }

        void Redraw()
        {
            if (boxes_pages == null) return;
            var box = boxes_pages[(int)nud_page_number.Value - 1];
            uc_PreviewControl1.SetImage(_page_preview, (float)box.Trimbox.wMM(), (float)box.Trimbox.hMM());
            _primitives = new List<IScreenPrimitive>();
            DrawSpiral();
            DrawRectangle();
            uc_PreviewControl1.Primitives = _primitives;
        }

        private void cb_place_SelectedIndexChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void cb_files_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSpiralPreview();
            Redraw();
        }

        private void cb_rect_CheckedChanged(object sender, EventArgs e)
        {
            panel_rect_params.Enabled = cb_rect.Checked;
            Redraw();
        }

        private void cb_fit_to_panel_CheckedChanged(object sender, EventArgs e)
        {
            uc_PreviewControl1.FitToScreen = cb_fit_to_panel.Checked;
        }

        private void cb_files_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cb_files.SelectedItem is FileInfo fsi)
            {
                _fsi = fsi;
                GetFilePreview();
                
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
            nud_rect_x.Value = (decimal)(((decimal)boxes_pages[(int)(nud_page_number.Value-1)].Trimbox.wMM() - nud_rect_w.Value)/2);
            nud_rect_y.Value = 0;
            Redraw();
        }

        private void bnt_top_right_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.wMM() - nud_rect_w.Value);
            nud_rect_y.Value = 0;
            Redraw();
        }

        private void btn_left_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = 0;
            nud_rect_y.Value = (decimal)(((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.hMM() - nud_rect_h.Value) / 2);
            Redraw();
        }

        private void btn_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)(((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.wMM() - nud_rect_w.Value) / 2);
            nud_rect_y.Value = (decimal)(((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.hMM() - nud_rect_h.Value) / 2);
            Redraw();
        }

        private void btn_right_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.wMM() - nud_rect_w.Value);
            nud_rect_y.Value = (decimal)(((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.hMM() - nud_rect_h.Value) / 2);
            Redraw();
        }

        private void bnt_bottom_left_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = 0;
            nud_rect_y.Value = (decimal)((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.hMM() - nud_rect_h.Value);
            Redraw();
        }

        private void btn_bottom_center_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)(((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.wMM() - nud_rect_w.Value) / 2);
            nud_rect_y.Value = (decimal)((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.hMM() - nud_rect_h.Value);
            Redraw();
        }

        private void btn_bottom_right_Click(object sender, EventArgs e)
        {
            nud_rect_x.Value = (decimal)((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.wMM() - nud_rect_w.Value);
            nud_rect_y.Value = (decimal)((decimal)boxes_pages[(int)(nud_page_number.Value - 1)].Trimbox.hMM() - nud_rect_h.Value);
            Redraw();
        }
    }
}
