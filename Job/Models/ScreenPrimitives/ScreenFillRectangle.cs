using Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenFillRectangle : IScreenPrimitive, IDisposable
    {
        float _x;
        float _y;
        float _w;
        float _h;
        Brush _brush;

        public ScreenFillRectangle(Brush brush, float x, float y, float w, float h)
        {
            _x = x;
            _y = y;
            _w = w;
            _h = h;
            _brush = brush.Clone() as Brush;
        }

        public void Dispose()
        {
            _brush.Dispose();
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(_brush, _x, _y, _w, _h);
        }
    }
}
