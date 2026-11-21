using System;
using System.Drawing;
using System.Text.Json.Serialization;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models
{
    /// <summary>
    /// Базова модель PDF-компонента конструктора квартального календаря (шапка, підкладка або календарний блок).
    /// Усі розміри та координати зберігаються у фізичних одиницях — міліметрах (мм).
    /// </summary>
    public class CalendarComponent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Відображувана назва компонента (наприклад, "Шапка", "Підкладка 1", "Блок 1").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Повний шлях до вихідного PDF-файлу.
        /// </summary>
        public string? FilePath { get; set; }

        /// <summary>
        /// Індекс вибраної сторінки PDF (0-based: 0 = перша сторінка).
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// Фізична ширина компонента у міліметрах (визначається за TrimBox).
        /// </summary>
        public double WidthMm { get; set; } = 0.0;

        /// <summary>
        /// Фізична висота компонента у міліметрах (визначається за TrimBox).
        /// </summary>
        public double HeightMm { get; set; } = 0.0;

        /// <summary>
        /// Ознака того, що розміри отримано саме з явного TrimBox у PDF, а не за допомогою fallback.
        /// </summary>
        public bool HasExplicitTrimBox { get; set; } = false;

        /// <summary>
        /// Зміщення компонента по осі X у міліметрах на віртуальному полотні календаря.
        /// </summary>
        public double OffsetXMm { get; set; } = 0.0;

        /// <summary>
        /// Зміщення компонента по осі Y у міліметрах на віртуальному полотні календаря.
        /// </summary>
        public double OffsetYMm { get; set; } = 0.0;

        /// <summary>
        /// Чи вибрано валідний файл.
        /// </summary>
        [JsonIgnore]
        public bool HasFile => !string.IsNullOrWhiteSpace(FilePath) && System.IO.File.Exists(FilePath);

        /// <summary>
        /// Кешоване растрове прев'ю сторінки для швидкого відображення в UI.
        /// Не серіалізується в JSON конфігурацію.
        /// </summary>
        [JsonIgnore]
        public Bitmap? CachedPreview { get; set; }

        /// <summary>
        /// Очищення кешованого зображення для звільнення ресурсів.
        /// </summary>
        public void ClearPreview()
        {
            CachedPreview?.Dispose();
            CachedPreview = null;
        }

        public override string ToString()
        {
            return $"{Name} ({WidthMm:0.#}x{HeightMm:0.#} мм)";
        }
    }
}
