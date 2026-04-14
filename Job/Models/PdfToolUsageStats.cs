using System;
using System.Collections.Generic;

namespace JobSpace.Models
{
    public sealed class PdfToolUsageStats
    {
        public Dictionary<string, PdfToolUsageItem> Tools { get; set; } =
            new Dictionary<string, PdfToolUsageItem>(StringComparer.OrdinalIgnoreCase);
    }

    public sealed class PdfToolUsageItem
    {
        public string ToolType { get; set; }
        public string Name { get; set; }
        public string MenuPath { get; set; }
        public int LaunchCount { get; set; }
        public DateTime LastStartedUtc { get; set; }
    }
}
