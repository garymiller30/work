using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public class Line : PrimitiveAbstract, IPrimitive
    {
        public PointD From { get; set; }
        public PointD To { get; set; }

        public double StrokeLen { get; set; } = 0.2;

        public double Draw(PDFlib p, ColorPalette palette, double x, double y, double w, double h)
        {
            PdfHelper.SetFillStroke(p, palette, this);

            p.setlinewidth(StrokeLen);
            p.moveto((From.X + x) * PdfHelper.mn, (From.Y + y) * PdfHelper.mn);
            p.lineto((To.X + x) * PdfHelper.mn, (To.Y + y) * PdfHelper.mn);

            PdfHelper.CloseFillStroke(p,this);

            return x+w;
        }
    }
}
