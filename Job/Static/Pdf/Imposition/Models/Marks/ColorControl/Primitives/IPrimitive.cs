using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks.ColorControl.Primitives
{
    public interface IPrimitive
    {
        string Id { get; set; }
        double Draw(PDFlib p,ColorPalette palette, double x, double y, double w, double h);
    }
}
