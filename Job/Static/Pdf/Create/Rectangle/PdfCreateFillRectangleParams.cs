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

        public bool isSpot { get;set;}
        public decimal C { get;set;}
        public decimal M { get;set;}
        public decimal Y { get;set;}
        public decimal K { get;set;}
        public string Name { get;set;}
        public string Lab { get;set;}
    }
}
