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

        List<MongoJobDocument> jobs = await jobCollection.Find(filter)
            .SortByDescending(x => x.Date)
            .Limit(500)
            .ToListAsync(cancellationToken);

        var jobIds = jobs.Select(x => x.Id).ToList();
        Dictionary<ObjectId, decimal>? paymentMap = new Dictionary<ObjectId, decimal>();
        Dictionary<ObjectId, bool> isPayedMap = new Dictionary<ObjectId, bool>();

        foreach (var processName in payProcesses)
        {
            var payCollection = database.GetCollection<MongoJobPayDocument>(processName);
            var payDocs = await payCollection
                .Find(x => jobIds.Contains(x.ParentId))
                .ToListAsync(cancellationToken);

            foreach (var doc in payDocs)
            {
                if (!decimal.TryParse(doc.Price, out var price))
                {
                    continue;
                }

                var paidSum = doc.Pays
                    .Select(p => decimal.TryParse(p.Sum, out var sum) ? sum : 0m)
                    .Sum();

                var remaining = price - paidSum;

                if (paymentMap.ContainsKey(doc.ParentId))
                {
                    paymentMap[doc.ParentId] += remaining;
                }
                else
                {
                    paymentMap[doc.ParentId] = remaining;
                }

                if (price == 0)
                {
                    isPayedMap[doc.ParentId] = false;
                }
                else
                {
                    isPayedMap[doc.ParentId] = paidSum >= price;
                }

            }
        }

        return jobs.Select(job =>
        {
            paymentMap.TryGetValue(job.Id, out var remaining);
            isPayedMap.TryGetValue(job.Id, out var isPayed);

            return new JobListItemViewModel
            {
                Id = job.Id.ToString(),
                Date = job.Date,
                StatusCode = job.StatusCode,
                StatusText = statusMap.TryGetValue(job.StatusCode, out var statusText)
                    ? statusText
                    : $"Status #{job.StatusCode}",
                Number = job.Number,
                Customer = job.Customer,
                Description = job.Description,
                Price = remaining,
                IsPayed = isPayed
            };
        }).ToList();
    }

    public async Task<int> UpdateJobStatusesAsync(
        WebProfileDefinition profile,
        IReadOnlyCollection<string> jobIds,
        int statusCode,
        CancellationToken cancellationToken = default)
    {
        var parsedIds = jobIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => ObjectId.TryParse(id, out var objectId)
                ? (IsValid: true, ObjectId: objectId)
                : (IsValid: false, ObjectId: ObjectId.Empty))
            .Where(x => x.IsValid)
            .Select(x => x.ObjectId)
            .Distinct()
            .ToList();

        if (parsedIds.Count == 0)
        {
            return 0;
        }

        var collection = GetDatabase(profile).GetCollection<MongoJobDocument>("Jobs");
        var filter = Builders<MongoJobDocument>.Filter.In(x => x.Id, parsedIds);
        var update = Builders<MongoJobDocument>.Update.Set(x => x.StatusCode, statusCode);
        var result = await collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);

        return (int)result.ModifiedCount;
    }

    private IMongoDatabase GetDatabase(WebProfileDefinition profile)
    {
        var cacheKey = $"{profile.MongoConnectionString}::{profile.MongoDatabaseName}";
        return _databaseCache.GetOrAdd(
            cacheKey,
            _ => new MongoClient(profile.MongoConnectionString).GetDatabase(profile.MongoDatabaseName));
    }
}
