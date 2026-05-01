using Interfaces;
using System;
using System.Drawing;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenLine : IScreenPrimitive
    {
        float _x1;
        float _y1;
        float _x2;
        float _y2;

        Pen _pen;


        public ScreenLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (pen == null) throw new ArgumentNullException(nameof(pen));

            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
            _pen = (Pen)pen.Clone();
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

            g.DrawLine(_pen, _x1, _y1, _x2, _y2);

        }
    }
}
