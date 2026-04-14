using Interfaces;
using MongoDB.Bson;

namespace JobSpace
{
    public sealed class PlateOwner : IWithId
    {
        public object Id { get; set; } = ObjectId.GenerateNewId();

        public string Name { get; set; }


    }
}
