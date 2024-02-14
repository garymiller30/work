using Job.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Content
{
    public class PdfContentFile
    {
        public string FilePath { get; private set; }
        public List<PdfPageInfo> Pages { get;set; } = new List<PdfPageInfo>();

        public PdfContentFile(string filePath)
        {
            FilePath = filePath;
            Pages.AddRange(PdfHelper.GetPagesInfo(filePath));
        }
    }
}
