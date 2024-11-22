using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ImposColor
    {
        public string Name { get; set; }
        public MarkColor MarkColor { get; set; }
        public bool IsFront { get; set; } = true;
        public bool IsBack { get; set; } = true;
    }
}
