using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Models
{
    public class CategoryToCustomer
    {
        public object Id { get; set; } = ObjectId.GenerateNewId();
        public object CustomerId { get; set; }
        public List<object> CategoriedIdList { get; set; } = new List<object>();
    }
}
