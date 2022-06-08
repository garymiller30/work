using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasheViewer
{
    [Serializable]
    public class CasheSettings
    {
        public int PayStatusCode { get; set; }
        public decimal PriceForPlate { get; set; } = 15;
    }
}
