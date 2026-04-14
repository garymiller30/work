using System.Collections.Concurrent;
using MongoDB.Driver;
using Web_ActiveWorks.Models;

namespace Web_ActiveWorks.Services;

public sealed class MongoProfileDataService
{
    private readonly ConcurrentDictionary<string, IMongoDatabase> _databaseCache = new();

    public async Task<IReadOnlyList<JobStatusOption>> GetStatusesAsync(WebProfileDefinition profile, CancellationToken cancellationToken = default)
    {
        var collection = GetDatabase(profile).GetCollection<MongoJobStatusDocument>("JobStatuses");
        var statuses = await collection.Find(FilterDefinition<MongoJobStatusDocument>.Empty)
            .SortBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return statuses
            .Select(x => new JobStatusOption
            {
                Code = x.Code,
                Name = x.Name
            })
            .ToList();
    }

    public async Task<IReadOnlyList<JobListItemViewModel>> GetJobsAsync(
        WebProfileDefinition profile,
        IReadOnlyCollection<int> visibleStatusCodes,
        CancellationToken cancellationToken = default)
    {
        var database = GetDatabase(profile);
        var jobCollection = database.GetCollection<MongoJobDocument>("Jobs");
        var statusMap = (await GetStatusesAsync(profile, cancellationToken))
            .ToDictionary(x => x.Code, x => x.Name);

        var filter = visibleStatusCodes.Count == 0
            ? Builders<MongoJobDocument>.Filter.Empty
            : Builders<MongoJobDocument>.Filter.In(x => x.StatusCode, visibleStatusCodes);

        List<MongoJobDocument>? jobs = await jobCollection.Find(filter)
            .SortByDescending(x => x.Date)
            .Limit(500)
            .ToListAsync(cancellationToken);

        return jobs.Select(job => new JobListItemViewModel
            {
                Date = job.Date,
                StatusCode = job.StatusCode,
                StatusText = statusMap.TryGetValue(job.StatusCode, out var statusText)
                    ? statusText
                    : $"Status #{job.StatusCode}",
                Number = job.Number,
                Customer = job.Customer,
                Description = job.Description
            })
            .ToList();
    }

    private IMongoDatabase GetDatabase(WebProfileDefinition profile)
    {
        var cacheKey = $"{profile.MongoConnectionString}::{profile.MongoDatabaseName}";
        return _databaseCache.GetOrAdd(
            cacheKey,
            _ => new MongoClient(profile.MongoConnectionString).GetDatabase(profile.MongoDatabaseName));
    }
}
