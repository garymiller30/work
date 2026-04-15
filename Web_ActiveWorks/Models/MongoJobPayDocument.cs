using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web_ActiveWorks.Models
{
    [BsonIgnoreExtraElements]
    public class MongoJobPayDocument
    {
        public ObjectId ParentId { get; set; }
        public string Price { get; set; } = default!;
        public List<MongoJobPayItem> Pays { get; set; } = new();
    }
}
