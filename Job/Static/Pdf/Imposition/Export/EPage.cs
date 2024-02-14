using Job.Static.Pdf.Common;
using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Product;
using Job.Static.Pdf.Imposition.Sheet;
using Job.Static.Pdf.Imposition.Subject;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Export
{
    public class EPage : IDisposable
    {
        PDFlib p = null;
        PdfProduct _product;
        PdfSheetRun _sheet;

        public EPage(PdfProduct product, PDFlib p, PdfSheetRun curSheet)
        {
            this.p = p;
            _product = product;
            _sheet = curSheet;

            p.begin_page_ext(_sheet.Width * PdfHelper.mn, _sheet.Height * PdfHelper.mn, "");
        }

        public void DrawSubject()
        {
            // потрібно взнати початок координат
            PdfFormat format = _sheet.Subject.GetFormat();
            
            double x=0;
            double y=0;

            if (_sheet.SubjectSettings.SubjectPosition.CenterHorizontally)
            {
                x = (_sheet.Width - format.Width) / 2;
            }
            else
            {
                x = _sheet.SubjectSettings.SubjectPosition.XOffset;
            }
            
            if (_sheet.SubjectSettings.SubjectPosition.CenterVertically)
            {
                y = (_sheet.Height - format.Height) / 2;
            }
            else
            {
                y = _sheet.SubjectSettings.SubjectPosition.YOffset;
            }
        }

        public void Dispose()
        {
            p.end_page_ext("");
        }
    }
}
