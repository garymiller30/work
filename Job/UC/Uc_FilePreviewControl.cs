using Interfaces;
using JobSpace.Models;
using JobSpace.Models.PdfDrawer;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JobSpace.UC
{
    public partial class Uc_FilePreviewControl : UserControl
    {
        int _currentPage = 1;

        PdfDrawerPageCache pdfDrawerPageCache;

        PdfPreviewParameters previewParameters;

        int _previewRequestId;




        public Uc_FilePreviewControl()
        {
            InitializeComponent();
            previewParameters = new PdfPreviewParameters();

            uc_PreviewControl1.SetPreviewParameters(previewParameters);
        }

        public async void Show(IFileSystemInfoExt filePath)
        {
            _previewRequestId++;
            int requestId = _previewRequestId;

            pdfDrawerPageCache?.Dispose();
            pdfDrawerPageCache = null;

            if (IsFontFile(filePath))
            {
                ShowFontPreview(filePath);
                return;
            }

            _currentPage = 1;
            tst_cur_page.Text = _currentPage.ToString();
            tsl_count_pages.Text = "/...";
            SetPdfNavigationEnabled(false);
            uc_PreviewControl1.StartWait(Path.Combine(AppContext.BaseDirectory, "db\\resources\\wait.gif"));
            TryShowCachedPreview(filePath, requestId);

            try
            {
                var pageCache = await Task.Run(() => new PdfDrawerPageCache(filePath));

                if (requestId != _previewRequestId)
                {
                    pageCache.Dispose();
                    return;
                }

                pdfDrawerPageCache = pageCache;
                tsl_count_pages.Text = $"/{pdfDrawerPageCache.TotalPages}";
                SetPdfNavigationEnabled(true);

                GetPreview();
            }
            catch (Exception e)
            {
                if (requestId != _previewRequestId)
                    return;

                Logger.Log.Error(null, "Uc_FilePreviewControl.Show", e.Message);
                uc_PreviewControl1.SetImage(null, 0, 0);
                uc_PreviewControl1.StopWait();
            }
        }

        private async void TryShowCachedPreview(IFileSystemInfoExt filePath, int requestId)
        {
            if (filePath?.FileInfo == null)
                return;

            string ext = filePath.FileInfo.Extension.ToLowerInvariant();
            if (ext != ".pdf" && ext != ".ai")
                return;

            try
            {
                Image cachedPreview = await Task.Run(() => FileBrowserSevices.File_GetPreview(filePath, 0, 150, true));

                if (requestId != _previewRequestId)
                {
                    cachedPreview?.Dispose();
                    return;
                }

                if (cachedPreview == null)
                    return;

                try
                {
                    const float screenDpi = 96f;
                    double w = cachedPreview.Width * 25.4 / screenDpi;
                    double h = cachedPreview.Height * 25.4 / screenDpi;
                    uc_PreviewControl1.SetImage(cachedPreview, w, h);
                    uc_PreviewControl1.StopWait();
                }
                finally
                {
                    cachedPreview.Dispose();
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "Uc_FilePreviewControl.TryShowCachedPreview", e.Message);
            }
        }

        private void ShowFontPreview(IFileSystemInfoExt filePath)
        {
            _currentPage = 1;
            tst_cur_page.Text = _currentPage.ToString();
            tsl_count_pages.Text = "/1";
            SetPdfNavigationEnabled(false);

            try
            {
                using (Image preview = CreateFontPreview(filePath.FileInfo.FullName))
                {
                    uc_PreviewControl1.SetImage(preview, 160, 220);
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "Uc_FilePreviewControl.ShowFontPreview", e.Message);
                uc_PreviewControl1.SetImage(null, 0, 0);
            }
            finally
            {
                uc_PreviewControl1.StopWait();
            }
        }



        private async void GetPreview()
        {
            int requestId = _previewRequestId;
            var pageCache = pdfDrawerPageCache;
            int page = _currentPage;
            previewParameters.PreviewTargetLongSidePixels = uc_PreviewControl1.GetPreviewTargetLongSidePixels();

            uc_PreviewControl1.StartWait(Path.Combine(AppContext.BaseDirectory, "db\\resources\\wait.gif"));
            try
            {
                var res = await PdfDrawerService.GetImageAsync(previewParameters, pageCache, page);

                if (requestId != _previewRequestId || pageCache != pdfDrawerPageCache || page != _currentPage)
                    return;

                if (res != null)
                {
                    uc_PreviewControl1.SetImage(res.Item1, res.Item2, res.Item3);
                }
                else
                {
                    uc_PreviewControl1.SetImage(null, 0, 0);
                }

            }
            catch (Exception e)
            {
                if (requestId != _previewRequestId || pageCache != pdfDrawerPageCache || page != _currentPage)
                    return;

                Logger.Log.Error(null, "Uc_FilePreviewControl.GetPreview", e.Message);
                uc_PreviewControl1.SetImage(null, 0, 0);
            }
            finally
            {
                if (requestId == _previewRequestId && pageCache == pdfDrawerPageCache && page == _currentPage)
                    uc_PreviewControl1.StopWait();
               
            }
        }

        private void tsb_previous_page_Click(object sender, EventArgs e)
        {
            if (pdfDrawerPageCache == null)
                return;

            if (_currentPage > 1)
            {
                _currentPage--;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
            }
        }

        private void tsb_next_page_Click(object sender, EventArgs e)
        {
            if (pdfDrawerPageCache == null)
                return;

            _currentPage++;
            if (previewParameters.Display == PdfPreviewDisplay.Spread)
            {
                _currentPage++;
            }
            
            if (_currentPage > pdfDrawerPageCache.TotalPages)
            { _currentPage = 1; }

            tst_cur_page.Text = _currentPage.ToString();
            GetPreview();
        }

        private void tst_cur_page_TextChanged(object sender, EventArgs e)
        {
            if (pdfDrawerPageCache == null)
                return;

            if (int.TryParse(tst_cur_page.Text, out int page))
            {
                if (page >= 1 && page <= pdfDrawerPageCache.TotalPages)
                {
                    _currentPage = page;
                    GetPreview();

                }
                else
                {
                    tst_cur_page.Text = _currentPage.ToString();
                }
            }
        }

        public void ClearPreview()
        {
            _previewRequestId++;
            uc_PreviewControl1.SetImage(null, 0, 0);
        }

        private void tsb_fit_to_window_CheckStateChanged(object sender, EventArgs e)
        {
            uc_PreviewControl1.SetFitAndResetZoom(tsb_fit_to_window.Checked);
        }

        public PdfPageInfo GetCurrentPageInfo()
        {
            return pdfDrawerPageCache?.GetPageInfo(_currentPage);
        }

        public PdfPageInfo GetPageInfo(int pageNo)
        {
            return pdfDrawerPageCache?.GetPageInfo(pageNo);
        }

        public int GetCurrentPageIdx()
        {
            return _currentPage - 1;
        }

        public int GetTotalPages()
        {
            return pdfDrawerPageCache.TotalPages;
        }


        private void tsb_show_spread_CheckedChanged(object sender, EventArgs e)
        {
            previewParameters.Display = tsb_show_spread.Checked ? PdfPreviewDisplay.Spread : PdfPreviewDisplay.Single;
            if (pdfDrawerPageCache != null)
                GetPreview();
        }

        public void Redraw()
        {
            if (pdfDrawerPageCache != null)
                GetPreview();
            //uc_PreviewControl1.Redraw();
        }

        public void SetFunc_GetScreenPrimitives(Func<int, List<IScreenPrimitive>> getPrimitives)
        {
            previewParameters.GetScreenPrimitives = getPrimitives;
        }

        private static bool IsFontFile(IFileSystemInfoExt file)
        {
            if (file?.FileInfo == null)
                return false;

            switch (file.FileInfo.Extension.ToLowerInvariant())
            {
                case ".ttf":
                case ".otf":
                case ".ttc":
                    return true;
                default:
                    return false;
            }
        }

        private void SetPdfNavigationEnabled(bool enabled)
        {
            tsb_previous_page.Enabled = enabled;
            tsb_next_page.Enabled = enabled;
            tst_cur_page.Enabled = enabled;
            tsl_count_pages.Enabled = enabled;
            tsb_show_spread.Enabled = enabled;
        }

        private static Image CreateFontPreview(string fontPath)
        {
            const int width = 1200;
            const int height = 1650;
            const int margin = 80;

            Bitmap bitmap = new Bitmap(width, height);
            bitmap.SetResolution(96, 96);

            using (Graphics g = Graphics.FromImage(bitmap))
            using (PrivateFontCollection fontCollection = new PrivateFontCollection())
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.Clear(System.Drawing.Color.White);

                fontCollection.AddFontFile(fontPath);
                FontFamily family = fontCollection.Families[0];

                using (Brush textBrush = new SolidBrush(System.Drawing.Color.FromArgb(32, 32, 32)))
                using (Brush mutedBrush = new SolidBrush(System.Drawing.Color.FromArgb(105, 105, 105)))
                using (Pen linePen = new Pen(System.Drawing.Color.FromArgb(220, 220, 220), 2))
                using (Font titleFont = new Font("Segoe UI", 28, FontStyle.Regular))
                using (Font metaFont = new Font("Segoe UI", 18, FontStyle.Regular))
                using (Font sample72 = CreateFont(family, 72))
                using (Font sample48 = CreateFont(family, 48))
                using (Font sample34 = CreateFont(family, 34))
                using (Font sample26 = CreateFont(family, 26))
                {
                    int y = margin;
                    string fileName = Path.GetFileName(fontPath);

                    g.DrawString(family.Name, titleFont, textBrush, margin, y);
                    y += 54;
                    g.DrawString(fileName, metaFont, mutedBrush, margin, y);
                    y += 58;
                    g.DrawLine(linePen, margin, y, width - margin, y);
                    y += 70;

                    g.DrawString("AaBbCcDdEeFfGg", sample72, textBrush, margin, y);
                    y += 150;
                    g.DrawString("1234567890 ?!&@", sample48, textBrush, margin, y);
                    y += 110;
                    g.DrawString("The quick brown fox jumps over the lazy dog", sample34, textBrush, margin, y);
                    y += 85;
                    g.DrawString("Ґґ Єє Іі Її Україна", sample34, textBrush, margin, y);
                    y += 110;

                    g.DrawLine(linePen, margin, y, width - margin, y);
                    y += 60;

                    DrawAlphabet(g, sample26, textBrush, margin, y, width - margin * 2);
                }
            }

            return bitmap;
        }

        private static Font CreateFont(FontFamily family, float size)
        {
            FontStyle style = family.IsStyleAvailable(FontStyle.Regular) ? FontStyle.Regular : FontStyle.Bold;
            return new Font(family, size, style, GraphicsUnit.Pixel);
        }

        private static void DrawAlphabet(Graphics g, Font font, Brush brush, int x, int y, int maxWidth)
        {
            string[] lines =
            {
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "abcdefghijklmnopqrstuvwxyz",
                "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ",
                "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя",
                "0123456789   .,:;!?()[]{}"
            };

            StringFormat format = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };

            foreach (string line in lines)
            {
                g.DrawString(line, font, brush, new RectangleF(x, y, maxWidth, font.Height + 14), format);
                y += font.Height + 28;
            }
        }
    }
}
