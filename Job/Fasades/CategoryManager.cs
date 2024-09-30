using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using JobSpace.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JobSpace.Fasades
{
    public sealed class CategoryManager : ICategoryManager
    {

        private readonly IUserProfile _userProfile;

        public CategoryManager(IUserProfile userProfile)
        {
            _userProfile = userProfile;
        }


        const string CollectionString = "Categories";

        private List<Category> _categories;

        public IEnumerable<ICategory> GetAll()
        {
            if (_categories == null) _categories = _userProfile.Base.GetCollection<Category>(CollectionString);

            return _categories.ToList();
        }

        public object Add(string category)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category is null or empty");
            
            var cat = GetAll().Where(x => x.Name.Equals(category, StringComparison.InvariantCultureIgnoreCase));
            if (cat.Any())
            {
                return cat.First().Id;
            }
                
            var newCat = new Category();
            newCat.Name = category;

            _userProfile.Base.Add(CollectionString, newCat);

            _categories.Add(newCat);

            return newCat.Id;
        }

        public ICategory GetCategoryById(object id)
        {
            if (id == null) return null;

            var category = GetAll().Where(x => (ObjectId)x.Id == (ObjectId)id);

            if (category.Any())
            {
                return category.First();
            }

            return null;

        }

        public string GetCategoryNameById(object categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category == null) return string.Empty;
            return category.Name;
        }

        public void Remove(ICategory category)
        {
            _userProfile.Base.Remove(CollectionString,(Category)category);
            _categories.Remove((Category)category);
        }

        public List<ICategory> GetCategoryByIds(List<object> categoriedIdList)
        {
            var categoryFilter = Builders<Category>.Filter.In(x=>x.Id,categoriedIdList);
            var collection = (IMongoCollection<Category>)_userProfile.Base.GetRawCollection<Category>(CollectionString);

            var res = collection.FindAsync(categoryFilter);
            res.Wait();
            var r = res.Result.ToListAsync();
            r.Wait();

            if (r.Result.Count > 0)
            {
                return r.Result.Cast<ICategory>().ToList();
            }
            return new List<ICategory>();
        }
    }
}
