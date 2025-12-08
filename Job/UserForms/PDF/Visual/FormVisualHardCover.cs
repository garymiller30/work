using Interfaces;
using JobSpace.Static.Pdf.Common;
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
    public partial class FormVisualHardCover : Form
    {
        float _zoomFactor = 1.0f;
        IFileSystemInfoExt _fileInfo;
        PdfPageInfo pdfPageInfo;
        Image _page_preview;

        public FormVisualHardCover(IFileSystemInfoExt f)
        {
            InitializeComponent();
            _fileInfo = f;
            pdfPageInfo = PdfHelper.GetPageInfo(_fileInfo.FileInfo.FullName);
            GetFilePreview();
            CalcSchemaAuto();
            ShowTotalCoverSize();
        }

        private void CalcSchemaAuto()
        {
            nud_width.Value = ((decimal)pdfPageInfo.Trimbox.wMM() - (nud_zagyn.Value + nud_rastav.Value)*2 - nud_root.Value)/2;
            nud_height.Value = (decimal)pdfPageInfo.Trimbox.hMM() - (nud_zagyn.Value * 2);
        }

        private void GetFilePreview()
        {
            if (_page_preview != null) _page_preview.Dispose();
            _page_preview = PdfHelper.RenderByTrimBox(_fileInfo.FileInfo.FullName, 0);

            UpdatePreviewLayout();
        }

        private void UpdatePreviewLayout()
        {
            if (_page_preview == null) return;



            float pageWmm = (float)pdfPageInfo.Trimbox.wMM();
            float pageHmm = (float)pdfPageInfo.Trimbox.hMM();

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

        }

        private void nud_width_ValueChanged(object sender, EventArgs e)
        {
            ShowTotalCoverSize();
            pb_preview.Invalidate();
        }

        private void ShowTotalCoverSize()
        {
            nud_total_width.Value = nud_root.Value + (nud_width.Value + nud_rastav.Value + nud_zagyn.Value) * 2;
            nud_total_height.Value = nud_height.Value + (nud_zagyn.Value * 2);
        }

        private void pb_preview_Paint(object sender, PaintEventArgs e)
        {
            if (_page_preview == null || pdfPageInfo == null) return;

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            g.ScaleTransform(_zoomFactor, _zoomFactor);

            g.DrawImage(_page_preview, 0, 0, (float)pdfPageInfo.Trimbox.wMM(), (float)pdfPageInfo.Trimbox.hMM());

            DrawSchema(g);

        }

        private void DrawSchema(Graphics g)
        {
            // Загини
            float zagyn = (float)nud_zagyn.Value;
            float width = (float)nud_width.Value;
            float height = (float)nud_height.Value;
            float rastav = (float)nud_rastav.Value;
            float root = (float)nud_root.Value;
            float totalW = (float)nud_total_width.Value;
            float totalH = (float)nud_total_height.Value;

            float x = ((float)pdfPageInfo.Trimbox.wMM() - totalW) / 2;
            float y = ((float)pdfPageInfo.Trimbox.hMM() - totalH) / 2;


            using (Pen pen = new Pen(Color.Red, 0.5f))
            {
                // загальний розмір
                g.DrawRectangle(pen, x, y, totalW, totalH);
                // лівий загин
                g.DrawLine(pen, x + zagyn, y, x + zagyn, y + totalH);
                // правий загин
                g.DrawLine(pen, x + totalW - zagyn, y, x + totalW - zagyn, y + totalH);
                // верхній загин
                g.DrawLine(pen, x, y+ zagyn,x+totalW,y+zagyn);
                // нижній загин
                g.DrawLine(pen,x,y+totalH - zagyn,x+totalW, y + totalH - zagyn);
                // ліва сторінка
                g.DrawLine(pen, x + zagyn +  width,y + zagyn, x + zagyn + width, y + totalH - zagyn);
                // растав
                g.DrawLine(pen, x + zagyn + width + rastav, y + zagyn, x + zagyn + width + rastav, y + totalH - zagyn);
                // корінець
                g.DrawLine(pen, x + zagyn + width + rastav + root, y + zagyn, x + zagyn + width + rastav + root, y + totalH - zagyn);
                // права сторінка
                g.DrawLine(pen, x + zagyn + width + rastav + root + rastav, y + zagyn, x + zagyn + width + rastav + root + rastav, y + totalH - zagyn);
            }
        }

     

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            UpdatePreviewLayout();
        }
    }
}
