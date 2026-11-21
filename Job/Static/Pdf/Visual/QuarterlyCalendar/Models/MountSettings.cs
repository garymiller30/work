using System;
using System.ComponentModel;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models
{
    /// <summary>
    /// Варіанти кріплення шапки календаря згідно з п. 5 ТЗ.
    /// </summary>
    public enum HeaderMountType
    {
        [Description("Пружина зверху та знизу")]
        SpringTopAndBottom = 0,

        [Description("Люверс зверху та пружина знизу")]
        EyeletTopSpringBottom = 1
    }

    /// <summary>
    /// Параметри люверса у верхній частині шапки згідно з п. 5 ТЗ:
    /// - Розміщення по центру шапки по горизонталі.
    /// - Відступ від верхнього краю — 5 мм.
    /// - Діаметр — від 3 до 5 мм (налаштовується користувачем).
    /// </summary>
    public class EyeletSettings
    {
        public bool Enabled { get; set; } = true;

        private double _diameterMm = 4.0;
        /// <summary>
        /// Діаметр люверса у міліметрах (від 3 до 5 мм).
        /// </summary>
        public double DiameterMm
        {
            get => _diameterMm;
            set => _diameterMm = Math.Clamp(value, 3.0, 5.0);
        }

        /// <summary>
        /// Відступ центру люверса від верхнього краю шапки у мм (фіксовано 5 мм за ТЗ).
        /// </summary>
        public double TopMarginMm { get; set; } = 5.0;

        /// <summary>
        /// Додаткове ручне зміщення люверса по X від центру шапки (0 = ідеально по центру).
        /// </summary>
        public double OffsetXMm { get; set; } = 0.0;
    }

    /// <summary>
    /// Параметри пружинного кріплення елементів календаря.
    /// </summary>
    public class SpiralItemConfig
    {
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Шлях до PDF-файлу моделі пружини (з db\spirals або іншого джерела).
        /// </summary>
        public string? SpiralFilePath { get; set; }

        /// <summary>
        /// Крок однієї ланки пружини у мм (TrimBox Width файлу пружини).
        /// </summary>
        public double SpiralStepMm { get; set; } = 0.0;

        /// <summary>
        /// Висота/діаметр ланки пружини у мм (TrimBox Height файлу пружини).
        /// </summary>
        public double SpiralHeightMm { get; set; } = 0.0;

        /// <summary>
        /// Користувацький або номінальний діаметр пружини у міліметрах.
        /// </summary>
        public double DiameterMm { get; set; } = 10.0;
    }
}
