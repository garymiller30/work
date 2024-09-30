using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public struct RectangleD
    {
        public double X1;
        public double Y1;
        public double X2;
        public double Y2;
        public double W => X2 - X1;
        public double H => Y2 - Y1;

        public bool IsInsideMe(CropMark mark)
        {

            if (mark.From.X >= X1 && mark.From.X <= X2 && mark.From.Y >= Y1 && mark.From.Y <= Y2)
            {
                return true;
            }
            if (mark.To.X >= X1 && mark.To.X <= X2 && mark.To.Y >= Y1 && mark.To.Y <= Y2)
            {
                return true;
            }


            return false;
        }
    }
}
