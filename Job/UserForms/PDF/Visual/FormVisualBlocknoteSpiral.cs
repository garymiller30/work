using ImageMagick.Drawing;
using Interfaces;
using JobSpace.Models;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualBlocknoteSpiral : Form
    {
        public SpiralSettings SpiralSettings { get; set; } = new SpiralSettings();
        float _zoomFactor = 1.0f;
        IFileSystemInfoExt _fsi;
        Bitmap _page_preview;
        Bitmap _spiralPreview;
        List<PdfPageInfo> boxes_pages;
        PdfPageInfo _spiralBox;
        

        public FormVisualBlocknoteSpiral()
        {
            InitializeComponent();
            SetDefaults();
            DialogResult = DialogResult.Cancel;
        }

        public FormVisualBlocknoteSpiral(IFileSystemInfoExt fsi) : this()
        {
            _fsi = fsi;
            boxes_pages = PdfHelper.GetPagesInfo(_fsi.FileInfo.FullName);
            label_total_pages.Text = $"/ {boxes_pages.Count}";
            nud_page_number.Maximum = boxes_pages.Count;
            GetPdfPagePreview((int)nud_page_number.Value);
            GetSpiralPreview();
        }

        private void GetSpiralPreview()
        {
            if (cb_files.SelectedItem is FileInfo fi)
            {
                _spiralBox = PdfHelper.GetPageInfo(fi.FullName);
                if (_spiralPreview != null) _spiralPreview.Dispose();
                _spiralPreview = PdfHelper.RenderByTrimBox(new FileSystemInfoExt(fi), 0);
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

            cb_files.DisplayMember = "Name";

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);

                cb_files.Items.Add(fi);
            }
            if (cb_files.Items.Count > 0)
            {
                cb_files.SelectedIndex = 0;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            SpiralSettings.SpiralPlace = (SpiralPlaceEnum)cb_place.SelectedIndex;
            SpiralSettings.SpiralFile = (cb_files.SelectedItem as FileInfo).FullName;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (_page_preview == null || boxes_pages == null) return;

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            g.ScaleTransform(_zoomFactor, _zoomFactor);

            PdfPageInfo box = boxes_pages[(int)(nud_page_number.Value - 1)];

            g.DrawImage(_page_preview, 0, 0, (float)box.Trimbox.wMM(), (float)box.Trimbox.hMM());

            DrawSpiral(g);
            DrawRectangle(g);
        }

        private void DrawRectangle(Graphics g)
        {
            if (!cb_rect.Checked) return;
            float rectX = (float)nud_rect_x.Value;
            float rectY = (float)nud_rect_y.Value;
            float rectW = (float)nud_rect_w.Value;
            float rectH = (float)nud_rect_h.Value;
            using (Brush b = new SolidBrush(System.Drawing.Color.FromArgb(64, 255, 0, 0)))
            {
                g.FillRectangle(b, rectX, rectY, rectW, rectH);
            }
            

        }

        private void DrawSpiral(Graphics g)
        {
            if (_spiralPreview == null) return;

            PdfPageInfo pageInfo = boxes_pages[(int)(nud_page_number.Value - 1)];
            SpiralPlaceEnum place = (SpiralPlaceEnum)cb_place.SelectedIndex;
            int curPageIdx = (int)(nud_page_number.Value - 1);

            switch (place)
            {
                case SpiralPlaceEnum.top:
                    DrawSpiralTop(g, pageInfo);
                    break;
                case SpiralPlaceEnum.bottom:
                    DrawSpiralBottom(g, pageInfo);
                    break;
                case SpiralPlaceEnum.left:
                    DrawSpiralLeft(g, pageInfo);
                    break;
                case SpiralPlaceEnum.right:
                    DrawSpiralRight(g, pageInfo);
                    break;
                case SpiralPlaceEnum.spread_horizontal:
                    if (curPageIdx % 2 == 0)
                        DrawSpiralLeft(g, pageInfo);
                    else
                        DrawSpiralRight(g, pageInfo);
                    break;
                case SpiralPlaceEnum.spread_vertical:
                    if (curPageIdx % 2 == 0)
                        DrawSpiralTop(g, pageInfo);
                    else
                        DrawSpiralBottom(g, pageInfo);
                    break;
                case SpiralPlaceEnum.top_bottom:
                    DrawSpiralTop(g, pageInfo);
                    DrawSpiralBottom(g, pageInfo);
                    break;

                case SpiralPlaceEnum.left_right:
                    DrawSpiralLeft(g, pageInfo);
                    DrawSpiralRight(g, pageInfo);
                    break;
                default:
                    break;
            }
        }

        private void DrawSpiralRight(Graphics g, PdfPageInfo pi)
        {
            double spiralWidth = _spiralBox.Trimbox.wMM();
            double spiralHeight = _spiralBox.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.hMM() / spiralWidth);
            double x = pi.Trimbox.wMM() - spiralHeight;
            double y = (pi.Trimbox.hMM() - (spiralWidth * cntHoles)) / 2;
            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralWidth;
                g.TranslateTransform((float)(holeX + spiralHeight / 2), (float)(holeY + spiralWidth / 2));
                g.RotateTransform(90);
                g.DrawImage(
                   _spiralPreview,
                   (float)(-spiralWidth / 2),
                   (float)(-spiralHeight / 2),
                   (float)spiralWidth,
                   (float)spiralHeight);
                g.ResetTransform();
            }
        }

        private void DrawSpiralLeft(Graphics g, PdfPageInfo pi)
        {
            double spiralWidth = _spiralBox.Trimbox.wMM();
            double spiralHeight = _spiralBox.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.hMM() / spiralWidth);
            double x = 0;
            double y = (pi.Trimbox.hMM() - (spiralWidth * cntHoles)) / 2;

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x;
                double holeY = y + i * spiralWidth;
                g.TranslateTransform((float)(holeX + spiralHeight / 2), (float)(holeY + spiralWidth / 2));
                g.RotateTransform(-90);
                g.DrawImage(
                   _spiralPreview,
                   (float)(-spiralWidth / 2),
                   (float)(-spiralHeight / 2),
                   (float)spiralWidth,
                   (float)spiralHeight);
                g.ResetTransform();
            }
        }

        private void DrawSpiralBottom(Graphics g, PdfPageInfo pi)
        {
            double spiralWidth = _spiralBox.Trimbox.wMM();
            double spiralHeight = _spiralBox.Trimbox.hMM();
            int cntHoles = (int)(pi.Trimbox.wMM() / spiralWidth);
            double x = (pi.Trimbox.wMM() - (spiralWidth * cntHoles)) / 2;
            double y = pi.Trimbox.hMM() - spiralHeight;

            for (int i = 0; i < cntHoles; i++)
            {
                double holeX = x + i * spiralWidth;
                double holeY = y;
                g.TranslateTransform((float)(holeX + spiralWidth / 2), (float)(holeY + spiralHeight / 2));
                g.RotateTransform(180);
                g.DrawImage(
                   _spiralPreview,
                   (float)(-spiralWidth / 2),
                   (float)(-spiralHeight / 2),
                   (float)spiralWidth,
                   (float)spiralHeight);

                g.ResetTransform();
            }
        }

        private void DrawSpiralTop(Graphics g, PdfPageInfo pi)
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

                g.DrawImage(_spiralPreview, (float)holeX, (float)holeY, (float)spiralWidth, (float)spiralHeight);
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

            pb_preview.Width = (int)(box.Trimbox.wMM() * pb_preview.DeviceDpi / 25.4d) + 1;
            pb_preview.Height = (int)(box.Trimbox.hMM() * pb_preview.DeviceDpi / 25.4d) + 1;

            pb_preview.Invalidate();
        }

        private void cb_place_SelectedIndexChanged(object sender, EventArgs e)
        {
            pb_preview.Invalidate();
        }

        private void cb_files_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSpiralPreview();
            pb_preview.Invalidate();
        }

        private void cb_rect_CheckedChanged(object sender, EventArgs e)
        {
            panel_rect_params.Enabled = cb_rect.Checked;
        }
    }
}
