using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginWorkProcessPlates
{
    [Serializable]
    public sealed class PlateSettings
    {
        public decimal PriceForPlate { get; set; } = 15;

        public List<Format> Formats = new List<Format>();
    }
}
