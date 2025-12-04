using Interfaces;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UC
{
    public partial class Uc_PreviewBrowserFile : UserControl
    {
        int _currentPage = 1;
        int _totalPage = 1;
        IFileSystemInfoExt _fileInfo;


        public Uc_PreviewBrowserFile()
        {
            InitializeComponent();
        }

        public void Show(IFileSystemInfoExt filePath)
        {
            _fileInfo = filePath;
            GetFileInfo();
            GetPreview();
        }

        private async void GetFileInfo()
        {
            _currentPage = 1;
            _totalPage = 1;
            tst_cur_page.Text = _currentPage.ToString();
            tsl_count_pages.Text = $"/{_totalPage}";

            string ext = _fileInfo.FileInfo.Extension.ToLower();
            // якщо це pdf файл, то отримуємо кількість сторінок
            if (ext == ".pdf" || ext == ".ai")
            {
                _totalPage = await Task.Run(() => PdfHelper.GetPageCount(_fileInfo.FileInfo.FullName));
                tsl_count_pages.Text = $"/{_totalPage}";
            }
        }

        private async void GetPreview()
        {
            // Скинути попереднє зображення коректно
            if (pb_preview.Image != null)
            {
                var old = pb_preview.Image;
                pb_preview.Image = null;
                old.Dispose();
            }
            pb_preview.SizeMode = PictureBoxSizeMode.CenterImage;
            // Спершу показуємо анімований GIF
            pb_preview.ImageLocation = Path.Combine(AppContext.BaseDirectory, "db\\resources\\wait.gif");

            // Асинхронно завантажуємо фінальне зображення
            var preview = await Task.Run(() =>
                FileBrowserSevices.File_GetPreview(_fileInfo, _currentPage - 1)
            );
            // створити копію зображення, щоб уникнути проблем з потоками
            var temp = new System.Drawing.Bitmap(preview);
            preview.Dispose();


            // Зупиняємо завантаження GIF і прибираємо його
            pb_preview.ImageLocation = null;
            pb_preview.SizeMode = PictureBoxSizeMode.Zoom;
            // Тепер ставимо вже фінальне зображення
            pb_preview.Image = temp;
        }

        private void tsb_previous_page_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
            }
        }

        private void tsb_next_page_Click(object sender, EventArgs e)
        {
            if (_currentPage < _totalPage)
            {
                _currentPage++;
                tst_cur_page.Text = _currentPage.ToString();
                GetPreview();
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
                }
                else
                {
                    tst_cur_page.Text = _currentPage.ToString();
                }
            }
        }

        public void ClearPreview()
        {
            pb_preview.Image?.Dispose();
            pb_preview.Image = null;
        }
    }
}
