namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models
{
    /// <summary>
    /// Налаштування експорту готової візуалізації (п. 11 ТЗ).
    /// </summary>
    public class CalendarExportSettings
    {
        /// <summary>
        /// Формат експорту: "PNG" або "JPEG".
        /// </summary>
        public string Format { get; set; } = "PNG";

        /// <summary>
        /// Роздільна здатність експортованого растру: 150, 300 або 600 DPI.
        /// </summary>
        public int Dpi { get; set; } = 300;

        /// <summary>
        /// Чи експортувати зображення з прозорим фоном (актуально тільки для формату PNG).
        /// Для JPEG фон завжди заливається білим кольором.
        /// </summary>
        public bool TransparentBackground { get; set; } = false;

        /// <summary>
        /// Останній використаний шлях для збереження зображення.
        /// </summary>
        public string? LastExportFilePath { get; set; }
    }
}
