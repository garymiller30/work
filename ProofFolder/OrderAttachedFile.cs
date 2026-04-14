using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightIdeasSoftware;

namespace ProofFolder
{
    [Table(Name= "dbo.OrderAttachedFiles")]
    public class OrderAttachedFile
    {
        [Column(IsPrimaryKey = true,IsDbGenerated = true, CanBeNull = false)]
        public int OrderAttachedFileID { get; set; }
        [Column(Name = "OrderID")]
        public int OrderID { get; set; }
        [Column(Name = "FileName",DbType = "varchar(255)")]
        public string FileName { get; set; }
        [Column(CanBeNull = true)]
        public string FileDesc { get; set; }
    }
}
