using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public class Rectangle : PrimitiveAbstract, IPrimitive
    {
        public PointD Coord { get; set; }
        public double W { get; set; }
        public double H { get; set; }

        public double Draw(PDFlib p,ColorPalette palette, double x, double y, double w, double h)
        {
            double tint = (Tint / 100);

            PdfHelper.SetFillStroke(p, palette,this);
            p.rect(x * PdfHelper.mn, y * PdfHelper.mn, w * PdfHelper.mn,h * PdfHelper.mn);
            PdfHelper.CloseFillStroke(p,this);

            return x+w;
        }
    }
}
