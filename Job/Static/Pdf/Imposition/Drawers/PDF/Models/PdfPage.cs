using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Models
{
    public sealed class PdfPage
    {
        public double W {  get; set; }
        public double H { get; set; }

        public List<PdfPagePlacer> pagePlacers { get; set; } = new List<PdfPagePlacer>();

        public PdfPage(ProductPart productPart)
        {
            if (productPart.TemplatePlate.IsLikePaperFormat)
            {
                W = productPart.Sheet.W + productPart.Sheet.ExtraSpace*2;
                H = productPart.Sheet.H + productPart.Sheet.ExtraSpace*2;
            }
            else
            {
                W = productPart.TemplatePlate.W;
                H = productPart.TemplatePlate.H;
            }
        }
    }
}
