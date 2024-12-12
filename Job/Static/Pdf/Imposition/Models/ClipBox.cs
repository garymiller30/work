using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ClipBox
    {
        public double Default { get;set; } 
        public double Right { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }

        public void SetDefault(double bleed)
        {
            Default = bleed;
            Set(bleed);
        }

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

        public ClipBox Copy()
        {
            ClipBox box = new ClipBox();
            box.Set(this);
            return box;
        }

        public void Set(decimal all)
        {
            var b = (double)all;
            Right = b;
            Left = b;
            Top = b;
            Bottom = b;
        }

        public ClipBox()
        {

        }
    }
}
