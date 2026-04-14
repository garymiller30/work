using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web_ActiveWorks.Models;

[BsonIgnoreExtraElements]
public sealed class MongoJobStatusDocument
{
    public ObjectId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Code { get; set; }
    
}
