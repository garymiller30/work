using System.Text.Json.Serialization;

namespace Web_ActiveWorks.Models;

public sealed class LicenseFeatureSet
{
    [JsonPropertyName("updates")]
    public bool Updates { get; set; }

    [JsonPropertyName("exportPdf")]
    public bool ExportPdf { get; set; }

    [JsonPropertyName("advancedReports")]
    public bool AdvancedReports { get; set; }

    [JsonPropertyName("sync")]
    public bool Sync { get; set; }

    [JsonPropertyName("maxProjects")]
    public int MaxProjects { get; set; }
}
