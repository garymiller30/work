using JobSpace.Static.Pdf.Imposition.Models.Marks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public readonly struct RectangleD
    {
        public double X1 { get; }
        public double Y1 { get; }
        public double X2 { get; }
        public double Y2 { get; }
        public double W => X2 - X1;
        public double H => Y2 - Y1;

        public double Left => X1;
        public double Bottom => Y1;
        public double Right => X2;
        public double Top => Y2;

        public RectangleD(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// Перевіряє, чи міститься хоча б одна з кінцевих точок мітки всередині прямокутника.
        /// </summary>
        public bool Contains(CropMark mark)
        {
            return IsPointInside(mark.From.X, mark.From.Y) ||
                   IsPointInside(mark.To.X, mark.To.Y);
        }
        //public bool IsInsideMe(CropMark mark)
        //{

        //    if ((mark.From.X >= X1 && mark.From.X <= X2 && mark.From.Y >= Y1 && mark.From.Y <= Y2) || (mark.To.X >= X1 && mark.To.X <= X2 && mark.To.Y >= Y1 && mark.To.Y <= Y2))
        //    {
        //        return true;
        //    }

        //    return false;
        //}
        /// <summary>
        /// Перевіряє, чи перетинається цей прямокутник з іншим.
        /// </summary>
        public bool IntersectsWith(RectangleD rect)
        {
            return rect.X1 < X2 &&
                    X1 < rect.X2 &&
                    rect.Y1 < Y2 &&
                    Y1 < rect.Y2;
        }

        // Хелпер для чистішого коду
        private bool IsPointInside(double x, double y)
        {
            return x >= X1 && x <= X2 &&
                   y >= Y1 && y <= Y2;
        }
    }
}
