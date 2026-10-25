using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Markup;
using System.Globalization;

public class FontItem
{
    public string DisplayName { get; set; } // Наприклад: "Myriad Pro Bold Condensed"
    public string FilePath { get; set; }    // Повний шлях до файлу (знадобиться для PDFlib)

    public override string ToString() => DisplayName;
}

public static class UltimateFontScanner
{
    public static List<FontItem> GetAllFonts()
    {
        var fontDict = new Dictionary<string, FontItem>(StringComparer.OrdinalIgnoreCase);

        // 1. Збираємо шляхи до обох папок зі шрифтами
        var directories = new List<string>();

        string systemDir = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        if (Directory.Exists(systemDir)) directories.Add(systemDir);

        string userDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"Microsoft\Windows\Fonts"
        );
        if (Directory.Exists(userDir)) directories.Add(userDir);

        // 2. Скануємо всі файли
        foreach (var dir in directories)
        {
            foreach (var file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".otf" || ext == ".ttf" || ext == ".ttc")
                {
                    try
                    {
                        var typeface = new GlyphTypeface(new Uri(file));

                        // Отримуємо англійську назву сімейства (FamilyName)
                        string familyName = GetLocalizedName(typeface.FamilyNames);

                        // Отримуємо назву стилю (FaceName / Subfamily)
                        string faceName = GetLocalizedName(typeface.FaceNames);

                        // Формуємо красиву повну назву (як у Illustrator)
                        string fullName = familyName;
                        if (!string.IsNullOrEmpty(faceName) &&
                            faceName != "Regular" &&
                            faceName != "Normal" &&
                            !familyName.Contains(faceName))
                        {
                            // Якщо це, наприклад, "Myriad Pro" + "Bold Condensed" -> "Myriad Pro Bold Condensed"
                            fullName = $"{familyName} {faceName}";
                        }

                        if (!string.IsNullOrEmpty(fullName) && !fontDict.ContainsKey(fullName))
                        {
                            fontDict[fullName] = new FontItem
                            {
                                DisplayName = fullName,
                                FilePath = file
                            };
                        }
                    }
                    catch
                    {
                        // Якщо файл пошкоджений або не читається — просто пропускаємо
                    }
                }
            }
        }

        return fontDict.Values.OrderBy(x => x.DisplayName).ToList();
    }

    private static string GetLocalizedName(IDictionary<CultureInfo, string> namesDict)
    {
        if (namesDict == null || namesDict.Count == 0) return string.Empty;

        // Шукаємо спочатку англійську (en-US) через CultureInfo
        var enCulture = new CultureInfo("en-US");
        if (namesDict.TryGetValue(enCulture, out string name))
        {
            return name;
        }

        // Якщо немає англійської, беремо першу ліпшу доступну
        return namesDict.Values.FirstOrDefault() ?? string.Empty;
    }
}