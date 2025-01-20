using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class PageSide
    {
        public int MasterIdx { get; set; }
        public int PrintIdx { get; set; }
        
        [JsonIgnore]
        public ImposRunPage AssignedRunPage { get; set; }

        public double Angle { get; set; } = 0;
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;

    }
}
