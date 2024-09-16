using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Drawers.PDF.Models
{
    public class PdfPagePlacer
    {
        public string FileName { get; set; }
        public int PageNo { get;set; }

        public PointD Coord { get; set; }
        public ClipBox Clipping { get; set; } = new ClipBox();

        public double Angle { get;set; }

    }
}
