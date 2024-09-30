using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.ExtractPages
{
    public sealed class PdfExtractPagesParams
    {
        public int[] Pages { get; set; } = new int[1]{ 1};
    }
}
