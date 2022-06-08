using Interfaces;
using MongoDB.Bson;

namespace Job
{
    public class PlateOwner : IWithId
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Name { get; set; }


    }
}
