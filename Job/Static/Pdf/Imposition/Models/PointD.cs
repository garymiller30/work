using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    /// <summary>
    /// Point double
    /// </summary>
    public sealed class PointD
    {
        public PointD()
        {
            
        }

        public PointD(double x,double y)
        {
            X = x; Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
