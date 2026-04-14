using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.AutoImpos
{
    public class AutoImposItem
    {
        public object Id { get; set; } = ObjectId.GenerateNewId();

        public string Customer { get; set; }
        public string CategoryId { get; set; }



    }
}
