using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models.Marks
{
    public class TextMarkParameters
    {
        public double Xofs { get; set; } = 0;
        public double Yofs { get; set; } = 0;

        public AnchorOfset AnchorOfset { get; set; } = new AnchorOfset();
        public AnchorPoint MarkAnchorPoint { get; set; } = AnchorPoint.BottomLeft;
        public AnchorPoint ParentAnchorPoint { get; set; } = AnchorPoint.BottomLeft;
        public bool IsFront { get; set; } = true;
        public bool IsBack { get; set; } = true;
        public bool IsBackMirrored { get; set; }
    }
}
