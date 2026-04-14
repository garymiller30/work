using System;
using System.Diagnostics;
using System.IO;

namespace ExtensionMethods
{
    public static class Files
    {
        private static readonly string[] Sizes = {"b", "Kb", "Mb", "Gb", "Tb", "Pb", "Eb"};
        /// <summary>
        /// return string like 100 Kb, 1Tb etc.
        /// </summary>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static string GetFileSizeInString(this long lenght)
        {
            int order = 0;
            while (lenght >= 1024 && ++order < Sizes.Length)
            {
                lenght = lenght / 1024;
            }
            return $"{lenght:0.##} {Sizes[order]}";
        }

        public static void ShowOpenWithDialog(string path) {
            var args = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += $",OpenAs_RunDLL {path}";
            Process.Start("rundll32.exe", args);
        }
    }
}
