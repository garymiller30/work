using JobSpace.Static.Pdf.SetTrimBox.ByBleed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class PdfFilePage
    {
        public int Idx { get; set; }
        public PdfBox Media { get; set; } = new PdfBox();
        public PdfBox Trim { get; set; } = new PdfBox();
        public PdfBox Crop { get; set; } = new PdfBox();
        public double Bleed { get => (Media.W -  Trim.W)/2; }
    }
}
