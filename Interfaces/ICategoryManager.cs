using System.Collections.Generic;
using MongoDB.Bson;

namespace Interfaces
{
    public interface ICategoryManager
    {
        string GetCategoryNameById(ObjectId categoryId);
        ICategory GetCategoryById(ObjectId id);
        IEnumerable<ICategory> GetAll();
        ObjectId Add(string category);
        void Remove(ICategory model);
    }
}
