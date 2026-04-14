using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public class Triangle : PrimitiveAbstract, IPrimitive
    {
        public PointD Point1 { get; set; }
        public PointD Point2 { get; set; }
        public PointD Point3 { get; set; }


        public Triangle()
        {
            
        }

        public Triangle(PointD p1, PointD p2, PointD p3)
        {
            Point1 = p1;
            Point2 = p2;
            Point3 = p3;
        }

        public Triangle(double p1x, double p1y,double p2x, double p2y, double p3x, double p3y)
        {
            Point1 = new PointD(p1x,p1y);
            Point2 = new PointD(p2x,p2y);
            Point3 = new PointD(p3x,p3y);
        }

        public double Draw(PDFlib p, ColorPalette palette, double x, double y, double w, double h)
        {
            PdfHelper.SetFillStroke(p,palette, this);

            p.moveto((Point1.X + x) * PdfHelper.mn, (Point1.Y + y) * PdfHelper.mn);
            p.lineto((Point2.X + x) * PdfHelper.mn, (Point2.Y + y) * PdfHelper.mn);
            p.lineto((Point3.X + x) * PdfHelper.mn, (Point3.Y + y) * PdfHelper.mn);

            PdfHelper.CloseFillStroke(p,this);

            return x+w;
        }
    }
}
