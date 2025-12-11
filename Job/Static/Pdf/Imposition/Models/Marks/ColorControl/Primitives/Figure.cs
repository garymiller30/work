using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public class Figure : IPrimitive
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<IPrimitive> Primitives { get; set; } = new List<IPrimitive>();


        public Figure()
        {

        }

        public double Draw(PDFlib p, ColorPalette palette, double x, double y, double w, double h)
        {
            foreach (var i in Primitives)
            {
                i.Draw(p, palette, x, y, w, h);
            }

            return x+w;
        }
    }
}
