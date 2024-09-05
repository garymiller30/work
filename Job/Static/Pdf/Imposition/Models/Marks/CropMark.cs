using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models.Marks
{
    public sealed class CropMark
    {
        public PointD From { get; set; }
        public PointD To { get; set; }
        public bool Enable { get; set; }

        public bool IsFront { get; set; } = true;
        public bool IsBack { get; set; }
    }
}
