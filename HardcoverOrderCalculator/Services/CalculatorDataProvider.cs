using System.Text.Json;
using HardcoverOrderCalculator.Models;

namespace HardcoverOrderCalculator.Services;

public sealed class CalculatorDataProvider
{
    private readonly IWebHostEnvironment _environment;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private CalculatorData? _cache;

    public CalculatorDataProvider(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<CalculatorData> GetAsync()
    {
        if (_cache is not null)
        {
            return _cache;
        }

        var path = Path.Combine(_environment.ContentRootPath, "Data", "calculator-data.json");
        await using var stream = File.OpenRead(path);
        _cache = await JsonSerializer.DeserializeAsync<CalculatorData>(stream, _jsonOptions)
                 ?? throw new InvalidOperationException("calculator-data.json is empty or invalid.");

        return _cache;
    }
}
