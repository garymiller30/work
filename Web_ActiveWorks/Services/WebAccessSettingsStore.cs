using System.Text.Json;
using Web_ActiveWorks.Models;

namespace Web_ActiveWorks.Services;

public sealed class WebAccessSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly SemaphoreSlim _syncRoot = new(1, 1);
    private readonly string _settingsPath;

    public WebAccessSettingsStore(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(dataDirectory);
        _settingsPath = Path.Combine(dataDirectory, "web-access-settings.json");

        if (!File.Exists(_settingsPath))
        {
            var sample = CreateSampleSettings();
            File.WriteAllText(_settingsPath, JsonSerializer.Serialize(sample, JsonOptions));
        }
    }

    public async Task<WebAccessSettings> GetAsync()
    {
        await _syncRoot.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_settingsPath);
            var settings = JsonSerializer.Deserialize<WebAccessSettings>(json, JsonOptions) ?? new WebAccessSettings();
            return Clone(settings);
        }
        finally
        {
            _syncRoot.Release();
        }
    }

    public async Task SaveAsync(WebAccessSettings settings)
    {
        await _syncRoot.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(settings, JsonOptions);
            await File.WriteAllTextAsync(_settingsPath, json);
        }
        finally
        {
            _syncRoot.Release();
        }
    }

    public async Task<WebUserDefinition?> ValidateCredentialsAsync(string? username, string? password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var settings = await GetAsync();
        return settings.Users.FirstOrDefault(x =>
            string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase) &&
            x.Password == password);
    }

    public async Task<WebUserDefinition?> FindUserAsync(string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        var settings = await GetAsync();
        return settings.Users.FirstOrDefault(x =>
            string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<WebProfileDefinition?> FindProfileAsync(string? profileKey)
    {
        if (string.IsNullOrWhiteSpace(profileKey))
        {
            return null;
        }

        var settings = await GetAsync();
        return settings.Profiles.FirstOrDefault(x =>
            string.Equals(x.Key, profileKey, StringComparison.OrdinalIgnoreCase));
    }

    private static WebAccessSettings Clone(WebAccessSettings settings)
    {
        return JsonSerializer.Deserialize<WebAccessSettings>(
                   JsonSerializer.Serialize(settings, JsonOptions),
                   JsonOptions) ??
               new WebAccessSettings();
    }

    private static WebAccessSettings CreateSampleSettings()
    {
        return new WebAccessSettings
        {
            Profiles =
            [
                new WebProfileDefinition
                {
                    Key = "default-profile",
                    Name = "Default profile",
                    MongoConnectionString = "mongodb://localhost:27017",
                    MongoDatabaseName = "ActiveWorks"
                }
            ],
            Users =
            [
                new WebUserDefinition
                {
                    Username = "admin",
                    Password = "admin123",
                    DisplayName = "Administrator",
                    IsAdmin = true,
                    CanAccessJobsPage = true,
                    ProfileKey = "default-profile"
                },
                new WebUserDefinition
                {
                    Username = "operator",
                    Password = "operator123",
                    DisplayName = "Operator",
                    CanAccessJobsPage = true,
                    ProfileKey = "default-profile",
                    VisibleStatusCodes = [1, 2]
                }
            ]
        };
    }
}
