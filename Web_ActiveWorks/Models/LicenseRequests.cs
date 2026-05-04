using System.Text.Json.Serialization;

namespace Web_ActiveWorks.Models;

public sealed class LicenseActivateRequest
{
    [JsonPropertyName("licenseKey")]
    public string LicenseKey { get; set; } = "";

    [JsonPropertyName("machineId")]
    public string MachineId { get; set; } = "";

    [JsonPropertyName("appVersion")]
    public string? AppVersion { get; set; }
}

public sealed class LicenseRefreshRequest
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";

    [JsonPropertyName("machineId")]
    public string MachineId { get; set; } = "";

    [JsonPropertyName("appVersion")]
    public string? AppVersion { get; set; }
}

public sealed class LicenseTokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";
}
