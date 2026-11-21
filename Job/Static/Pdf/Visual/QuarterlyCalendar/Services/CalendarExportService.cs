using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Services
{
    /// <summary>
    /// Сервіс високоякісного експорту готової візуалізації квартального календаря (п. 11 ТЗ).
    /// Рендерить всю конструкцію цілком на основі фізичних розмірів у заданому DPI (150, 300 або 600).
    /// </summary>
    public static class CalendarExportService
    {
        /// <summary>
        /// Згенерувати повнорозмірне растрове зображення календаря високої роздільної здатності.
        /// </summary>
        /// <param name="config">Конфігурація календаря.</param>
        /// <param name="dpi">Роздільна здатність: 150, 300 або 600 DPI.</param>
        /// <param name="transparentBackground">Чи залишати прозорий фон (тільки для PNG).</param>
        public static Bitmap GenerateExportBitmap(CalendarConfiguration config, int dpi = 300, bool transparentBackground = false)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (dpi != 150 && dpi != 300 && dpi != 600)
                dpi = 300;

            // 1. Оновлюємо та отримуємо габарити сцени
            config.RecalculateBounds();

            var activeComponents = config.GetAllComponents()
                .Where(c => c.WidthMm > 0 && c.HeightMm > 0)
                .ToList();

            double minX = activeComponents.Count > 0 ? activeComponents.Min(c => c.OffsetXMm) : 0;
            double minY = activeComponents.Count > 0 ? activeComponents.Min(c => c.OffsetYMm) : 0;
            double totalWidthMm = config.TotalWidthMm > 0 ? config.TotalWidthMm : 297.0;
            double totalHeightMm = config.TotalHeightMm > 0 ? config.TotalHeightMm : 840.0;

            // Додаємо невеликі поля 5 мм навколо виробу
            double paddingMm = 5.0;
            double canvasWidthMm = totalWidthMm + paddingMm * 2;
            double canvasHeightMm = totalHeightMm + paddingMm * 2;

            int widthPx = (int)Math.Ceiling(canvasWidthMm * dpi / 25.4);
            int heightPx = (int)Math.Ceiling(canvasHeightMm * dpi / 25.4);

            var bitmap = new Bitmap(widthPx, heightPx, PixelFormat.Format32bppArgb);
            bitmap.SetResolution(dpi, dpi);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                if (transparentBackground)
                {
                    g.Clear(Color.Transparent);
                }
                else
                {
                    g.Clear(Color.White);
                }

                // Масштабуємо контекст малювання у фізичні міліметри з урахуванням зсуву
                float scale = dpi / 25.4f;
                g.ScaleTransform(scale, scale);
                g.TranslateTransform((float)(paddingMm - minX), (float)(paddingMm - minY));

                // 2. Додаємо м'яку тінь під усім календарем, щоб білий виріб чітко контрастував із білим фоном
                if (!transparentBackground)
                {
                    CalendarRenderService.RenderCalendarDropShadow(g, config);
                }

                // 3. Для якісного експорту генеруємо бітмапи PDF-сторінок з високою роздільною здатністю (target DPI)
                foreach (var comp in activeComponents)
                {
                    // Якщо це календарний блок, малюємо м'яку тінь під його низом на підкладку
                    if (comp.Name.Contains("блок", StringComparison.OrdinalIgnoreCase) && comp.WidthMm > 0 && comp.HeightMm > 0)
                    {
                        CalendarRenderService.RenderBlockBottomShadow(g, comp);
                    }

                    if (comp.HasFile)
                    {
                        try
                        {
                            var hiResBmp = PdfHelper.RenderByTrimBox(comp.FilePath!, comp.PageIndex, dpi);
                            if (hiResBmp != null)
                            {
                                float x = (float)comp.OffsetXMm;
                                float y = (float)comp.OffsetYMm;
                                float w = (float)comp.WidthMm;
                                float h = (float)comp.HeightMm;

                                g.FillRectangle(Brushes.White, x, y, w, h);
                                g.DrawImage(hiResBmp, x, y, w, h);
                                hiResBmp.Dispose();
                            }
                        }
                        catch
                        {
                            // У разі збою використовуємо звичайний рендер компонента
                            CalendarRenderService.RenderComponent(g, comp, isScreenPreview: false);
                        }
                    }
                    else
                    {
                        CalendarRenderService.RenderComponent(g, comp, isScreenPreview: false);
                    }
                }

                // 4. Малюємо всі пружинні кріплення та люверс
                CalendarMountRenderService.RenderAllMounts(g, config);
            }

            return bitmap;
        }

        /// <summary>
        /// Зберегти готове зображення у файл за вказаним шляхом і параметрами.
        /// </summary>
        public static void SaveExportImage(
            CalendarConfiguration config,
            string targetFilePath,
            string format = "PNG",
            int dpi = 300,
            bool transparentBackground = false)
        {
            if (string.IsNullOrWhiteSpace(targetFilePath))
                throw new ArgumentException("Шлях збереження порожній.", nameof(targetFilePath));

            bool isPng = string.Equals(format, "PNG", StringComparison.OrdinalIgnoreCase) ||
                         targetFilePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase);

            using var bmp = GenerateExportBitmap(config, dpi, isPng && transparentBackground);

            if (isPng)
            {
                bmp.Save(targetFilePath, ImageFormat.Png);
            }
            else
            {
                // JPEG зберігаємо з високою якістю (95%)
                var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                if (jpegEncoder != null)
                {
                    using var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 95L);
                    bmp.Save(targetFilePath, jpegEncoder, encoderParams);
                }
                else
                {
                    bmp.Save(targetFilePath, ImageFormat.Jpeg);
                }
            }
        }

        private static ImageCodecInfo? GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
