using Job.Static.Pdf.Imposition.Product;
using Job.Static.Pdf.Imposition.Sheet;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Export
{
    public class EDocument : IDisposable
    {
        string _outputPath;
        PdfProduct _product;
        PDFlib p;

        public EDocument(PdfProduct pdfProduct, string outputPath)
        {
            _product = pdfProduct;
            p = new PDFlib();
            p.begin_document(outputPath, "");
            _outputPath = outputPath;   
        }

        public void Dispose()
        {
            p.end_document("");
            p?.Dispose();
        }


        internal EPage AddPage(PdfSheetRun sheet)
        {
            return new EPage(_product, p, sheet);
        }
    }
}
