using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Models
{
    public class PdfColorResult
    {
        public bool IsSpot { get;set;}

        public string Lab { get;set;}
        public string Name { get;set;}
        public decimal C { get;set;}
        public decimal M { get;set;}
        public decimal Y { get;set;}
        public decimal K { get;set;}

       
    }
}
