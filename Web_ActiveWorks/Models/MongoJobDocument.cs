using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web_ActiveWorks.Models;

[BsonIgnoreExtraElements]
public sealed class MongoJobDocument
{
    public ObjectId Id { get; set; }
    public int StatusCode { get; set; }
 

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime Date { get; set; }

    public string Number { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}
