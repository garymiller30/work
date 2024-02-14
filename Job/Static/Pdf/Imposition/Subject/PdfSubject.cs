using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Scheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Subject
{
    public class PdfSubject
    {
        public List<PdfScheme> Schemes { get; set; } = new List<PdfScheme>();

        public void AddScheme(PdfScheme scheme)
        {
            Schemes.Add(scheme);
        }


        internal PdfFormat GetFormat()
        {
            PdfFormat format = new PdfFormat();
            format.Width = Schemes.Where(x => x.ColumnIdx == 0).Sum(x => x.GetRotatedWidth());
            format.Height = Schemes.Where(x=>x.RowIdx == 0).Sum(x=>x.GetRotatedHeight());
            return format;
        }
    }
}
