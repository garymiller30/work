using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Content
{
    public class PdfContent
    {
        public List<PdfContentFile> ContentFiles { get; set; } = new List<PdfContentFile>();

        public void Add(string pdfFile)
        {
            PdfContentFile file = new PdfContentFile(pdfFile);
            ContentFiles.Add(file);
        }
    }
}
