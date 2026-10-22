using Interfaces;
using JobSpace.Static;
using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenPdf : IScreenPrimitive, IDisposable
    {

        IFileSystemInfoExt _file;
        int _pageNo;
        Image image;
        PdfPageInfo info;

        public float X { get; set; } = -1;
        public float Y { get; set; } = -1;
        public float Angle { get; set; } = 0;

        public ScreenPdf(IFileSystemInfoExt file, int pageNo)
        {
            _file = file;
            _pageNo = pageNo;
            info = PdfHelper.GetPageInfo(_file.FullName);
            image = PdfHelper.RenderByTrimBox(_file.FullName, pageNo - 1);
        }

        public ScreenPdf(string filePath, int pageNo)
        {
            _file = new FileInfo(filePath).ToFileSystemInfoExt();
            _pageNo = pageNo;
            info = PdfHelper.GetPageInfo(filePath);
            image = PdfHelper.RenderByTrimBox(_file.FullName, pageNo - 1);
        }

        public void Dispose()
        {
            image?.Dispose();
        }

        public void Draw(Graphics g)
        {
            float w = (float)info.Trimbox.wMM();
            float h = (float)info.Trimbox.hMM();

            if (X < 0)
            {
                // намалювати image по центру
                X = (g.VisibleClipBounds.Width - w) / 2;
            }
            if (Y < 0)
            {
                var y = (g.VisibleClipBounds.Height - h) / 2;
            }
            // 1. Зберігаємо поточний стан графіки
            var saveState = g.Save();

            // 2. Обчислюємо центр об'єкта для повороту
            float centerX = X + w / 2;
            float centerY = Y + h / 2;

            // 3. Переміщуємо координати до центру
            g.TranslateTransform(centerX, centerY);

            // 4. Застосовуємо поворот
            g.RotateTransform(Angle);

            // 5. Малюємо. 
            // Оскільки ми змістили координати до центру, малюємо відносно -w/2 та -h/2
            g.FillRectangle(Brushes.White, -w / 2, -h / 2, w, h);
            g.DrawImage(image, -w / 2, -h / 2, w, h);

            // 6. Відновлюємо стан графіки до того, як ми викликали Save()
            g.Restore(saveState);
        }
    }
}
