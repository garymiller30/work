using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActiveWorks.PluginHub
{
    internal static class DisabledPluginStore
    {
        public const string FileName = "disabled-plugins.txt";

        public static HashSet<string> Load(string pluginsDirectory)
        {
            var path = GetPath(pluginsDirectory);
            if (!File.Exists(path))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            return new HashSet<string>(
                File.ReadAllLines(path)
                    .Select(x => (x ?? string.Empty).Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x)),
                StringComparer.OrdinalIgnoreCase);
        }

        public static void Save(string pluginsDirectory, IEnumerable<string> disabledPluginFiles)
        {
            Directory.CreateDirectory(pluginsDirectory);
            var lines = disabledPluginFiles
                .Select(x => (x ?? string.Empty).Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            File.WriteAllLines(GetPath(pluginsDirectory), lines);
        }

        private static string GetPath(string pluginsDirectory)
        {
            return Path.Combine(pluginsDirectory, FileName);
        }
    }
}
