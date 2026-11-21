using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Services
{
    /// <summary>
    /// Механізм компонування, координатної прив'язки та взаємодії з мишкою для квартального календаря.
    /// Усі розрахунки ведуться у фізичних міліметрах.
    /// </summary>
    public static class CalendarLayoutEngine
    {
        /// <summary>
        /// Суворе вертикальне компонування календаря:
        /// - Шапка і модулі йдуть строго вертикально зверху вниз одне під одним без проміжків.
        /// - Календарні блоки кріпляться до своєї підкладки строго TOP CENTER.
        /// - Користувач не змінює координати мишкою, вони розраховуються автоматично.
        /// </summary>
        public static void ApplyStrictVerticalLayout(CalendarConfiguration config)
        {
            if (config == null) return;

            // 1. Визначаємо базову ширину для центрування (максимальна ширина серед шапки, підкладок та блоків)
            double maxWidth = config.Header.WidthMm;
            foreach (var mod in config.Modules)
            {
                if (mod.Backing.WidthMm > maxWidth) maxWidth = mod.Backing.WidthMm;
                if (mod.CalendarBlock.WidthMm > maxWidth) maxWidth = mod.CalendarBlock.WidthMm;
            }

            if (maxWidth <= 0) maxWidth = 297.0; // стандартний A4/квартальний за замовчуванням

            double currentY = 0.0;

            // 2. Шапка в самому верху календаря, центрована по ширині
            double headerW = config.Header.WidthMm > 0 ? config.Header.WidthMm : maxWidth;
            double headerH = config.Header.HeightMm > 0 ? config.Header.HeightMm : 0;

            config.Header.OffsetXMm = Math.Max(0, (maxWidth - headerW) / 2.0);
            config.Header.OffsetYMm = currentY;

            currentY += headerH;

            // 3. Модулі календаря йдуть строго вертикально зверху вниз одне під одним
            foreach (var mod in config.Modules)
            {
                double backingW = mod.Backing.WidthMm > 0 ? mod.Backing.WidthMm : maxWidth;
                double backingH = mod.Backing.HeightMm > 0 ? mod.Backing.HeightMm : (mod.CalendarBlock.HeightMm > 0 ? mod.CalendarBlock.HeightMm : 100.0);

                // Підкладка розміщується строго за попередньою секцією
                mod.Backing.OffsetXMm = Math.Max(0, (maxWidth - backingW) / 2.0);
                mod.Backing.OffsetYMm = currentY;

                // Календарний блок кріпиться до своєї підкладки TOP CENTER (п. вимог):
                // Горизонтально — строго по центру підкладки:
                double blockW = mod.CalendarBlock.WidthMm > 0 ? mod.CalendarBlock.WidthMm : backingW;
                mod.CalendarBlock.OffsetXMm = mod.Backing.OffsetXMm + Math.Max(0, (backingW - blockW) / 2.0);

                // Вертикально — строго у верхній край підкладки (TOP):
                mod.CalendarBlock.OffsetYMm = mod.Backing.OffsetYMm;

                // Наступний модуль починається після завершення поточної секції
                double sectionH = Math.Max(backingH, mod.CalendarBlock.HeightMm);
                currentY += sectionH;
            }

            config.RecalculateBounds();
        }

        /// <summary>
        /// Автоматичне вирівнювання всієї конструкції (викликає суворе вертикальне компонування).
        /// </summary>
        public static void AutoLayoutTopToBottom(CalendarConfiguration config, double springGapMm = 0)
        {
            ApplyStrictVerticalLayout(config);
        }

        /// <summary>
        /// Конвертація екранних координат (пікселі) у фізичні координати полотна (міліметри).
        /// </summary>
        public static PointF ScreenToMm(Point screenPt, float zoomFactor, PointF panOffsetPx, float screenDpi = 96f)
        {
            if (zoomFactor <= 0.001f) zoomFactor = 1.0f;

            // 1. Віднімаємо екранне зміщення панорамування
            float canvasPxX = screenPt.X - panOffsetPx.X;
            float canvasPxY = screenPt.Y - panOffsetPx.Y;

            // 2. Враховуємо масштаб
            float unscaledPxX = canvasPxX / zoomFactor;
            float unscaledPxY = canvasPxY / zoomFactor;

            // 3. Переводимо пікселі в міліметри (1 inch = 25.4 mm)
            float mmX = unscaledPxX * 25.4f / screenDpi;
            float mmY = unscaledPxY * 25.4f / screenDpi;

            return new PointF(mmX, mmY);
        }

        /// <summary>
        /// Конвертація фізичних координат полотна (міліметри) в екранні координати (пікселі).
        /// </summary>
        public static Point MmToScreen(PointF mmPt, float zoomFactor, PointF panOffsetPx, float screenDpi = 96f)
        {
            // 1. Переводимо міліметри в пікселі
            float pxX = mmPt.X * screenDpi / 25.4f;
            float pxY = mmPt.Y * screenDpi / 25.4f;

            // 2. Враховуємо масштаб і зміщення
            float screenX = pxX * zoomFactor + panOffsetPx.X;
            float screenY = pxY * zoomFactor + panOffsetPx.Y;

            return new Point((int)Math.Round(screenX), (int)Math.Round(screenY));
        }

        /// <summary>
        /// Отримати прямокутник компонента в екранних пікселях.
        /// </summary>
        public static RectangleF GetComponentScreenRect(CalendarComponent component, float zoomFactor, PointF panOffsetPx, float screenDpi = 96f)
        {
            var topLeft = MmToScreen(new PointF((float)component.OffsetXMm, (float)component.OffsetYMm), zoomFactor, panOffsetPx, screenDpi);

            float widthPx = (float)(component.WidthMm * screenDpi / 25.4) * zoomFactor;
            float heightPx = (float)(component.HeightMm * screenDpi / 25.4) * zoomFactor;

            return new RectangleF(topLeft.X, topLeft.Y, widthPx, heightPx);
        }

        /// <summary>
        /// Визначення компонента під курсором миші (Hit-Test).
        /// Пошук іде у зворотному порядку Z-Index (спочатку верхні шари — блоки, потім підкладки, потім шапка).
        /// </summary>
        public static CalendarComponent? HitTest(CalendarConfiguration config, PointF mouseMm)
        {
            if (config == null) return null;

            // 1. Спочатку перевіряємо календарні блоки (найвищий шар серед паперових елементів)
            for (int i = config.Modules.Count - 1; i >= 0; i--)
            {
                var block = config.Modules[i].CalendarBlock;
                if (IsPointInside(block, mouseMm))
                    return block;
            }

            // 2. Далі перевіряємо підкладки
            for (int i = config.Modules.Count - 1; i >= 0; i--)
            {
                var backing = config.Modules[i].Backing;
                if (IsPointInside(backing, mouseMm))
                    return backing;
            }

            // 3. Шапка
            if (IsPointInside(config.Header, mouseMm))
                return config.Header;

            return null;
        }

        private static bool IsPointInside(CalendarComponent component, PointF ptMm)
        {
            if (component.WidthMm <= 0 || component.HeightMm <= 0)
                return false;

            return ptMm.X >= component.OffsetXMm &&
                   ptMm.X <= (component.OffsetXMm + component.WidthMm) &&
                   ptMm.Y >= component.OffsetYMm &&
                   ptMm.Y <= (component.OffsetYMm + component.HeightMm);
        }

        /// <summary>
        /// Отримати порядок малювання компонентів за Z-Index (від нижнього до верхнього).
        /// </summary>
        public static List<CalendarComponent> GetZOrderedComponents(CalendarConfiguration config)
        {
            var list = new List<CalendarComponent>();

            // 1. Підкладки модулів (нижній шар)
            foreach (var mod in config.Modules)
            {
                list.Add(mod.Backing);
            }

            // 2. Календарні блоки (поверх підкладок)
            foreach (var mod in config.Modules)
            {
                list.Add(mod.CalendarBlock);
            }

            // 3. Шапка календаря
            list.Add(config.Header);

            return list;
        }
    }
}
