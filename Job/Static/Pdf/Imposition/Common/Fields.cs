using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Common
{
    public class Fields
    {
        public double Outer { get; set;}
        public double Inner { get; set;}
        public double Top { get; set;}
        public double Bottom { get; set;}

        public void SetAll(double value)
        {
            Outer = value;
            Inner = value;
            Top = value;
            Bottom = value;
        }
    }
}
