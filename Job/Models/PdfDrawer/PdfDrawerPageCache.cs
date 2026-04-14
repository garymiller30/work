using Interfaces;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using JobSpace.UC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.PdfDrawer
{
    public class PdfDrawerPageCache : IDisposable
    {
        public int TotalPages { get; private set; }

        IFileSystemInfoExt _file;
        PdfPageInfo[] boxes_pages;
        Image[] images;
        public void Dispose()
        {
            if (images != null)
                foreach (var image in images)
                {
                    image?.Dispose();
                }
        }


        public PdfDrawerPageCache(IFileSystemInfoExt file)
        {
            _file = file;
            GetFileInfo();
        }

        void GetFileInfo()
        {
            string ext = _file.FileInfo.Extension.ToLower();
            // якщо це pdf файл, то отримуємо кількість сторінок
            if (ext == ".pdf" || ext == ".ai")
            {
                TotalPages = PdfHelper.GetPageCount(_file.FileInfo.FullName);
            }
            else
            {
                TotalPages = 1;
            }

            boxes_pages = new PdfPageInfo[TotalPages];
            // підготувати кеш зображень
            images = new Image[TotalPages];
        }

        public async Task<Tuple<Image, double, double>> GetPreviewAsync(int pageNo)
        {

            double wMM = 100;
            double hMM = 100;

            Image preview = null;
            int pageIdx = pageNo - 1;

            if (pageIdx >= 0 && pageIdx < TotalPages)

                // перевірити чи є кешоване зображення
                if (images != null && images[pageIdx] != null)
                {
                    preview = images[pageIdx];
                }
                else
                {
                    string ext = _file.FileInfo.Extension.ToLower();
                    // якщо це pdf файл, то отримуємо кількість сторінок
                    if (ext == ".pdf" || ext == ".ai")
                    {
                        PdfPageInfo pageInfo = PdfHelper.GetPageInfo(_file.FileInfo.FullName, pageIdx);
                        boxes_pages[pageIdx] = pageInfo;
                    }
                    else
                    {
                        // для інших типів файлів можна отримати розміри, але не обертати
                        //Отримати розміри зображення без завантаження повного зображення
                        Size size = FileBrowserSevices.GetImageSize(_file.FileInfo.FullName);

                        boxes_pages[pageIdx] = new PdfPageInfo
                        {
                            Trimbox = new Box { width = size.Width * PdfHelper.mn, height = size.Height * PdfHelper.mn },
                            Rotate = 0
                        };
                    }

                    // Асинхронно завантажуємо фінальне зображення
                    preview = await Task.Run(() => FileBrowserSevices.File_GetPreview(_file, pageIdx));
                }

            if (preview != null)
            {
                // створити копію зображення, щоб уникнути проблем з потоками
                var temp = new System.Drawing.Bitmap(preview);
                preview.Dispose();
                preview = temp;
                // кешувати зображення
                if (images != null)
                {
                    images[pageIdx] = preview;
                }
                double angle = boxes_pages[pageIdx].Rotate;
                wMM = boxes_pages[pageIdx].Trimbox.wMM(angle);
                hMM = boxes_pages[pageIdx].Trimbox.hMM(angle);

                return new Tuple<Image, double, double>(preview, wMM, hMM);
            }

            return null;
        }

        public PdfPageInfo GetPageInfo(int pageNo)
        {
            return boxes_pages != null ? boxes_pages[pageNo - 1] : null;
        }


    }
}
