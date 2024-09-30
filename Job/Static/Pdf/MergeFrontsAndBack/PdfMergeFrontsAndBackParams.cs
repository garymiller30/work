using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.MergeFrontsAndBack
{
    public sealed class PdfMergeFrontsAndBackParams
    {
        public string[] FrontsFiles { get;set; }
        public string BackFile { get;set; }
    }
}
