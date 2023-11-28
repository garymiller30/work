using Job.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Convert
{
    public sealed class PdfConvertParams
    {
        public Box TrimBox { get; set; } = new Box();
    }
}
