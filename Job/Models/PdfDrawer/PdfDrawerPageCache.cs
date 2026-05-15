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
        int[] imageDpis;
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
            imageDpis = new int[TotalPages];
        }

        public async Task<Tuple<Image, double, double>> GetPreviewAsync(PdfPreviewParameters parameters, int pageNo)
        {

            double wMM = 100;
            double hMM = 100;

            Image preview = null;
            int pageIdx = pageNo - 1;

            if (pageIdx < 0 || pageIdx >= TotalPages || boxes_pages == null || images == null)
                return null;

            string ext = _file.FileInfo.Extension.ToLower();
            if (boxes_pages[pageIdx] == null)
            {
                boxes_pages[pageIdx] = await Task.Run(() =>
                {
                    // якщо це pdf файл, то отримуємо кількість сторінок
                    if (ext == ".pdf" || ext == ".ai")
                    {
                        return PdfHelper.GetPageInfo(_file.FileInfo.FullName, pageIdx);
                    }

                    // для інших типів файлів можна отримати розміри, але не обертати
                    //Отримати розміри зображення без завантаження повного зображення
                    Size size = FileBrowserSevices.GetImageSize(_file.FileInfo.FullName);

                    return new PdfPageInfo
                    {
                        Trimbox = new Box { width = size.Width * PdfHelper.mn, height = size.Height * PdfHelper.mn },
                        Rotate = 0
                    };
                });
            }

            int requiredDpi = GetRequiredPreviewDpi(ext, boxes_pages[pageIdx], parameters);

            // перевірити чи є кешоване зображення достатньої якості
            if (images[pageIdx] != null && imageDpis[pageIdx] >= requiredDpi)
            {
                preview = images[pageIdx];
            }
            else
            {
                // Асинхронно завантажуємо фінальне зображення
                preview = await Task.Run(() => FileBrowserSevices.File_GetPreview(_file, pageIdx, requiredDpi));
            }

            if (preview != null)
            {
                // створити копію зображення, щоб уникнути проблем з потоками
                var temp = new System.Drawing.Bitmap(preview);
                if (!ReferenceEquals(preview, images[pageIdx]))
                    preview.Dispose();
                preview = temp;
                // кешувати зображення
                images[pageIdx]?.Dispose();
                images[pageIdx] = preview;
                imageDpis[pageIdx] = requiredDpi;

                if (boxes_pages[pageIdx]?.Trimbox != null)
                {
                    double angle = boxes_pages[pageIdx].Rotate;
                    wMM = boxes_pages[pageIdx].Trimbox.wMM(angle);
                    hMM = boxes_pages[pageIdx].Trimbox.hMM(angle);
                }

                return new Tuple<Image, double, double>(preview, wMM, hMM);
            }

            return null;
        }

        private static int GetRequiredPreviewDpi(string ext, PdfPageInfo pageInfo, PdfPreviewParameters parameters)
        {
            const int defaultDpi = 150;
            const double smallPdfLongSideMm = 60.0;
            const int minTargetLongSidePixels = 512;
            const int maxTargetLongSidePixels = 2400;

            if (ext != ".pdf" && ext != ".ai")
                return defaultDpi;

            if (pageInfo?.Trimbox == null)
                return defaultDpi;

            double angle = pageInfo.Rotate;
            double longSideMm = Math.Max(pageInfo.Trimbox.wMM(angle), pageInfo.Trimbox.hMM(angle));
            if (longSideMm <= 0 || longSideMm > smallPdfLongSideMm)
                return defaultDpi;

            int targetLongSidePixels = parameters?.PreviewTargetLongSidePixels ?? 0;
            targetLongSidePixels = Math.Max(minTargetLongSidePixels, Math.Min(maxTargetLongSidePixels, targetLongSidePixels));

            int dpi = (int)Math.Ceiling(targetLongSidePixels * 25.4 / longSideMm);
            return Math.Max(defaultDpi, dpi);
        }

        public PdfPageInfo GetPageInfo(int pageNo)
        {
            return boxes_pages != null ? boxes_pages[pageNo - 1] : null;
        }


    }
}
