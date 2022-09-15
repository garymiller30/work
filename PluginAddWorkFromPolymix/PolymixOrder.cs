using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginAddWorkFromPolymix
{
    public sealed class PolymixOrder
    {
        public int Number { get; set; }
        public string  Customer { get; set; }
        public string  Description { get; set; }

        public override string ToString()
        {
            return $"{Number} • {Customer} • {Description}";
        }
    }
}
