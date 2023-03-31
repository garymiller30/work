using Interfaces;
using MongoDB.Bson;

namespace Job
{
    public class Category :ICategory, IWithId
    {
        public object Id { get; set; }
        public string Name { get;set; }
    }
}
