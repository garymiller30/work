using System;

namespace PluginFileshareWeb
{
    internal sealed class PageDownloadLink
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Host { get; set; }
        public long? SizeBytes { get; set; }

        public string SizeText
        {
            get
            {
                if (!SizeBytes.HasValue)
                {
                    return string.Empty;
                }

                double size = SizeBytes.Value;
                string[] units = { "B", "KB", "MB", "GB" };
                int unitIndex = 0;

                while (size >= 1024 && unitIndex < units.Length - 1)
                {
                    size /= 1024;
                    unitIndex++;
                }

                return unitIndex == 0 ? $"{size:0} {units[unitIndex]}" : $"{size:0.##} {units[unitIndex]}";
            }
        }

        public string DisplayName
        {
            get
            {
                string text = string.IsNullOrWhiteSpace(Text) ? Url : Text.Trim();

                return string.IsNullOrWhiteSpace(Host) ? text : $"{text}  [{Host}]";
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
