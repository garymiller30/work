﻿using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public double Left => X1;
        public double Bottom => Y1;
        public double Right => X2;
        public double Top => Y2;

        public bool IsInsideMe(CropMark mark)
        {

            if ((mark.From.X >= X1 && mark.From.X <= X2 && mark.From.Y >= Y1 && mark.From.Y <= Y2) || (mark.To.X >= X1 && mark.To.X <= X2 && mark.To.Y >= Y1 && mark.To.Y <= Y2))
            {
                return true;
            }

            return false;
        }

        public bool IntersectsWith(RectangleD rect)
        {
            if (rect.X1 < X2 && X1 < rect.X2 && rect.Y1 < Y2)
            {
                return Y1 < rect.Y2;
            }

            return false;
        }
    }
}
