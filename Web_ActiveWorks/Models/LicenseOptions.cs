namespace Web_ActiveWorks.Models;

public sealed class LicenseOptions
{
    public string StorePath { get; set; } = "Data/licenses.json";

    public string UpdateRootPath { get; set; } = "wwwroot/aw";

    public string ManifestFileName { get; set; } = "version.json";

    public string PrivateKeyPem { get; set; } = "";

    public int TokenLifetimeDays { get; set; } = 7;

    public int GracePeriodDays { get; set; } = 7;
}
