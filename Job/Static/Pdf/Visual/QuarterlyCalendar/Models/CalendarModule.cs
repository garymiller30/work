using System;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models
{
    /// <summary>
    /// Конструктивний модуль квартального календаря (п. 3.2 ТЗ).
    /// Обов'язково складається з двох компонентів:
    /// 1. Підкладка (Backing) — нижній шар модуля.
    /// 2. Календарний блок (CalendarBlock) — верхній шар, лежить поверх своєї підкладки.
    /// Блок нерозривно прив'язаний до своєї підкладки.
    /// </summary>
    public class CalendarModule
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        private int _moduleIndex = 1;
        /// <summary>
        /// Порядковий номер модуля (від 1 до 4).
        /// </summary>
        public int ModuleIndex
        {
            get => _moduleIndex;
            set
            {
                _moduleIndex = value;
                UpdateComponentNames();
            }
        }

        /// <summary>
        /// Підкладка модуля (картонна основа).
        /// </summary>
        public CalendarComponent Backing { get; set; } = new CalendarComponent();

        /// <summary>
        /// Календарний блок модуля (розташовується поверх підкладки).
        /// </summary>
        public CalendarComponent CalendarBlock { get; set; } = new CalendarComponent();

        /// <summary>
        /// Внутрішнє пружинне кріплення (п. 4.1 ТЗ): кріпить календарний блок до своєї підкладки.
        /// </summary>
        public SpiralItemConfig InternalSpiral { get; set; } = new SpiralItemConfig();

        /// <summary>
        /// Зовнішнє пружинне кріплення (п. 4.2 ТЗ): з'єднує підкладку даного модуля з попереднім елементом
        /// (з шапкою для модуля №1 або з підкладкою попереднього модуля для №2..4).
        /// </summary>
        public SpiralItemConfig ExternalSpiral { get; set; } = new SpiralItemConfig();

        public CalendarModule()
        {
            UpdateComponentNames();
        }

        public CalendarModule(int moduleIndex) : this()
        {
            ModuleIndex = moduleIndex;
        }

        public void UpdateComponentNames()
        {
            Backing.Name = $"Підкладка {ModuleIndex}";
            CalendarBlock.Name = $"Календарний блок {ModuleIndex}";
        }

        public override string ToString()
        {
            return $"Модуль {ModuleIndex}";
        }
    }
}
