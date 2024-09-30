using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class PDFLIBDocument : IDisposable
    {
        PDFlib _p;
        string _filePath;
        int _doc;

        public PDFLIBDocument(PDFlib p, string filePath, string optlist = "")
        {
            _p = p;
            _filePath = filePath;
            _doc = p.open_pdi_document(filePath, optlist);
        }


        public void fit_pdi_page(int pageNo, double x, double y, string optlist = "")
        {
            int page = _p.open_pdi_page(_doc, pageNo, "");

            _p.fit_pdi_page(page, x * PdfHelper.mn, y * PdfHelper.mn, optlist);

            _p.close_pdi_page(page);
        }

        public void Dispose()
        {
            _p.close_pdi_document(_doc);
        }
    }
}
