using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create
{
    public class PdfCreateFillRectangleParams
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Bleeds { get; set; }
        public MarkColor Color { get; set; } = new MarkColor();
        public string Lab { get;set;}

        public override string ToString()
        {
            return $"{Width:0.#}x{Height:0.#}+{Bleeds:0.#}.pdf";
        }
    }
}
