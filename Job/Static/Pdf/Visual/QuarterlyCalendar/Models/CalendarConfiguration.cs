using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobSpace.Static.Pdf.Visual.QuarterlyCalendar.Models
{
    /// <summary>
    /// Головна модель квартального календаря (п. 10 та п. 12 ТЗ).
    /// Містить шапку, від 1 до 4 конструктивних модулів, розраховані габарити та налаштування експорту.
    /// </summary>
    public class CalendarConfiguration
    {
        public const int MinModules = 1;
        public const int MaxModules = 4;

        public string Name { get; set; } = "Квартальний календар";

        /// <summary>
        /// Шапка календаря (завжди одна, розташована вгорі).
        /// </summary>
        public CalendarHeader Header { get; set; } = new CalendarHeader();

        /// <summary>
        /// Список конструктивних модулів (від 1 до 4).
        /// </summary>
        public List<CalendarModule> Modules { get; set; } = new List<CalendarModule>();

        /// <summary>
        /// Загальні розраховані габарити готового календаря у міліметрах (Bounding Box усіх компонентів).
        /// </summary>
        public double TotalWidthMm { get; set; } = 0.0;
        public double TotalHeightMm { get; set; } = 0.0;

        /// <summary>
        /// Налаштування експорту готової візуалізації.
        /// </summary>
        public CalendarExportSettings ExportSettings { get; set; } = new CalendarExportSettings();

        [JsonIgnore]
        public bool CanAddModule => Modules.Count < MaxModules;

        [JsonIgnore]
        public bool CanRemoveModule => Modules.Count > MinModules;

        public CalendarConfiguration()
        {
            CreateDefaultModules(3);
        }

        public CalendarConfiguration(int initialModuleCount)
        {
            int count = Math.Clamp(initialModuleCount, MinModules, MaxModules);
            CreateDefaultModules(count);
        }

        [JsonConstructor]
        public CalendarConfiguration(string? name, CalendarHeader? header, List<CalendarModule>? modules, CalendarExportSettings? exportSettings)
        {
            if (name != null) Name = name;
            if (header != null) Header = header;
            Modules = modules ?? new List<CalendarModule>();
            if (exportSettings != null) ExportSettings = exportSettings;
        }

        private void CreateDefaultModules(int count)
        {
            Modules.Clear();
            for (int i = 1; i <= count; i++)
            {
                Modules.Add(new CalendarModule(i));
            }
        }

        /// <summary>
        /// Додати новий конструктивний модуль (якщо кількість не перевищує 4).
        /// </summary>
        public CalendarModule? AddModule()
        {
            if (!CanAddModule)
                return null;

            int newIndex = Modules.Count + 1;
            var module = new CalendarModule(newIndex);
            Modules.Add(module);
            return module;
        }

        /// <summary>
        /// Видалити останній конструктивний модуль (якщо кількість більша за 1).
        /// </summary>
        public bool RemoveLastModule()
        {
            if (!CanRemoveModule)
                return false;

            int lastIdx = Modules.Count - 1;
            var mod = Modules[lastIdx];
            mod.Backing.ClearPreview();
            mod.CalendarBlock.ClearPreview();
            Modules.RemoveAt(lastIdx);
            return true;
        }

        /// <summary>
        /// Отримати плаский перелік усіх PDF-компонентів календаря у порядку відображення.
        /// </summary>
        public IEnumerable<CalendarComponent> GetAllComponents()
        {
            yield return Header;
            foreach (var module in Modules)
            {
                yield return module.Backing;
                yield return module.CalendarBlock;
            }
        }

        /// <summary>
        /// Розрахувати загальні габарити календаря на основі координат та розмірів усіх компонентів.
        /// </summary>
        public void RecalculateBounds()
        {
            var components = GetAllComponents()
                .Where(c => c.WidthMm > 0 && c.HeightMm > 0)
                .ToList();

            if (components.Count == 0)
            {
                TotalWidthMm = 0;
                TotalHeightMm = 0;
                return;
            }

            double minX = components.Min(c => c.OffsetXMm);
            double minY = components.Min(c => c.OffsetYMm);
            double maxX = components.Max(c => c.OffsetXMm + c.WidthMm);
            double maxY = components.Max(c => c.OffsetYMm + c.HeightMm);

            TotalWidthMm = Math.Max(0, maxX - minX);
            TotalHeightMm = Math.Max(0, maxY - minY);
        }

        /// <summary>
        /// Серіалізація конфігурації у JSON-рядок з відступами.
        /// </summary>
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Serialize(this, options);
        }

        /// <summary>
        /// Десеріалізація конфігурації з JSON-рядка.
        /// </summary>
        public static CalendarConfiguration? FromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<CalendarConfiguration>(json, options);
        }

        /// <summary>
        /// Збереження конфігурації у файл.
        /// </summary>
        public void SaveToFile(string filePath)
        {
            string json = ToJson();
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Завантаження конфігурації з файлу.
        /// </summary>
        public static CalendarConfiguration? LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            var config = FromJson(json);
            config?.RecalculateBounds();
            return config;
        }
    }
}
