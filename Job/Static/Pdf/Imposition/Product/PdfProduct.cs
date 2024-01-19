using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Product
{
    public class PdfProduct
    {
        public string Number { get;set; }

        public string Name { get;set; }
        public string CustomerName { get;set;}

        public List<PdfProductPart> ProductParts { get; set; } = new List<PdfProductPart>();

        public PdfContent Content { get; set; } = new PdfContent();
       
    }
}
