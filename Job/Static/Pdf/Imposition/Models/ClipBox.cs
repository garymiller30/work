using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models
{
    public class ClipBox
    {
        public double Right { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }

        public void Set(double all)
        {
            Right = all;
            Left = all;
            Top = all;
            Bottom = all;
        }

        public void Set(ClipBox box)
        {
            Right = box.Right;
            Left = box.Left;
            Top = box.Top;
            Bottom = box.Bottom;

        }
        public ClipBox()
        {

        }
        public ClipBox(double value)
        {
            Set(value);
        }

        public void Set(double right, double left, double top, double bottom)
        {
            Right = right;
            Left = left;
            Top = top;
            Bottom = bottom;
        }
    }
}
