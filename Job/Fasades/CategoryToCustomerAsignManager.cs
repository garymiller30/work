using BrightIdeasSoftware;
using Interfaces;
using Job.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Fasades
{
    public static class CategoryToCustomerAsignManager
    {
        public static List<ICategory> GetCustomerCategories(IUserProfile profile, object customerId)
        {
            // знайти замовника по id
            List<ICategory> categories = new List<ICategory>();
            var collection = (IMongoCollection<CategoryToCustomer>)profile.Base.GetRawCollection<CategoryToCustomer>();

            var id = ObjectId.Empty;

            if (customerId is string idStr)
            {
                id = ObjectId.Parse(idStr);
            }
            else
            {
                id = (ObjectId)customerId;
            }


            var filter = Builders<CategoryToCustomer>.Filter.Eq(t => t.CustomerId, id);

            var f = collection.FindAsync(filter);
            f.Wait();
            var r = f.Result.ToListAsync(default);
            r.Wait();
            if (r.Result.Any())
            {
                var customer = r.Result[0];
                categories = profile.Categories.GetCategoryByIds(customer.CategoriedIdList);
            }


            return categories;
        }

        public static async void SetCategory(IUserProfile profile, object customerId, object categoryId, bool isChecked)
        {
            // отримати колекцію
            var collection = (IMongoCollection<CategoryToCustomer>)profile.Base.GetRawCollection<CategoryToCustomer>();
            // знайти замовника
            var id = ObjectId.Empty;

            if (customerId is string idStr)
            {
                id = ObjectId.Parse(idStr);
            }
            else
            {
                id = (ObjectId)customerId;
            }
            var filter = Builders<CategoryToCustomer>.Filter.Eq(t => t.CustomerId, id);
            var f = collection.FindAsync(filter);
            f.Wait();
            var r = f.Result.ToListAsync();
            r.Wait();
            if (r.Result.Any())
            {
                // якщо є, то додати/видалити id категорії
                var customer = r.Result[0];
                if (isChecked)
                {
                    var catId = customer.CategoriedIdList.FirstOrDefault(x => x.ToString().Equals(categoryId.ToString()));
                    if (catId != null) return;

                    customer.CategoriedIdList.Add(categoryId);
                }
                else
                {
                    //знайти з таким id і видалити
                    var catId = customer.CategoriedIdList.FirstOrDefault(x => x.ToString().Equals(categoryId.ToString()));

                    if (catId == null) return;
                    // якщо є, то видалити
                    customer.CategoriedIdList.Remove(catId);
                    // оновити

                }
                var custfilter = Builders<CategoryToCustomer>.Filter.Eq(t => t.Id, customer.Id);

                var res = await collection.ReplaceOneAsync(custfilter, customer).ConfigureAwait(false);
            }
            else
            {
                // якщо нема, то додати замовника
                if (isChecked)
                {
                    var customer = new CategoryToCustomer();
                    customer.CustomerId = customerId;
                    customer.CategoriedIdList.Add(categoryId);
                    await collection.InsertOneAsync(customer).ConfigureAwait(false);
                }
            }
        }
    }
}
