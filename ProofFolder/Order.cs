using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProofFolder
{
    [Table(Name= "dbo.WorkOrder")]
    public class Order
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int N { get; set; }

        [Column(Name = "ID_number")]
        public int ID_number { get; set; }

        [Column(Name = "Comment")]
        public string Comment { get; set; }

        [Column(Name = "OrderState")]
        public int OrderState { get; set; }
    }
}
