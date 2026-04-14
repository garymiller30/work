using Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenRectangle : IScreenPrimitive
    {
        System.Drawing.Color _color;
        float _pen_width;
        float _x;
        float _y;
        float _w;
        float _h;

        public ScreenRectangle(Pen pen, float x, float y, float w, float h)
        {
            _color = pen.Color;
            _pen_width = pen.Width;
            _x = x;
            _y = y;
            _w = w;
            _h = h;
        }
        public void Draw(Graphics g)
        {
            using (Pen pen = new Pen(_color, _pen_width))
            {
                g.DrawRectangle(pen, _x, _y, _w, _h);
            }
        }
    }
}
