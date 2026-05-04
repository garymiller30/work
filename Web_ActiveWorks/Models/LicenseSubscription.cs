using System.Text.Json.Serialization;

namespace Web_ActiveWorks.Models;

public sealed class LicenseSubscription
{
    [JsonPropertyName("licenseKey")]
    public string LicenseKey { get; set; } = "";

    [JsonPropertyName("licenseId")]
    public string LicenseId { get; set; } = "";

    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "active";

    [JsonPropertyName("paidUntilUtc")]
    public DateTimeOffset PaidUntilUtc { get; set; }

    [JsonPropertyName("maxDevices")]
    public int MaxDevices { get; set; } = 1;

    [JsonPropertyName("activatedMachineIds")]
    public List<string> ActivatedMachineIds { get; set; } = [];

    [JsonPropertyName("features")]
    public LicenseFeatureSet Features { get; set; } = new();
}
