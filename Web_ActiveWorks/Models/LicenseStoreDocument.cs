using System.Text.Json.Serialization;

namespace Web_ActiveWorks.Models;

public sealed class LicenseStoreDocument
{
    [JsonPropertyName("licenses")]
    public List<LicenseSubscription> Licenses { get; set; } = [];
}
