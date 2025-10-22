using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public class Group : IPrimitive
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public double Width { get; set; }
        public double Height { get; set; }

        public List<IPrimitive> Items { get; set; } = new List<IPrimitive>();

        public Group()
        {
            
        }
        public Group(double w,double h)
        {
            Width = w; Height = h;
        }

        public void Add(params string[] colorId)
        {
            Items.AddRange(colorId.Select(x =>
            new Rectangle() 
            { 
                FillId = x, 
                Coord = new PointD { X = 0, Y = 0 }, 
                W = Width, 
                H = Height 
                }
            ));
        }
        public void Add(string baseColorId, double tint)
        {
            Items.Add( new Rectangle()
            {
                FillId = baseColorId,
                Tint = tint,
                Coord = new PointD { X = 0, Y = 0 },
                W = Width,
                H = Height
            });
        }
        public void Add(params IPrimitive[] primitives)
        {
            Items.AddRange(primitives);
        }

        public double Draw(PDFlib p, ColorPalette palette, double x, double y, double w, double h)
        {
            var curX = x;

            foreach(var primitive in Items)
            {
                curX = primitive.Draw(p,palette, curX, y, w, h);
            }
            return curX;
        }
    }
}
