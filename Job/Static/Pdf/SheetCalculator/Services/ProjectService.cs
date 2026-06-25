using System;
using System.IO;
using System.Text.Json;
using JobSpace.Static.Pdf.SheetCalculator.Models;

namespace JobSpace.Static.Pdf.SheetCalculator.Services
{
    public static class ProjectService
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static Project CreateNewProject()
        {
            var project = new Project();
            
            // Add a default printing sheet
            project.Sheets.Add(new Sheet
            {
                Name = "Лист SRA3",
                Width = 450,
                Height = 320,
                MarginLeft = 10,
                MarginRight = 10,
                MarginTop = 10,
                MarginBottom = 10
            });

            // Add some default products
            project.Products.Add(new Product
            {
                Name = "Візитка",
                Width = 90,
                Height = 50,
                RequiredCirculation = 1000,
                TechMargin = 2
            });

            project.Products.Add(new Product
            {
                Name = "Листівка А6",
                Width = 105,
                Height = 148,
                RequiredCirculation = 500,
                TechMargin = 2
            });

            return project;
        }

        public static Project LoadProject(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Файл проєкту не знайдено", filePath);

            string json = File.ReadAllText(filePath);
            var project = JsonSerializer.Deserialize<Project>(json, JsonOptions);
            
            if (project == null)
                throw new InvalidDataException("Не вдалося завантажити проєкт. Невірний формат файлу.");

            // Recalculate calculations immediately after loading
            CalculationService.Recalculate(project);

            return project;
        }

        public static void SaveProject(Project project, string filePath)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Шлях до файлу порожній", nameof(filePath));

            // Recalculate before saving to ensure data is consistent
            CalculationService.Recalculate(project);

            string json = JsonSerializer.Serialize(project, JsonOptions);
            File.WriteAllText(filePath, json);
        }
    }
}
