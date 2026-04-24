using System.Text.RegularExpressions;

namespace UpdateHub
{
    public static class FileMaskMatcher
    {
        public static bool IsMatch(string mask, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(mask) || string.IsNullOrWhiteSpace(relativePath))
            {
                return false;
            }

            var normalizedMask = Normalize(mask);
            var normalizedPath = Normalize(relativePath);
            var fileName = GetFileName(normalizedPath);

            return Matches(normalizedMask, normalizedPath) || Matches(normalizedMask, fileName);
        }

        private static bool Matches(string mask, string value)
        {
            var regex = "^" + Regex.Escape(mask)
                .Replace(@"\*\*", ".*")
                .Replace(@"\*", @"[^/\\]*")
                .Replace(@"\?", ".") + "$";

            return Regex.IsMatch(value, regex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        private static string Normalize(string value)
        {
            return value.Replace('\\', '/').Trim();
        }

        private static string GetFileName(string value)
        {
            var index = value.LastIndexOf('/');
            return index >= 0 ? value.Substring(index + 1) : value;
        }
    }
}
