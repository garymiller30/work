using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Common
{
    public class Position
    {
        public double XOffset { get; set;}
        public double YOffset { get; set;}
        public bool CenterVertically { get; set;}
        public bool CenterHorizontally { get; set;} = true;
    }
}
