using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class PdfMarkParameters 
    {
        public double Xofs { get; set; } = 0;
        public double Yofs { get; set; } = 0;

        public AnchorPoint MarkAnchorPoint { get; set; } = AnchorPoint.Center;
        public AnchorPoint ParentAnchorPoint { get; set; } = AnchorPoint.Center;

        public bool IsFront { get; set; } = true;
        public bool IsBack { get; set; } = true;
        public bool IsBackMirrored { get; set; } = true;
    }
}
