using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models.Marks
{
    public sealed class CropMarksParam
    {
        public double Len { get; set; } = 5;
        public double Distance { get; set; } = 2;
        public double Height { get; set; } = 0.5;
    }
}
