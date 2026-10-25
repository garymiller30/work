using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public static class FontManager
{
    [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
    private static extern int AddFontResourceEx(string lpszFilename, uint fl, IntPtr pdv);

    private const uint FR_PRIVATE = 0x10;

    // Зберігаємо шлях до вже зареєстрованих файлів (реєстронезалежно для шляхів Windows)
    private static readonly HashSet<string> _registeredFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Перевіряє папку шрифтів користувача та реєструє нові файли, якщо вони з'явилися
    /// </summary>
    public static void RefreshUserFonts()
    {
        string userFontDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"Microsoft\Windows\Fonts"
        );

        if (!Directory.Exists(userFontDir)) return;

        // Отримуємо всі файли шрифтів у папці
        string[] allFiles = Directory.GetFiles(userFontDir, "*.*");

        foreach (var file in allFiles)
        {
            string ext = Path.GetExtension(file).ToLower();
            if (ext == ".ttf" || ext == ".otf" || ext == ".ttc")
            {
                // Якщо цього файлу ще немає в списку зареєстрованих — додаємо
                if (!_registeredFiles.Contains(file))
                {
                    AddFontResourceEx(file, FR_PRIVATE, IntPtr.Zero);
                    _registeredFiles.Add(file);
                }
            }
        }
    }
}