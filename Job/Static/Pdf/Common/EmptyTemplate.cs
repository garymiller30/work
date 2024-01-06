using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Common
{
    public sealed class EmptyTemplate
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public int Count { get; set; }
        public int Multiplier { get; set; }
        public bool IsValidated()
        {
            if (Width == 0 || Height == 0 || Count == 0 || Multiplier == 0) return false;
            return true;
        }
    }
}
