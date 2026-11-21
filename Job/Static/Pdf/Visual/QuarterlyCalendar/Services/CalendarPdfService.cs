using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models;
using PDFlib_dotnet;
using System;
using System.Drawing;
using System.IO;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Services
{
    /// <summary>
    /// Результат аналізу PDF-сторінки з інформацією про розміри та наявність TrimBox.
    /// </summary>
    public class PdfTrimBoxInfo
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }

        public bool HasTrimBox { get; set; }
        public double TrimWidthMm { get; set; }
        public double TrimHeightMm { get; set; }

        public double MediaWidthMm { get; set; }
        public double MediaHeightMm { get; set; }
    }

    /// <summary>
    /// Сервіс для роботи з PDF-файлами та їхніми фізичними розмірами у квартальному календарі.
    /// </summary>
    public static class CalendarPdfService
    {
        /// <summary>
        /// Отримати кількість сторінок у PDF-файлі.
        /// </summary>
        public static int GetPageCount(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return 0;

            try
            {
                return PdfHelper.GetPageCount(filePath);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Перевірити сторінку PDF та отримати фізичні розміри за TrimBox (п. 6.1 ТЗ).
        /// Якщо TrimBox відсутній, HasTrimBox = false, але повертаються розміри MediaBox для можливості вибору користувача.
        /// </summary>
        public static PdfTrimBoxInfo GetPageTrimBoxInfo(string filePath, int pageIndex)
        {
            var result = new PdfTrimBoxInfo
            {
                PageIndex = pageIndex
            };

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                result.Success = false;
                result.ErrorMessage = "Файл не знайдено або шлях порожній.";
                return result;
            }

            if (pageIndex < 0)
            {
                result.Success = false;
                result.ErrorMessage = "Некоректний номер сторінки.";
                return result;
            }

            try
            {
                using var p = new PDFlib();
                p.begin_document("", "");
                int indoc = p.open_pdi_document(filePath, "");
                int totalPages = (int)p.pcos_get_number(indoc, "length:pages");
                result.PageCount = totalPages;

                if (pageIndex >= totalPages)
                {
                    p.close_pdi_document(indoc);
                    result.Success = false;
                    result.ErrorMessage = $"Сторінка {pageIndex + 1} перевищує загальну кількість сторінок ({totalPages}).";
                    return result;
                }

                // Перевіряємо наявність явного TrimBox у PDF-сторінці
                string trimType = p.pcos_get_string(indoc, $"type:pages[{pageIndex}]/TrimBox");
                bool hasExplicitTrim = string.Equals(trimType, "array", StringComparison.OrdinalIgnoreCase);
                result.HasTrimBox = hasExplicitTrim;

                // Читаємо MediaBox
                var mediaRaw = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    mediaRaw[i] = p.pcos_get_number(indoc, $"pages[{pageIndex}]/MediaBox[{i}]");
                }
                double mediaW = Math.Abs(mediaRaw[2] - mediaRaw[0]);
                double mediaH = Math.Abs(mediaRaw[3] - mediaRaw[1]);
                result.MediaWidthMm = Math.Round(mediaW / PdfHelper.mn, 1);
                result.MediaHeightMm = Math.Round(mediaH / PdfHelper.mn, 1);

                if (hasExplicitTrim)
                {
                    var trimRaw = new double[4];
                    for (int i = 0; i < 4; i++)
                    {
                        trimRaw[i] = p.pcos_get_number(indoc, $"pages[{pageIndex}]/TrimBox[{i}]");
                    }
                    double trimW = Math.Abs(trimRaw[2] - trimRaw[0]);
                    double trimH = Math.Abs(trimRaw[3] - trimRaw[1]);
                    result.TrimWidthMm = Math.Round(trimW / PdfHelper.mn, 1);
                    result.TrimHeightMm = Math.Round(trimH / PdfHelper.mn, 1);
                }
                else
                {
                    // Якщо явного TrimBox немає, надаємо розміри MediaBox як альтернативу
                    result.TrimWidthMm = result.MediaWidthMm;
                    result.TrimHeightMm = result.MediaHeightMm;
                }

                p.close_pdi_document(indoc);
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Оновити розміри компонента на основі PDF-файлу та номера сторінки.
        /// Повертає false, якщо у PDF відсутній явний TrimBox (для сповіщення користувача).
        /// </summary>
        public static bool UpdateComponentDimensions(CalendarComponent component)
        {
            if (string.IsNullOrWhiteSpace(component.FilePath) || !File.Exists(component.FilePath))
            {
                component.WidthMm = 0;
                component.HeightMm = 0;
                component.HasExplicitTrimBox = false;
                component.ClearPreview();
                return false;
            }

            var info = GetPageTrimBoxInfo(component.FilePath, component.PageIndex);
            if (!info.Success)
            {
                component.ClearPreview();
                return false;
            }

            component.WidthMm = info.TrimWidthMm;
            component.HeightMm = info.TrimHeightMm;
            component.HasExplicitTrimBox = info.HasTrimBox;

            return info.HasTrimBox;
        }

        /// <summary>
        /// Рендеринг прев'ю PDF-сторінки по TrimBox для відображення в інтерфейсі (за замовчуванням 150 DPI).
        /// </summary>
        public static Bitmap? RenderComponentPreview(CalendarComponent component, int dpi = 150)
        {
            if (string.IsNullOrWhiteSpace(component.FilePath) || !File.Exists(component.FilePath))
                return null;

            try
            {
                component.ClearPreview();
                var bmp = PdfHelper.RenderByTrimBox(component.FilePath, component.PageIndex, dpi);
                component.CachedPreview = bmp;
                return bmp;
            }
            catch
            {
                return null;
            }
        }
    }
}
