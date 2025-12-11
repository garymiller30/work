using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.UC;
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
        IFileSystemInfoExt _fileInfo;
        PdfPageInfo pdfPageInfo;
        Image _page_preview;

        public FormVisualHardCover(IFileSystemInfoExt f)
        {
            InitializeComponent();
            uc_PreviewControl1.FitToScreen = cb_fit_to_panel.Checked;
            _fileInfo = f;
            pdfPageInfo = PdfHelper.GetPageInfo(_fileInfo.FileInfo.FullName);
            GetFilePreview();
            CalcSchemaAuto();
            ShowTotalCoverSize();
        }

        private void CalcSchemaAuto()
        {
            nud_width.Value = ((decimal)pdfPageInfo.Trimbox.wMM() - (nud_zagyn.Value + nud_rastav.Value) * 2 - nud_root.Value) / 2;
            nud_height.Value = (decimal)pdfPageInfo.Trimbox.hMM() - (nud_zagyn.Value * 2);
        }

        private void GetFilePreview()
        {
            if (_page_preview != null) _page_preview.Dispose();
            _page_preview = PdfHelper.RenderByTrimBox(_fileInfo.FileInfo.FullName, 0);
            uc_PreviewControl1.SetImage(_page_preview, pdfPageInfo.Trimbox.wMM(), pdfPageInfo.Trimbox.hMM());
        }



        private void nud_width_ValueChanged(object sender, EventArgs e)
        {
            ShowTotalCoverSize();
            uc_PreviewControl1.Redraw();
        }

        private void ShowTotalCoverSize()
        {
            DrawSchema();
            nud_total_width.Value = nud_root.Value + (nud_width.Value + nud_rastav.Value + nud_zagyn.Value) * 2;
            nud_total_height.Value = nud_height.Value + (nud_zagyn.Value * 2);
        }

        private void DrawSchema()
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

            List<IScreenPrimitive> primitives = new List<IScreenPrimitive>();

            using (Pen pen = new Pen(Color.Red, 0.5f))
            {
                // загальний розмір
                primitives.Add(new ScreenRectangle(pen, x, y, totalW, totalH));
                // лівий загин
                primitives.Add(new ScreenLine(pen, x + zagyn, y, x + zagyn, y + totalH));
                // правий загин
                primitives.Add(new ScreenLine(pen, x + totalW - zagyn, y, x + totalW - zagyn, y + totalH));
                // верхній загин
                primitives.Add(new ScreenLine(pen, x, y + zagyn, x + totalW, y + zagyn));
                // нижній загин
                primitives.Add(new ScreenLine(pen, x, y + totalH - zagyn, x + totalW, y + totalH - zagyn));
                // ліва сторінка
                primitives.Add(new ScreenLine(pen, x + zagyn + width, y + zagyn, x + zagyn + width, y + totalH - zagyn));
                // растав
                primitives.Add(new ScreenLine(pen, x + zagyn + width + rastav, y + zagyn, x + zagyn + width + rastav, y + totalH - zagyn));
                // корінець
                primitives.Add(new ScreenLine(pen, x + zagyn + width + rastav + root, y + zagyn, x + zagyn + width + rastav + root, y + totalH - zagyn));
                // права сторінка
                primitives.Add(new ScreenLine(pen, x + zagyn + width + rastav + root + rastav, y + zagyn, x + zagyn + width + rastav + root + rastav, y + totalH - zagyn));
            }

            uc_PreviewControl1.Primitives = primitives;
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
