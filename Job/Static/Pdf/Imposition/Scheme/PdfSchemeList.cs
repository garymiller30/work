using Job.Static.Pdf.Imposition.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Scheme
{
    public class PdfSchemeList
    {
        PdfProductPart _part;
        public IEnumerable<PdfMasterScheme> Schemes { get; set; } = new List<PdfMasterScheme>();
        public PdfSchemeList(PdfProductPart part)
        {
            _part = part;
        }

        public void AddScheme(PdfMasterScheme scheme)
        {
            Schemes = Schemes.Append(scheme);
        }
    }
}
