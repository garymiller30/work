using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Services
{
    /// <summary>
    /// Сервіс графічного відображення пружинних кріплень та люверсів (розділи 4 та 5 ТЗ).
    /// Усі розрахунки та відмальовка виконуються у фізичних міліметрах (мм).
    /// </summary>
    public static class CalendarMountRenderService
    {
        // Кеш бітмапів ланок пружин та їхніх розмірів
        private static readonly ConcurrentDictionary<string, Tuple<Bitmap, double, double>> _spiralCache = new();

        /// <summary>
        /// Отримати шлях до стандартної пружини проєкту за замовчуванням.
        /// </summary>
        public static string? GetDefaultSpiralPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string defaultPath = Path.Combine(baseDir, "db", "spirals", "spiral_10mm.pdf");
            if (File.Exists(defaultPath))
                return defaultPath;

            // Пошук у можливих суміжних каталогах
            string altPath = Path.Combine(Directory.GetCurrentDirectory(), "ActiveWorks", "db", "spirals", "spiral_10mm.pdf");
            if (File.Exists(altPath))
                return altPath;

            return null;
        }

        /// <summary>
        /// Отримати растрове зображення ланки пружини та її розміри у мм (TrimBox).
        /// </summary>
        public static Tuple<Bitmap, double, double>? GetSpiralResource(string? filePath)
        {
            string? targetFile = filePath;
            if (string.IsNullOrWhiteSpace(targetFile) || !File.Exists(targetFile))
            {
                targetFile = GetDefaultSpiralPath();
            }

            if (string.IsNullOrWhiteSpace(targetFile) || !File.Exists(targetFile))
                return null;

            if (_spiralCache.TryGetValue(targetFile, out var cached))
                return cached;

            try
            {
                var pageInfo = PdfHelper.GetPageInfo(targetFile, 0);
                if (pageInfo?.Trimbox == null) return null;

                double stepMm = pageInfo.Trimbox.wMM();
                double heightMm = pageInfo.Trimbox.hMM();

                var bmp = PdfHelper.RenderByTrimBox(targetFile, 0, 150);
                if (bmp != null)
                {
                    // Об'єднуємо дві половини в одну цілісну пружину
                    var fullBmp = CombineSpiralHalves(bmp);
                    bmp.Dispose();

                    double fullHeightMm = heightMm * 2.0;
                    var item = Tuple.Create(fullBmp, stepMm, fullHeightMm);
                    _spiralCache[targetFile] = item;
                    return item;
                }
            }
            catch
            {
                // Помилка завантаження ресурсу пружини
            }

            return null;
        }

        /// <summary>
        /// З'єднати дві дзеркальні половини пружини в одну повну цілісну ланку.
        /// Файл пружини містить тільки половину витка; з'єднуючи його з дзеркальною копією,
        /// отримуємо повноцінне кільце/петлю пружини.
        /// </summary>
        private static Bitmap CombineSpiralHalves(Bitmap halfBmp)
        {
            int w = halfBmp.Width;
            int h = halfBmp.Height;

            // Створюємо повнорозмірний бітмап для цілої пружини з прозорим фоном
            var fullBmp = new Bitmap(w, h * 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            fullBmp.SetResolution(halfBmp.HorizontalResolution, halfBmp.VerticalResolution);

            using (var g = Graphics.FromImage(fullBmp))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 1. Верхня половина пружини: розвернута на 180 градусів, щоб отвори були зверху (ззовні)
                using (var upperHalf = new Bitmap(halfBmp))
                {
                    upperHalf.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(upperHalf, new Rectangle(0, 0, w, h));
                }

                // 2. Нижня половина пружини: розвернута так, щоб отвори були знизу (ззовні)
                using (var lowerHalf = new Bitmap(halfBmp))
                {
                    lowerHalf.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(lowerHalf, new Rectangle(0, h, w, h));
                }
            }

            return fullBmp;
        }

        /// <summary>
        /// Відмалювати пружинне кріплення горизонтального типу по заданій ширині та координатах.
        /// </summary>
        /// <param name="g">Графіка полотна (в одиницях мм).</param>
        /// <param name="spiralConfig">Конфігурація пружини.</param>
        /// <param name="xMm">Координата X лівого краю елемента.</param>
        /// <param name="yMm">Координата Y лінії кріплення (центр пружини).</param>
        /// <param name="widthMm">Ширина зони перфорації/кріплення у мм.</param>
        public static void RenderHorizontalSpiral(Graphics g, SpiralItemConfig spiralConfig, double xMm, double yMm, double widthMm)
        {
            if (spiralConfig == null || !spiralConfig.Enabled || widthMm <= 0)
                return;

            var spiralRes = GetSpiralResource(spiralConfig.SpiralFilePath);

            if (spiralRes != null)
            {
                // Малювання пружини на основі растрового PDF-еталону (алгоритм DrawSpiral.cs)
                Bitmap bmp = spiralRes.Item1;
                double stepMm = spiralRes.Item2 > 0 ? spiralRes.Item2 : 8.47; // стандартний крок 3:1 (8.47 мм)
                double heightMm = spiralRes.Item3 > 0 ? spiralRes.Item3 : spiralConfig.DiameterMm;

                int cntHoles = (int)(widthMm / stepMm);
                if (cntHoles <= 0) cntHoles = 1;

                double startX = xMm + (widthMm - (stepMm * cntHoles)) / 2.0;
                double topY = yMm - heightMm / 2.0;

                for (int i = 0; i < cntHoles; i++)
                {
                    float holeX = (float)(startX + i * stepMm);
                    g.DrawImage(bmp, holeX, (float)topY, (float)stepMm, (float)heightMm);
                }
            }
            else
            {
                // Запасний процедурний рендеринг реалістичної металевої пружини (якщо файл пружини відсутній)
                RenderProceduralSpiral(g, xMm, yMm, widthMm, spiralConfig.DiameterMm);
            }
        }

        /// <summary>
        /// Процедурний векторний рендеринг реалістичної пружини та перфорації.
        /// </summary>
        private static void RenderProceduralSpiral(Graphics g, double xMm, double yMm, double widthMm, double diameterMm)
        {
            double stepMm = 8.47; // Крок 3:1
            if (diameterMm <= 0) diameterMm = 10.0;
            double heightMm = diameterMm;

            int cnt = (int)(widthMm / stepMm);
            if (cnt <= 0) cnt = 1;

            double startX = xMm + (widthMm - (stepMm * cnt)) / 2.0;
            double topY = yMm - heightMm / 2.0;

            float holeW = 4.0f;
            float holeH = 4.0f;

            using (var holeBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
            using (var wirePen = new Pen(Color.FromArgb(200, 205, 210), 1.6f))
            using (var wireShadowPen = new Pen(Color.FromArgb(120, 125, 130), 1.6f))
            {
                for (int i = 0; i < cnt; i++)
                {
                    float hx = (float)(startX + i * stepMm + (stepMm - holeW) / 2.0);
                    float hy = (float)yMm - holeH / 2.0f;

                    // Квадратні отвори перфорації
                    g.FillRectangle(holeBrush, hx, hy, holeW, holeH);

                    // Петля дротяної пружини
                    float wx = (float)(startX + i * stepMm);
                    g.DrawArc(wireShadowPen, wx, (float)topY, (float)stepMm, (float)heightMm, 180, 180);
                    g.DrawArc(wirePen, wx, (float)topY, (float)stepMm, (float)heightMm, 0, 180);
                }
            }
        }

        /// <summary>
        /// Відмалювати люверс у верхній частині шапки:
        /// - Центрування по горизонталі шапки (з урахуванням OffsetXMm).
        /// - Відстань від верхнього краю шапки до верхнього краю люверса задається користувачем (за замовчуванням 5 мм).
        /// - Діаметр отвору люверса — від 3 до 5 мм.
        /// </summary>
        public static void RenderEyelet(Graphics g, EyeletSettings eyelet, double headerXMm, double headerYMm, double headerWidthMm)
        {
            if (eyelet == null || !eyelet.Enabled)
                return;

            float radius = (float)(eyelet.DiameterMm / 2.0);
            float rimThickness = 1.0f; // металевий обідок люверса товщиною 1 мм
            float outerRadius = radius + rimThickness;

            float centerX = (float)(headerXMm + headerWidthMm / 2.0 + eyelet.OffsetXMm);
            // Відстань від краю шапки до верхнього краю люверса = eyelet.TopMarginMm
            float centerY = (float)(headerYMm + eyelet.TopMarginMm + outerRadius);

            var oldSmoothing = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Металевий обідок люверса (латунь/срібло з металевим градієнтом)
            var outerRect = new RectangleF(centerX - outerRadius, centerY - outerRadius, outerRadius * 2, outerRadius * 2);
            using (var rimBrush = new LinearGradientBrush(outerRect, Color.FromArgb(235, 235, 240), Color.FromArgb(160, 165, 175), 45f))
            {
                g.FillEllipse(rimBrush, outerRect);
            }

            // Тонкий контур зовнішнього металевого кільця
            using (var rimBorderPen = new Pen(Color.FromArgb(120, 125, 135), 0.2f))
            {
                g.DrawEllipse(rimBorderPen, outerRect);
            }

            // 2. Внутрішній отвір люверса (просічка в папері)
            var innerRect = new RectangleF(centerX - radius, centerY - radius, radius * 2, radius * 2);
            using (var holeBrush = new SolidBrush(Color.FromArgb(40, 40, 45)))
            {
                g.FillEllipse(holeBrush, innerRect);
            }

            // Внутрішня тінь всередині отвору
            using (var innerShadowPen = new Pen(Color.FromArgb(20, 20, 25), 0.25f))
            {
                g.DrawEllipse(innerShadowPen, innerRect);
            }

            // Легкий відблиск на металевому кільці зверху зліва
            using (var highlightPen = new Pen(Color.FromArgb(180, 255, 255, 255), 0.35f))
            {
                g.DrawArc(highlightPen, centerX - outerRadius + 0.3f, centerY - outerRadius + 0.3f, outerRadius * 2 - 0.6f, outerRadius * 2 - 0.6f, 135, 90);
            }

            g.SmoothingMode = oldSmoothing;
        }

        /// <summary>
        /// Відмалювати всі кріплення календаря: люверс/верхню пружину на шапці та внутрішні пружини календарних блоків (Top Center).
        /// Пружини між секціями/модулями вимкнено за вимогами компонування.
        /// </summary>
        public static void RenderAllMounts(Graphics g, CalendarConfiguration config)
        {
            if (config == null) return;

            var header = config.Header;

            // 1. Верхнє кріплення шапки (люверс або пружина)
            if (header.WidthMm > 0 && header.HeightMm > 0)
            {
                if (header.MountType == HeaderMountType.EyeletTopSpringBottom)
                {
                    // Варіант 2: Люверс зверху
                    RenderEyelet(g, header.Eyelet, header.OffsetXMm, header.OffsetYMm, header.WidthMm);
                }
                else
                {
                    // Варіант 1: Пружина зверху
                    RenderHorizontalSpiral(g, header.TopSpiral, header.OffsetXMm, header.OffsetYMm, header.WidthMm);
                }
            }

            // 2. Внутрішні пружини кожного модуля: календарний блок кріпиться пружиною до своєї підкладки (Top Center)
            for (int i = 0; i < config.Modules.Count; i++)
            {
                var mod = config.Modules[i];

                if (mod.CalendarBlock.WidthMm > 0 && mod.CalendarBlock.HeightMm > 0)
                {
                    // Пружина розташовується у верхній частині календарного блоку
                    double blockSpringY = mod.CalendarBlock.OffsetYMm;
                    RenderHorizontalSpiral(g, mod.InternalSpiral, mod.CalendarBlock.OffsetXMm, blockSpringY, mod.CalendarBlock.WidthMm);
                }
            }
        }
    }
}
