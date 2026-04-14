using JobSpace.Static.Pdf.Common;
using Org.BouncyCastle.Math.Field;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public class Circle : PrimitiveAbstract, IPrimitive
    {
        public PointD Center { get; set; }
        public double Radius { get; set; }
        public double StrokeLen { get; set; } = 0.2;

        public double Draw(PDFlib p,ColorPalette palette, double x, double y, double w, double h)
        {
            PdfHelper.SetFillStroke(p, palette, this);
            p.setlinewidth(StrokeLen);
            p.circle((Center.X + x) * PdfHelper.mn, (Center.Y + y) * PdfHelper.mn, Radius * PdfHelper.mn);

            PdfHelper.CloseFillStroke(p, this);

            return x+w;
        }
    }
}
