using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Services
{
    /// <summary>
    /// Сервіс рендерингу композиції квартального календаря на полотні (Graphics) у фізичних одиницях (мм).
    /// </summary>
    public static class CalendarRenderService
    {
        /// <summary>
        /// Відмалювати всю композицію календаря на Graphics.
        /// Графіка має перебувати у системі координат полотна (у міліметрах або з відповідним ScaleTransform).
        /// </summary>
        public static void RenderCalendarScene(
            Graphics g,
            CalendarConfiguration config,
            CalendarComponent? selectedComponent = null,
            bool isScreenPreview = true)
        {
            if (config == null) return;

            var oldSmoothing = g.SmoothingMode;
            var oldInterpolation = g.InterpolationMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // 1. М'яка зовнішня тінь під усією конструкцією календаря
            RenderCalendarDropShadow(g, config);

            // 2. Малюємо підкладки всіх модулів (нижній шар паперових елементів)
            foreach (var mod in config.Modules)
            {
                RenderComponent(g, mod.Backing, isScreenPreview);
            }

            // 3. Малюємо календарні блоки (з м'якою тінню під низом блоку, що падає на підкладку)
            foreach (var mod in config.Modules)
            {
                if (mod.CalendarBlock.WidthMm > 0 && mod.CalendarBlock.HeightMm > 0)
                {
                    RenderBlockBottomShadow(g, mod.CalendarBlock);
                }
                RenderComponent(g, mod.CalendarBlock, isScreenPreview);
            }

            // 4. Малюємо шапку календаря
            RenderComponent(g, config.Header, isScreenPreview);

            // 5. Малюємо всі пружинні кріплення та люверс (розділи 4 та 5 ТЗ)
            CalendarMountRenderService.RenderAllMounts(g, config);

            // 6. Якщо це екранний режим і є вибраний компонент — малюємо рамку виділення
            if (isScreenPreview && selectedComponent != null && selectedComponent.WidthMm > 0 && selectedComponent.HeightMm > 0)
            {
                RenderSelectionOutline(g, selectedComponent);
            }

            g.SmoothingMode = oldSmoothing;
            g.InterpolationMode = oldInterpolation;
        }

        /// <summary>
        /// Відмалювати реалістичну м'яку тінь під нижнім зрізом календарного блока, що падає на підкладку.
        /// </summary>
        public static void RenderBlockBottomShadow(Graphics g, CalendarComponent block, float shadowHeightMm = 3.2f)
        {
            if (block == null || block.WidthMm <= 0 || block.HeightMm <= 0)
                return;

            float x = (float)block.OffsetXMm;
            float y = (float)block.OffsetYMm;
            float w = (float)block.WidthMm;
            float h = (float)block.HeightMm;

            // Нижня горизонтальна тінь від стопки аркушів календарного блоку
            var shadowRect = new RectangleF(x, y + h, w, shadowHeightMm);
            using (var brush = new LinearGradientBrush(shadowRect, Color.FromArgb(80, 0, 0, 0), Color.Transparent, LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, shadowRect);
            }

            // Легка бічна вертикальна тінь з правого боку календарного блоку
            float sideShadowW = 1.6f;
            var sideShadowRect = new RectangleF(x + w, y + 1.0f, sideShadowW, h + shadowHeightMm - 1.0f);
            using (var sideBrush = new LinearGradientBrush(sideShadowRect, Color.FromArgb(45, 0, 0, 0), Color.Transparent, LinearGradientMode.Horizontal))
            {
                g.FillRectangle(sideBrush, sideShadowRect);
            }
        }

        /// <summary>
        /// Відмалювати об'ємну м'яку тінь під усією площею календаря, щоб виріб не зливався з білим фоном.
        /// </summary>
        public static void RenderCalendarDropShadow(Graphics g, CalendarConfiguration config)
        {
            if (config == null) return;

            // Збираємо прямокутники шапки та підкладок модулів
            var rects = new System.Collections.Generic.List<RectangleF>();

            float hw = (float)(config.Header.WidthMm > 0 ? config.Header.WidthMm : 297.0);
            float hh = (float)(config.Header.HeightMm > 0 ? config.Header.HeightMm : 100.0);
            rects.Add(new RectangleF((float)config.Header.OffsetXMm, (float)config.Header.OffsetYMm, hw, hh));

            foreach (var mod in config.Modules)
            {
                float bw = (float)(mod.Backing.WidthMm > 0 ? mod.Backing.WidthMm : 297.0);
                float bh = (float)(mod.Backing.HeightMm > 0 ? mod.Backing.HeightMm : 100.0);
                rects.Add(new RectangleF((float)mod.Backing.OffsetXMm, (float)mod.Backing.OffsetYMm, bw, bh));
            }

            // Малюємо кілька напівпрозорих шарів тіні зі зсувом вправо і вниз для ефекту розмиття
            float[] offsets = { 1.0f, 2.0f, 3.2f, 4.5f };
            int[] alphas = { 28, 20, 14, 8 };

            for (int i = 0; i < offsets.Length; i++)
            {
                float off = offsets[i];
                using (var brush = new SolidBrush(Color.FromArgb(alphas[i], 0, 0, 0)))
                {
                    foreach (var r in rects)
                    {
                        g.FillRectangle(brush, r.X + off * 0.7f, r.Y + off, r.Width + off * 0.5f, r.Height);
                    }
                }
            }
        }

        /// <summary>
        /// Відмалювати окремий PDF-компонент календаря.
        /// </summary>
        public static void RenderComponent(Graphics g, CalendarComponent component, bool isScreenPreview)
        {
            if (component == null) return;

            float x = (float)component.OffsetXMm;
            float y = (float)component.OffsetYMm;
            float w = (float)component.WidthMm;
            float h = (float)component.HeightMm;

            if (w <= 0 || h <= 0)
            {
                // Якщо розміри ще не визначені файлом, малюємо плейсхолдер 297x100 для наочності
                w = 297f;
                h = 100f;
            }

            var rect = new RectangleF(x, y, w, h);

            // Легка тінь під компонентом для реалістичності в екранному режимі
            if (isScreenPreview)
            {
                using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                {
                    g.FillRectangle(shadowBrush, x + 1.2f, y + 1.2f, w, h);
                }
            }

            // Фонова заливка паперу (білий колір)
            using (var paperBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(paperBrush, rect);
            }

            // Малюємо вміст PDF-сторінки, якщо доступно прев'ю
            if (component.CachedPreview != null)
            {
                g.DrawImage(component.CachedPreview, rect);
            }
            else
            {
                // Якщо файлу ще немає або прев'ю не згенеровано — малюємо інформаційну заглушку
                using (var pen = new Pen(Color.FromArgb(180, 200, 220), 0.5f) { DashStyle = DashStyle.Dash })
                {
                    g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                }

                using (var font = new Font("Segoe UI", 3.5f, FontStyle.Regular, GraphicsUnit.Millimeter))
                using (var brush = new SolidBrush(Color.FromArgb(120, 140, 160)))
                {
                    var format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    string text = string.IsNullOrWhiteSpace(component.FilePath)
                        ? $"[{component.Name}]\nВиберіть PDF-файл"
                        : $"{component.Name}\n{System.IO.Path.GetFileName(component.FilePath)} (Стор. {component.PageIndex + 1})\n{w:0.#} x {h:0.#} мм";

                    g.DrawString(text, font, brush, rect, format);
                }
            }

            // Тонка межа аркуша
            using (var borderPen = new Pen(Color.FromArgb(180, 180, 180), 0.3f))
            {
                g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// Відмалювати рамку виділення навколо вибраного користувачем компонента.
        /// </summary>
        public static void RenderSelectionOutline(Graphics g, CalendarComponent component)
        {
            float x = (float)component.OffsetXMm;
            float y = (float)component.OffsetYMm;
            float w = (float)component.WidthMm;
            float h = (float)component.HeightMm;

            using (var selPen = new Pen(Color.FromArgb(0, 120, 215), 0.8f) { DashStyle = DashStyle.Dash })
            {
                g.DrawRectangle(selPen, x - 0.5f, y - 0.5f, w + 1f, h + 1f);
            }

            // Малюємо кутові маркери виділення
            float handleSize = 2.0f; // 2 мм
            using (var handleBrush = new SolidBrush(Color.FromArgb(0, 120, 215)))
            {
                g.FillRectangle(handleBrush, x - handleSize / 2, y - handleSize / 2, handleSize, handleSize);
                g.FillRectangle(handleBrush, x + w - handleSize / 2, y - handleSize / 2, handleSize, handleSize);
                g.FillRectangle(handleBrush, x - handleSize / 2, y + h - handleSize / 2, handleSize, handleSize);
                g.FillRectangle(handleBrush, x + w - handleSize / 2, y + h - handleSize / 2, handleSize, handleSize);
            }
        }
    }
}
