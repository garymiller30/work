using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
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
        List<string> payProcesses,
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

        var jobIds = jobs.Select(x => x.Id).ToList();

        // ===== Збір оплат з усіх процесів =====

        var paymentMap = new Dictionary<ObjectId, decimal>();

        foreach (var processName in payProcesses)
        {
            var payCollection = database.GetCollection<MongoJobPayDocument>(processName);

            var payDocs = await payCollection
                .Find(x => jobIds.Contains(x.ParentId))
                .ToListAsync(cancellationToken);

            foreach (var doc in payDocs)
            {
                if (!decimal.TryParse(doc.Price, out var price))
                    continue;

                var paidSum = doc.Pays
                    .Select(p => decimal.TryParse(p.Sum, out var s) ? s : 0m)
                    .Sum();

                var remaining = price - paidSum;

                if (paymentMap.ContainsKey(doc.ParentId))
                    paymentMap[doc.ParentId] += remaining;
                else
                    paymentMap[doc.ParentId] = remaining;
            }
        }



        return jobs.Select(job =>
        {
            paymentMap.TryGetValue(job.Id, out var remaining);

            return new JobListItemViewModel
            {
                Date = job.Date,
                StatusCode = job.StatusCode,
                StatusText = statusMap.TryGetValue(job.StatusCode, out var statusText)
                    ? statusText
                    : $"Status #{job.StatusCode}",
                Number = job.Number,
                Customer = job.Customer,
                Description = job.Description,
                Price = remaining
            };
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
