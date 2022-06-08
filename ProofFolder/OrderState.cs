using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProofFolder
{
    [Table(Name = "dbo.Dic_OrderState")]
    public class OrderState
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int DicItemID { get; set; }
        [Column(Name = "Code")]
        public int Code { get; set; }
        [Column(Name = "Name")]
        public string Name { get; set; }
    }
}
