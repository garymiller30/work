using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Models
{
    public class PantoneTable
    {
        public ColorTable ColorTable { get; set; }
    }


    public class ColorTable
    {
        public IEnumerable<Color> Color { get;set;}
        public string _ActionIfObjectExist { get;set;}
        public string _Editable { get;set;}
        public string _Name { get;set;}
        public string _Prefix { get;set;}
        public string _Producer { get;set;}
    }

    public class Color
    {
        public string _Lab { get;set;}
        public string _Name { get;set;}
        public string _Opacity { get;set;}
    }
}
