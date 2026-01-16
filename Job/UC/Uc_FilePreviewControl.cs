using Interfaces;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UC
{
    public partial class Uc_FilePreviewControl : UserControl
    {
        int _currentPage = 1;
        int _totalPage = 1;
        IFileSystemInfoExt _fileInfo;
        List<PdfPageInfo> boxes_pages;
        Image[] images;

        #region [ EVENTS ]
        public event EventHandler<int> OnPageChanged = delegate { };
        #endregion

        public Uc_FilePreviewControl()
        {
            InitializeComponent();
        }

        public void Show(IFileSystemInfoExt filePath)
        {
            _fileInfo = filePath;
            GetFileInfo();
            GetPreview();
        }

        private void GetFileInfo()
        {
            _currentPage = 1;
            _totalPage = 1;
            tst_cur_page.Text = _currentPage.ToString();
            tsl_count_pages.Text = $"/{_totalPage}";

            string ext = _fileInfo.FileInfo.Extension.ToLower();
            // якщо це pdf файл, то отримуємо кількість сторінок
            if (ext == ".pdf" || ext == ".ai")
            {
                _totalPage = PdfHelper.GetPageCount(_fileInfo.FileInfo.FullName);
                tsl_count_pages.Text = $"/{_totalPage}";
                boxes_pages = PdfHelper.GetPagesInfo(_fileInfo.FileInfo.FullName);

                // очистити старі зображення
                if (images != null)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        if (images[i] != null)
                        {
                            images[i].Dispose();
                        }
                    }
                }
                // підготувати кеш зображень
                images = new Image[_totalPage];
            }
        }

        private async void GetPreview()
        {
            double wMM = 100;
            double hMM = 100;

            Image preview = null;

            // перевірити чи є кешоване зображення
            if (images != null && images[_currentPage - 1] != null)
            {
                preview = images[_currentPage - 1];
            }
            else
            {
                uc_PreviewControl1.StartWait(Path.Combine(AppContext.BaseDirectory, "db\\resources\\wait.gif"));
                // Асинхронно завантажуємо фінальне зображення
                preview = await Task.Run(() =>
                   FileBrowserSevices.File_GetPreview(_fileInfo, _currentPage - 1)
               );
                // створити копію зображення, щоб уникнути проблем з потоками
                if (preview != null)
                {
                    var temp = new System.Drawing.Bitmap(preview);
                    preview.Dispose();
                    preview = temp;
                    wMM = (double)(preview.Width / preview.HorizontalResolution * 25.4);
                    hMM = (double)(preview.Height / preview.VerticalResolution * 25.4);
                    // кешувати зображення
                    if (images != null)
                    {
                        images[_currentPage - 1] = preview;
                    }
                }
                uc_PreviewControl1.StopWait();
            }

            if (boxes_pages != null)
            {
                wMM = boxes_pages[_currentPage - 1].Trimbox.wMM();
                hMM = boxes_pages[_currentPage - 1].Trimbox.hMM();
            }

            uc_PreviewControl1.SetImage(preview, wMM, hMM);
        }

        private void tsb_previous_page_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
                OnPageChanged(this, _currentPage);
            }
        }

        private void tsb_next_page_Click(object sender, EventArgs e)
        {
            if (_currentPage < _totalPage)
            {
                _currentPage++;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
                OnPageChanged(this, _currentPage);
            }
        }

        private void tst_cur_page_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tst_cur_page.Text, out int page))
            {
                if (page >= 1 && page <= _totalPage)
                {
                    _currentPage = page;
                    GetPreview();
                    OnPageChanged(this, _currentPage);
                }
                else
                {
                    tst_cur_page.Text = _currentPage.ToString();
                }
            }
        }

        public void ClearPreview()
        {
            uc_PreviewControl1.SetImage(null, 0, 0);
        }

        private void tsb_fit_to_window_CheckStateChanged(object sender, EventArgs e)
        {
            uc_PreviewControl1.SetFitAndResetZoom(tsb_fit_to_window.Checked);
        }

        public void SetPrimitives(List<IScreenPrimitive> primitives)
        {
            uc_PreviewControl1.Primitives = primitives;
        }

        public PdfPageInfo GetCurrentPageInfo()
        {
            return boxes_pages != null ? boxes_pages[_currentPage - 1] : null;
        }

        public int GetCurrentPageIdx()
        {
            return _currentPage - 1;
        }
    }
}
