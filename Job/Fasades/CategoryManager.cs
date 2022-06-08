using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using MongoDB.Bson;

namespace Job.Fasades
{
    public class CategoryManager : ICategoryManager
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

        public ObjectId Add(string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                var cat = GetAll().Where(x => x.Name.Equals(category, StringComparison.InvariantCultureIgnoreCase));
                if (cat.Any())
                {
                    return cat.First().Id;
                }
                
                //var newCat = (Category)UserProfile.Base.Add<Category>(CollectionString);
                var newCat = new Category();
                newCat.Name = category;

                _userProfile.Base.Add(CollectionString, newCat);

                _categories.Add(newCat);

                return newCat.Id;
            }

            throw new ArgumentNullException("category is null or empty");
        }

        public ICategory GetCategoryById(ObjectId id)
        {
            var category = GetAll().Where(x => x.Id == id);

            if (category.Any())
            {
                return category.First();
            }

            return null;

        }

        public string GetCategoryNameById(ObjectId categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category == null) return string.Empty;
            return category.Name;
        }


    }
}
