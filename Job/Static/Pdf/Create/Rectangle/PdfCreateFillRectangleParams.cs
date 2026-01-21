using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.Rectangle
{
    public class PdfCreateFillRectangleParams
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Bleeds { get; set; }
        public MarkColor Color { get; set; } = new MarkColor();
        public string Lab { get;set;}
    }
}
