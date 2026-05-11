using MongoDB.Bson;

namespace Web_ActiveWorks.Models
{
    public class MongoJobPayItem
    {
        public DateTime Date { get; set; }
        public BsonValue Sum { get; set; } = BsonNull.Value;
    }
}
