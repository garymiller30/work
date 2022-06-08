using MongoDB.Bson;

namespace Interfaces
{
    public interface ICategory
    {
        ObjectId Id { get; set; }
        string Name { get;set; }
    }
}
