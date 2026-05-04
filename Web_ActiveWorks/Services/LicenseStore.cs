using System.Text.Json;
using Web_ActiveWorks.Models;

namespace Web_ActiveWorks.Services;

public sealed class LicenseStore
{
    private readonly string _storePath;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public LicenseStore(IWebHostEnvironment environment, IConfiguration configuration)
    {
        var options = configuration.GetSection("Licensing").Get<LicenseOptions>() ?? new LicenseOptions();
        _storePath = Path.IsPathRooted(options.StorePath)
            ? options.StorePath
            : Path.Combine(environment.ContentRootPath, options.StorePath);
    }

    public async Task<LicenseSubscription?> FindByKeyAsync(string licenseKey)
    {
        var document = await LoadAsync();
        return document.Licenses.FirstOrDefault(x => string.Equals(x.LicenseKey, licenseKey, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<LicenseSubscription?> FindByIdAsync(string licenseId)
    {
        var document = await LoadAsync();
        return document.Licenses.FirstOrDefault(x => string.Equals(x.LicenseId, licenseId, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> RegisterMachineAsync(LicenseSubscription license, string machineId)
    {
        await _gate.WaitAsync();
        try
        {
            var document = await LoadCoreAsync();
            var stored = document.Licenses.FirstOrDefault(x => string.Equals(x.LicenseId, license.LicenseId, StringComparison.OrdinalIgnoreCase));
            if (stored is null)
            {
                return false;
            }

            if (stored.ActivatedMachineIds.Any(x => string.Equals(x, machineId, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            if (stored.ActivatedMachineIds.Count >= Math.Max(stored.MaxDevices, 1))
            {
                return false;
            }

            stored.ActivatedMachineIds.Add(machineId);
            await SaveCoreAsync(document);
            license.ActivatedMachineIds = stored.ActivatedMachineIds;
            return true;
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<LicenseStoreDocument> LoadAsync()
    {
        await _gate.WaitAsync();
        try
        {
            return await LoadCoreAsync();
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<LicenseStoreDocument> LoadCoreAsync()
    {
        if (!File.Exists(_storePath))
        {
            return new LicenseStoreDocument();
        }

        await using var stream = File.OpenRead(_storePath);
        return await JsonSerializer.DeserializeAsync<LicenseStoreDocument>(stream, _jsonOptions) ?? new LicenseStoreDocument();
    }

    private async Task SaveCoreAsync(LicenseStoreDocument document)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_storePath)!);
        await using var stream = File.Create(_storePath);
        await JsonSerializer.SerializeAsync(stream, document, _jsonOptions);
    }
}
