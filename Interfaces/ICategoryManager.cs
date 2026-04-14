using System.Collections.Generic;

namespace Interfaces
{
    public interface ICategoryManager
    {
        string GetCategoryNameById(object categoryId);
        ICategory GetCategoryById(object id);
        IEnumerable<ICategory> GetAll();
        object Add(string category);
        void Remove(ICategory model);
        List<ICategory> GetCategoryByIds(List<object> categoriedIdList);
    }
}
