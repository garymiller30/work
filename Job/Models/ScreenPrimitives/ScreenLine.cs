using Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenLine : IScreenPrimitive
    {
        System.Drawing.Color _color;
        float _pen_width;
        float _x1;
        float _y1;
        float _x2;
        float _y2;

        public ScreenLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
            _color = pen.Color;
            _pen_width = pen.Width;
        }

        public void Draw(Graphics g)
        {
            using (Pen pen = new Pen(_color, _pen_width))
            {
                g.DrawLine(pen, _x1, _y1, _x2, _y2);
            }
        }
    }
}
