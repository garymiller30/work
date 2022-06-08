using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PluginAddWorkFromPolymix.Model
{
    public class KindIdFilter : IFilter
    {
        public string TypeName { get; set; } = "тип замовлення";

        public string Name { get; set; }

        public int KindID { get;set; }
        [JsonIgnore]
        public Image Img { get; set; }

        public override int GetHashCode()
        {
            return $"{Name}{KindID}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is KindIdFilter filter)
            {
                if (filter.KindID == KindID && filter.Name.Equals(Name))
                    return true;
            }
            return false;
        }
    }
}
