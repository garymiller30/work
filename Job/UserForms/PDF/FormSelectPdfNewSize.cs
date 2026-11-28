using ImageMagick;
using Interfaces;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Manipulation.Scale;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public partial class FormSelectPdfNewSize : Form
    {
        public PdfScaleParams Params { get; set; } = new PdfScaleParams();
        private IFileSystemInfoExt _fsi;
        private double _origWidth;
        private double _origHeight;
        private bool _isUpdating = false;

        private List<PdfPageInfo> _pagesInfo = new();
        private int _currentPageIdx = 0;
        private int _totalPages = 1;
        private readonly Dictionary<int, Bitmap> _previewCache = new();

        public FormSelectPdfNewSize()
        {
            InitializeComponent();
            comboBoxWorH.SelectedIndex = 0;
            DialogResult = DialogResult.Cancel;
        }

        public FormSelectPdfNewSize(IFileSystemInfoExt fsi) : this()
        {
            _fsi = fsi;
            try
            {
                var pages = PdfHelper.GetPagesInfo(_fsi.FileInfo.FullName);
                if (pages != null && pages.Count > 0)
                {
                    _pagesInfo = pages;
                    _totalPages = Math.Max(1, _pagesInfo.Count);

                    var page0 = pages[0];
                    _origWidth = page0.Trimbox.wMM();
                    _origHeight = page0.Trimbox.hMM();
                    if (_origWidth <= 0 || _origHeight <= 0)
                    {
                        _origWidth = page0.Mediabox.wMM();
                        _origHeight = page0.Mediabox.hMM();
                    }

                    _isUpdating = true;
                    try
                    {
                        numericUpDownWidth.Value = (decimal)_origWidth;
                        numericUpDownHeight.Value = (decimal)_origHeight;
                    }
                    finally
                    {
                        _isUpdating = false;
                    }

                    if (pages.Count > 1)
                    {
                        bool hasDifferentSizes = pages.Any(p =>
                        {
                            var box = p.Trimbox.wMM() > 0 ? p.Trimbox : p.Mediabox;
                            return Math.Abs(box.wMM() - _origWidth) > 0.5 || Math.Abs(box.hMM() - _origHeight) > 0.5;
                        });

                        bool hasDifferentOrientation = pages.Any(p =>
                        {
                            var box = p.Trimbox.wMM() > 0 ? p.Trimbox : p.Mediabox;
                            return (box.wMM() > box.hMM()) != (_origWidth > _origHeight);
                        });

                        if (hasDifferentSizes || hasDifferentOrientation)
                        {
                            labelWarning.Visible = true;
                            labelWarning.Text = "⚠ У документі виявлено сторінки різних розмірів або орієнтації";
                        }
                    }
                }
            }
            catch
            {
                var pageInfo = PdfHelper.GetPageInfo(_fsi.FileInfo.FullName, 0);
                if (pageInfo != null)
                {
                    _pagesInfo = new List<PdfPageInfo> { pageInfo };
                    _origWidth = pageInfo.Trimbox.wMM();
                    _origHeight = pageInfo.Trimbox.hMM();
                    if (_origWidth <= 0 || _origHeight <= 0)
                    {
                        _origWidth = pageInfo.Mediabox.wMM();
                        _origHeight = pageInfo.Mediabox.hMM();
                    }

                    _isUpdating = true;
                    try
                    {
                        numericUpDownWidth.Value = (decimal)_origWidth;
                        numericUpDownHeight.Value = (decimal)_origHeight;
                    }
                    finally
                    {
                        _isUpdating = false;
                    }
                }
            }

            UpdatePageNav();
            UpdatePreview();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            foreach (var bmp in _previewCache.Values)
            {
                bmp?.Dispose();
            }
            _previewCache.Clear();
        }

        private void UpdatePreview_Event(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            pictureBoxPreview?.Invalidate();
        }

        private void pictureBoxPreview_Resize(object sender, EventArgs e)
        {
            pictureBoxPreview?.Invalidate();
        }

        private void buttonPrevPage_Click(object sender, EventArgs e)
        {
            if (_currentPageIdx > 0)
            {
                _currentPageIdx--;
                UpdatePageNav();
                UpdatePreview();
            }
        }

        private void buttonNextPage_Click(object sender, EventArgs e)
        {
            if (_currentPageIdx < _totalPages - 1)
            {
                _currentPageIdx++;
                UpdatePageNav();
                UpdatePreview();
            }
        }

        private void UpdatePageNav()
        {
            labelPageNav.Text = $"{_currentPageIdx + 1} / {_totalPages}";
            buttonPrevPage.Enabled = _currentPageIdx > 0;
            buttonNextPage.Enabled = _currentPageIdx < _totalPages - 1;
        }

        private Bitmap? GetPageBitmap(int pageIdx)
        {
            if (_previewCache.TryGetValue(pageIdx, out var cached))
                return cached;

            if (_fsi == null) return null;

            try
            {
                var img = FileBrowserSevices.File_GetPreview(_fsi, pageIdx, 100);
                if (img is Bitmap bmp)
                {
                    _previewCache[pageIdx] = bmp;
                    return bmp;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, "FormSelectPdfNewSize.GetPageBitmap", ex.Message);
            }
            return null;
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_pagesInfo == null || _pagesInfo.Count == 0 || _currentPageIdx >= _pagesInfo.Count)
                return;

            var pageInfo = _pagesInfo[_currentPageIdx];
            Box sourceBox = radioButtonMediabox.Checked ? pageInfo.Mediabox : pageInfo.Trimbox;
            if (sourceBox == null || sourceBox.width <= 0 || sourceBox.height <= 0)
                sourceBox = pageInfo.Mediabox;

            if (sourceBox == null || sourceBox.width <= 0 || sourceBox.height <= 0)
                return;

            double sourceW = sourceBox.wMM();
            double sourceH = sourceBox.hMM();
            if (sourceW <= 0 || sourceH <= 0) return;

            double bleed = (double)numericUpDownBleed.Value;
            double targetTrimW;
            double targetTrimH;
            double targetMediaW;
            double targetMediaH;
            double xScale;
            double yScale;

            if (radioButtonScalePercent.Checked)
            {
                double factor = (double)numericUpDownPercent.Value / 100.0;
                xScale = factor;
                yScale = factor;

                if (radioButtonTrimbox.Checked)
                {
                    targetTrimW = sourceW * xScale;
                    targetTrimH = sourceH * yScale;
                    targetMediaW = targetTrimW + bleed * 2;
                    targetMediaH = targetTrimH + bleed * 2;
                }
                else
                {
                    targetMediaW = sourceW * xScale;
                    targetMediaH = sourceH * yScale;
                    targetTrimW = Math.Max(0, targetMediaW - bleed * 2);
                    targetTrimH = Math.Max(0, targetMediaH - bleed * 2);
                }
            }
            else
            {
                bool isSourceLandscape = sourceW > sourceH;
                bool isTargetLandscape = numericUpDownWidth.Value > numericUpDownHeight.Value;

                if (checkBoxAutoOrientation.Checked && (isSourceLandscape != isTargetLandscape))
                {
                    targetTrimW = (double)numericUpDownHeight.Value;
                    targetTrimH = (double)numericUpDownWidth.Value;
                }
                else
                {
                    targetTrimW = (double)numericUpDownWidth.Value;
                    targetTrimH = (double)numericUpDownHeight.Value;
                }

                targetMediaW = targetTrimW + bleed * 2;
                targetMediaH = targetTrimH + bleed * 2;

                if (radioButtonTrimbox.Checked)
                {
                    xScale = targetTrimW / sourceW;
                    yScale = targetTrimH / sourceH;
                }
                else // Mediabox
                {
                    xScale = targetMediaW / sourceW;
                    yScale = targetMediaH / sourceH;
                }

                if (radioButtonProportial.Checked)
                {
                    if (xScale > yScale) xScale = yScale;
                    else yScale = xScale;
                }
            }

            // Розрахунок розміщення на полотні прев'ю
            float pad = 16f;
            float availW = pictureBoxPreview.ClientSize.Width - pad * 2;
            float availH = pictureBoxPreview.ClientSize.Height - pad * 2;
            if (availW <= 0 || availH <= 0) return;

            double pxPerMm = Math.Min(availW / targetMediaW, availH / targetMediaH);
            float sheetW = (float)(targetMediaW * pxPerMm);
            float sheetH = (float)(targetMediaH * pxPerMm);
            float sheetX = (pictureBoxPreview.ClientSize.Width - sheetW) / 2f;
            float sheetY = (pictureBoxPreview.ClientSize.Height - sheetH) / 2f;
            RectangleF sheetRect = new RectangleF(sheetX, sheetY, sheetW, sheetH);

            // Тінь аркуша
            using (var shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
            {
                g.FillRectangle(shadowBrush, sheetX + 3, sheetY + 3, sheetW, sheetH);
            }

            // Тло нового аркуша (біле)
            g.FillRectangle(Brushes.White, sheetRect);

            // Малювання вмісту сторінки
            Bitmap? pageBmp = GetPageBitmap(_currentPageIdx);
            if (pageBmp != null)
            {
                double pageXmm;
                double pageYmm;

                if (radioButtonTrimbox.Checked)
                {
                    double deltaX = (targetTrimW - (sourceBox.width * xScale / PdfHelper.mn)) / 2.0;
                    double deltaY = (targetTrimH - (sourceBox.height * yScale / PdfHelper.mn)) / 2.0;
                    pageXmm = bleed - (sourceBox.left * xScale / PdfHelper.mn) + deltaX;
                    pageYmm = bleed - (sourceBox.bottom * yScale / PdfHelper.mn) + deltaY;
                }
                else // Mediabox
                {
                    double deltaX = (targetMediaW - (sourceBox.width * xScale / PdfHelper.mn)) / 2.0;
                    double deltaY = (targetMediaH - (sourceBox.height * yScale / PdfHelper.mn)) / 2.0;
                    pageXmm = deltaX;
                    pageYmm = deltaY;
                }

                double imgWmm = pageInfo.Mediabox.wMM() * xScale;
                double imgHmm = pageInfo.Mediabox.hMM() * yScale;

                float imgScreenX = sheetX + (float)(pageXmm * pxPerMm);
                float imgScreenY = sheetY + sheetH - (float)((pageYmm + imgHmm) * pxPerMm);
                float imgScreenW = (float)(imgWmm * pxPerMm);
                float imgScreenH = (float)(imgHmm * pxPerMm);

                var prevClip = g.Clip;
                g.SetClip(sheetRect);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(pageBmp, new RectangleF(imgScreenX, imgScreenY, imgScreenW, imgScreenH));
                g.Clip = prevClip;
            }

            // Рамка аркуша
            using (var penSheet = new Pen(Color.FromArgb(160, 160, 160), 1f))
            {
                g.DrawRectangle(penSheet, sheetX, sheetY, sheetW, sheetH);
            }

            // Виділення зони вильотів (Bleed) та лінії обрізки (TrimBox)
            float trimX = sheetX + (float)(bleed * pxPerMm);
            float trimY = sheetY + (float)(bleed * pxPerMm);
            float trimW = (float)(targetTrimW * pxPerMm);
            float trimH = (float)(targetTrimH * pxPerMm);

            if (bleed > 0)
            {
                using var bleedBrush = new SolidBrush(Color.FromArgb(20, 0, 140, 255));
                g.FillRectangle(bleedBrush, sheetX, sheetY, sheetW, (float)(bleed * pxPerMm));
                g.FillRectangle(bleedBrush, sheetX, trimY + trimH, sheetW, (float)(bleed * pxPerMm));
                g.FillRectangle(bleedBrush, sheetX, trimY, (float)(bleed * pxPerMm), trimH);
                g.FillRectangle(bleedBrush, trimX + trimW, trimY, (float)(bleed * pxPerMm), trimH);
            }

            if (checkBoxShowTrim.Checked)
            {
                using var penTrim = new Pen(Color.FromArgb(20, 140, 20), 1.2f) { DashStyle = DashStyle.Dash };
                g.DrawRectangle(penTrim, trimX, trimY, trimW, trimH);
            }

            // Оновлення інформаційного рядка
            labelPreviewDetails.Text = $"{targetTrimW:0.#}×{targetTrimH:0.#} мм | {xScale * 100:0.#}%";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Params.ScaleVariant = radioButtonNonProportial.Checked ? ScaleVariantEnum.NonProportial : ScaleVariantEnum.Proportial;
            Params.ScaleBy = radioButtonMediabox.Checked ? ScaleByEnum.Mediabox : ScaleByEnum.TrimBox;
            Params.AutoMatchOrientation = checkBoxAutoOrientation.Checked;
            Params.ScaleMode = radioButtonScalePercent.Checked ? ScaleModeEnum.ScaleByPercent : ScaleModeEnum.FitToTargetSize;
            Params.ScalePercent = (double)numericUpDownPercent.Value;

            Params.TargetSize.Width = (double)numericUpDownWidth.Value;
            Params.TargetSize.Height = (double)numericUpDownHeight.Value;
            Params.TargetSize.Bleed = (double)numericUpDownBleed.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdating || _origWidth <= 0) return;

            _isUpdating = true;
            try
            {
                decimal pct = (numericUpDownWidth.Value / (decimal)_origWidth) * 100m;
                if (pct >= numericUpDownPercent.Minimum && pct <= numericUpDownPercent.Maximum)
                {
                    numericUpDownPercent.Value = Math.Round(pct, 1);
                }
            }
            finally
            {
                _isUpdating = false;
            }

            UpdatePreview();
        }

        private void numericUpDownPercent_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdating || !radioButtonScalePercent.Checked) return;
            if (_origWidth <= 0 || _origHeight <= 0) return;

            _isUpdating = true;
            try
            {
                decimal factor = numericUpDownPercent.Value / 100m;
                numericUpDownWidth.Value = Math.Round((decimal)_origWidth * factor, 1);
                numericUpDownHeight.Value = Math.Round((decimal)_origHeight * factor, 1);
            }
            finally
            {
                _isUpdating = false;
            }

            UpdatePreview();
        }

        private void radioButtonScalePercent_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownPercent.Enabled = radioButtonScalePercent.Checked;
            if (radioButtonScalePercent.Checked)
            {
                numericUpDownPercent_ValueChanged(sender, e);
            }
            UpdatePreview();
        }

        private void numericUpDownWidth_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void ucSelectStandartPageFormat1_PaperFormatChanged(object sender, PaperFormat e)
        {
            numericUpDownWidth.Value = e.Width;
            numericUpDownHeight.Value = e.Height;
            UpdatePreview();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var tmp = numericUpDownWidth.Value;
            numericUpDownWidth.Value = numericUpDownHeight.Value;
            numericUpDownHeight.Value = tmp;
            UpdatePreview();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelWorH.Text = comboBoxWorH.SelectedIndex == 0 ? "висота" : "ширина";
            Calc();
        }

        private void buttonGarazd_Click(object sender, EventArgs e)
        {
            var res = Calc();
            numericUpDownWidth.Value = res.Item1;
            numericUpDownHeight.Value = res.Item2;
            UpdatePreview();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Calc();
        }

        Tuple<decimal, decimal> Calc()
        {
            decimal val = numericUpDownVal.Value;
            var width = numericUpDownWidth.Value;
            var height = numericUpDownHeight.Value;

            decimal percent;

            if (comboBoxWorH.SelectedIndex == 0)
            {
                percent = width / val;
                decimal height_scaled = height / percent;
                textBox1.Text = height_scaled.ToString("N01");
                return Tuple.Create(val, height_scaled);
            }
            else
            {
                percent = height / val;
                decimal width_scaled = width / percent;
                textBox1.Text = width_scaled.ToString("N01");
                return Tuple.Create(width_scaled, val);
            }
        }
    }
}
