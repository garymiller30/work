using System.Text.Json.Serialization;

namespace Web_ActiveWorks.Models;

public sealed class LicenseTokenPayload
{
    [JsonPropertyName("licenseId")]
    public string LicenseId { get; set; } = "";

    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = "";

    [JsonPropertyName("machineId")]
    public string MachineId { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "";

    [JsonPropertyName("expiresAtUtc")]
    public string ExpiresAtUtc { get; set; } = "";

    [JsonPropertyName("paidUntilUtc")]
    public string? PaidUntilUtc { get; set; }

    [JsonPropertyName("features")]
    public LicenseFeatureSet Features { get; set; } = new();
}
