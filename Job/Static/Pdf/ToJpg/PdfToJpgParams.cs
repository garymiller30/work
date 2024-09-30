using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.ToJpg
{
    public sealed class PdfToJpgParams
    {
        public int Dpi { get; set; } = 96;
        public long Quality { get; set; } = 100;
    }
}
