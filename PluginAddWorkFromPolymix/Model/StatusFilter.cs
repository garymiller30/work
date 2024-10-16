using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PluginAddWorkFromPolymix.Model
{
    public class StatusFilter : IFilter
    {
        public string TypeName { get; set; } = "стан замовлення";
        public string Name { get; set; }
        [JsonIgnore]
        public Image Img { get; set; }

        public int Code { get; set; }

        public override int GetHashCode()
        {
            return $"{Name}{Code}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is StatusFilter filter)
            {
                if (filter.Name.Equals(Name) && filter.Code == Code) return true;
            }
            return false;
        }
    }
}
