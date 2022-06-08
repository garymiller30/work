using MongoDB.Bson;

namespace Interfaces
{
    public interface IWithId
    {
        ObjectId Id { get; set; }
    }
}