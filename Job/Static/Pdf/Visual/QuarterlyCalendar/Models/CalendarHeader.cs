namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models
{
    /// <summary>
    /// Модель шапки квартального календаря (п. 3.1 ТЗ).
    /// Завжди одна у виробі, розташовується на самому верху.
    /// Підтримує два варіанти кріплення (п. 5 ТЗ):
    /// 1. Пружина зверху та знизу.
    /// 2. Люверс зверху та пружина знизу.
    /// </summary>
    public class CalendarHeader : CalendarComponent
    {
        public CalendarHeader()
        {
            Name = "Шапка календаря";
        }

        /// <summary>
        /// Варіант кріплення шапки.
        /// </summary>
        public HeaderMountType MountType { get; set; } = HeaderMountType.EyeletTopSpringBottom;

        /// <summary>
        /// Параметри люверса (використовується, якщо MountType == HeaderMountType.EyeletTopSpringBottom).
        /// </summary>
        public EyeletSettings Eyelet { get; set; } = new EyeletSettings();

        /// <summary>
        /// Верхня пружина (використовується, якщо MountType == HeaderMountType.SpringTopAndBottom).
        /// </summary>
        public SpiralItemConfig TopSpiral { get; set; } = new SpiralItemConfig();

        /// <summary>
        /// Нижня пружина для з'єднання шапки з підкладкою першого конструктивного модуля.
        /// </summary>
        public SpiralItemConfig BottomSpiral { get; set; } = new SpiralItemConfig();
    }
}
