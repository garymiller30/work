using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Web_ActiveWorks.Models;

namespace Web_ActiveWorks.Services;

public sealed class LicenseTokenService
{
    private readonly LicenseOptions _options;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public LicenseTokenService(IConfiguration configuration)
    {
        _options = configuration.GetSection("Licensing").Get<LicenseOptions>() ?? new LicenseOptions();
    }

    public string CreateToken(LicenseSubscription license, string machineId)
    {
        var payload = new LicenseTokenPayload
        {
            LicenseId = license.LicenseId,
            CustomerId = license.CustomerId,
            MachineId = machineId,
            Status = GetRuntimeStatus(license),
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(Math.Max(_options.TokenLifetimeDays, 1)).ToString("O"),
            PaidUntilUtc = license.PaidUntilUtc.ToString("O"),
            Features = BuildRuntimeFeatures(license)
        };

        var payloadJson = JsonSerializer.Serialize(payload, _jsonOptions);
        var payloadPart = Base64Url.Encode(Encoding.UTF8.GetBytes(payloadJson));
        var signedBytes = Encoding.UTF8.GetBytes(payloadPart);

        using var rsa = CreatePrivateRsa();
        var signature = rsa.SignData(signedBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return payloadPart + "." + Base64Url.Encode(signature);
    }

    public LicenseTokenPayload? ValidateToken(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 2)
        {
            return null;
        }

        using var rsa = CreatePrivateRsa();
        var signedBytes = Encoding.UTF8.GetBytes(parts[0]);
        var signature = Base64Url.Decode(parts[1]);
        if (!rsa.VerifyData(signedBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
        {
            return null;
        }

        var payloadJson = Encoding.UTF8.GetString(Base64Url.Decode(parts[0]));
        return JsonSerializer.Deserialize<LicenseTokenPayload>(payloadJson, _jsonOptions);
    }

    public bool AllowsUpdates(string bearerToken)
    {
        var payload = ValidateToken(bearerToken);
        if (payload is null || !string.Equals(payload.Status, "active", StringComparison.OrdinalIgnoreCase) && !string.Equals(payload.Status, "grace", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!DateTimeOffset.TryParse(payload.ExpiresAtUtc, out var expiresAtUtc) || expiresAtUtc <= DateTimeOffset.UtcNow)
        {
            return false;
        }

        return payload.Features.Updates;
    }

    private LicenseFeatureSet BuildRuntimeFeatures(LicenseSubscription license)
    {
        if (IsActiveOrGrace(license))
        {
            return license.Features;
        }

        return new LicenseFeatureSet
        {
            Updates = false,
            ExportPdf = false,
            AdvancedReports = false,
            Sync = false,
            MaxProjects = 3
        };
    }

    private string GetRuntimeStatus(LicenseSubscription license)
    {
        if (string.Equals(license.Status, "blocked", StringComparison.OrdinalIgnoreCase))
        {
            return "blocked";
        }

        if (license.PaidUntilUtc >= DateTimeOffset.UtcNow)
        {
            return "active";
        }

        if (license.PaidUntilUtc.AddDays(Math.Max(_options.GracePeriodDays, 0)) >= DateTimeOffset.UtcNow)
        {
            return "grace";
        }

        return "expired";
    }

    private bool IsActiveOrGrace(LicenseSubscription license)
    {
        var status = GetRuntimeStatus(license);
        return string.Equals(status, "active", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(status, "grace", StringComparison.OrdinalIgnoreCase);
    }

    private RSA CreatePrivateRsa()
    {
        if (string.IsNullOrWhiteSpace(_options.PrivateKeyPem))
        {
            throw new InvalidOperationException("Licensing:PrivateKeyPem is not configured.");
        }

        var rsa = RSA.Create();
        rsa.ImportFromPem(_options.PrivateKeyPem);
        return rsa;
    }
}
