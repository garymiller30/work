using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public class PrimitiveAbstract
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FillId {  get; set; }
        public double Tint { get; set; } = 100;
        public string StrokeId { get; set; }
    }
}
