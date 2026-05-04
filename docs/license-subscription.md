# ActiveWorks license subscription flow

This branch adds the first production-ready slice for monthly subscription licensing:

- desktop app stores a short-lived signed license token in user settings;
- token is bound to the local machine id;
- token carries feature entitlements such as `updates`, `exportPdf`, `advancedReports`, `sync`, and `maxProjects`;
- `UpdateHub` sends `Authorization: Bearer <token>` when licensing is enabled;
- the web app exposes protected update endpoints.

## Desktop settings

Licensing is disabled by default so existing installations keep using the current public update manifest.

To enable it in `ActiveWorks.exe.config`:

```xml
<setting name="LicenseServerEnabled" serializeAs="String">
  <value>True</value>
</setting>
<setting name="LicenseServerUrl" serializeAs="String">
  <value>https://aworks.pp.ua/</value>
</setting>
<setting name="LicensePublicKeyXml" serializeAs="String">
  <value>...</value>
</setting>
```

The app stores license state in `%LOCALAPPDATA%\ActiveWorks\license.json`, so the token survives application version upgrades.

When licensing is enabled, the app uses:

- `POST /api/license/activate`
- `POST /api/license/refresh`
- `GET /api/updates/manifest`
- `GET /api/updates/download/{path}`

## Server settings

Configure the server private key in `Web_ActiveWorks/appsettings.json` or environment-specific configuration:

```json
{
  "Licensing": {
    "StorePath": "Data/licenses.json",
    "UpdateRootPath": "wwwroot/aw",
    "ManifestFileName": "version.json",
    "PrivateKeyPem": "-----BEGIN PRIVATE KEY-----\n...\n-----END PRIVATE KEY-----",
    "TokenLifetimeDays": 7,
    "GracePeriodDays": 7
  }
}
```

The matching public key must be exported as XML and placed in the desktop `LicensePublicKeyXml` setting because the .NET Framework client verifies RSA signatures with `RSACryptoServiceProvider.FromXmlString`.

## License store

The initial store is `Web_ActiveWorks/Data/licenses.json`. It is intentionally simple and can later be replaced by MongoDB or a payment-provider-backed table.

Example:

```json
{
  "licenseKey": "DEV-CHANGE-ME",
  "licenseId": "lic_dev",
  "customerId": "cus_dev",
  "status": "active",
  "paidUntilUtc": "2099-01-01T00:00:00+00:00",
  "maxDevices": 2,
  "activatedMachineIds": [],
  "features": {
    "updates": true,
    "exportPdf": true,
    "advancedReports": true,
    "sync": true,
    "maxProjects": 999
  }
}
```

When `paidUntilUtc` expires and the grace period passes, the server issues a token with `updates=false` and limited features.

## Creating a license

Use the helper script:

```powershell
.\Web_ActiveWorks\Scripts\New-License.ps1 -CustomerId "customer-name-or-email" -Months 1 -MaxDevices 1
```

The script appends a new license to `Web_ActiveWorks/Data/licenses.json` and prints the generated `LicenseKey`.

Put that key into `%LOCALAPPDATA%\ActiveWorks\license.json`:

```json
{
  "licenseKey": "XXXX-XXXX-XXXX-XXXX-XXXX"
}
```

On the next update check the app calls `/api/license/activate`, the server binds the license to the current machine id, and the app stores the returned short-lived `LicenseToken`.

## Restricting features in code

Feature restrictions are declared with attributes from `Interfaces.Licensing`:

```csharp
[RequiresFeature(LicenseFeature.ExportPdf)]
public sealed class PdfConvert : IPdfTool
{
}
```

PDF tools are checked centrally before execution in `Job/UC/UCFileBrowser.cs`. `ActiveWorks` wires the shared `LicenseFeatureGate` to the current license token during startup.

For non-PDF code you can use the same gate explicitly:

```csharp
if (!LicenseFeatureGate.RequireFor(typeof(SomePremiumClass)))
{
    return;
}
```

or for methods:

```csharp
if (!LicenseFeatureGate.RequireFor(GetType(), nameof(SomePremiumMethod)))
{
    return;
}
```
