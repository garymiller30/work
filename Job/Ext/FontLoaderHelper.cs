using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media; // Потребує PresentationCore.dll

public static class FontLoaderHelper
{
    public static List<string> GetAllSystemAndUserFontFaces()
    {
        var fontNames = new List<string>();

        // 1. Формуємо списки шляхів до обох папок зі шрифтами
        var directoriesToScan = new List<string>();

        // Загальносистемна папка (C:\Windows\Fonts)
        string systemFontDir = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        if (Directory.Exists(systemFontDir))
        {
            directoriesToScan.Add(systemFontDir);
        }

        // Локальна папка поточного користувача (AppData\Local\Microsoft\Windows\Fonts)
        string userFontDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"Microsoft\Windows\Fonts"
        );
        if (Directory.Exists(userFontDir))
        {
            directoriesToScan.Add(userFontDir);
        }

        // 2. Скануємо обидві папки
        foreach (var dir in directoriesToScan)
        {
            foreach (var file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".otf" || ext == ".ttf" || ext == ".ttc")
                {
                    try
                    {
                        var typeface = new GlyphTypeface(new Uri(file));
                        foreach (var name in typeface.FaceNames.Values)
                        {
                            if (!fontNames.Contains(name))
                            {
                                fontNames.Add(name);
                            }
                        }
                    }
                    catch
                    {
                        // Пропускаємо файли, які не вдалося розібрати
                    }
                }
            }
        }

        fontNames.Sort();
        return fontNames;
    }
}