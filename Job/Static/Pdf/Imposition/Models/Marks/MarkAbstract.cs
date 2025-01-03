using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public abstract class MarkAbstract
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public bool Enable { get; set; } = true;
        public PointD Front { get; set; }
        public PointD Back { get; set; }
        public double Angle { get; set; }
        public MarkAbstract()
        {
        }
        public abstract double GetW();
        public abstract double GetH();
    }
}
