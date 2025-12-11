using Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenImage : IScreenPrimitive,IDisposable
    {
        Bitmap _image;
        float _x;
        float _y;
        float _width;
        float _height;

        public ScreenImage(Bitmap img, float x, float y, float w, float h)
        {
            _image = new Bitmap(img);
            _x = x;
            _y = y;
            _width = w;
            _height = h;
        }

        public void Dispose()
        {
            _image?.Dispose();
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(_image, _x, _y, _width, _height);
        }
    }
}
