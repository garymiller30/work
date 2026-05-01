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
        Pen _pen;
        float _x;
        float _y;
        float _w;
        float _h;

        public ScreenRectangle(Pen pen, float x, float y, float w, float h)
        {
            if (pen == null) throw new ArgumentNullException(nameof(pen));
            _pen = (Pen)pen.Clone();
            _x = x;
            _y = y;
            _w = w;
            _h = h;
        }

        public void Dispose()
        {
            if (_pen != null)
            {
                _pen.Dispose();
                _pen = null;
            }
        }

        public void Draw(Graphics g)
        {
            if (_pen == null)
                throw new ObjectDisposedException(nameof(ScreenLine));
            g.DrawRectangle(_pen, _x, _y, _w, _h);
        }
    }
}
