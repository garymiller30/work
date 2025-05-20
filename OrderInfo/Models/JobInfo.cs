using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrderInfo.Models
{
    public class JobInfo
    {
        public ObjectId Id { get;set; } = ObjectId.GenerateNewId();

        public ObjectId JobId { get;set; }

        public bool Cut { get;set; }
        public bool UVLak { get;set; }
        public bool ProtectedLak { get; set; }

    }
}
