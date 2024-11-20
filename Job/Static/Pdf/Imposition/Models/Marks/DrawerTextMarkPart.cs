using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class DrawerTextMarkPart
    {
        public double X { get;set; }
        public double Y { get;set; }

        public double Angle { get;set; }
        public MarkColor Color { get;set; }
        
        public string Text { get;set; }
    }
}
