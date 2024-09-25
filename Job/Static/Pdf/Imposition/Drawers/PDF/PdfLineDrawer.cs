using Job.Static.Pdf.Common;
using Job.Static.Pdf.Imposition.Models.Marks;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Drawers
{
    public class PdfLineDrawer
    {
        PDFlib _p;
        double _baseX;
        double _baseY;
        public PdfLineDrawer(PDFlib p)
        {
            _p = p;
        }

        public PdfLineDrawer(PDFlib p, double baseX, double baseY)
        {
            _p = p;
            _baseX = baseX;
            _baseY = baseY;
        }

        public PdfLineDrawer From(double xOfs, double yOfs)
        {
            double x = _baseX + xOfs;
            double y = _baseY + yOfs;

            _p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);

            _baseX = x;
            _baseY = y;

            return this;
        }

        public void To(double xOfs, double yOfs)
        {
            _p.lineto((_baseX + xOfs) * PdfHelper.mn, (_baseY + yOfs) * PdfHelper.mn);
            _p.stroke();

        }

        public static void DrawCropMark(PDFlib p, CropMark mark)
        {

            p.moveto(mark.From.X * PdfHelper.mn, mark.From.Y * PdfHelper.mn);
            p.lineto(mark.To.X * PdfHelper.mn, mark.To.Y * PdfHelper.mn);
            p.stroke();
        }
    }
}
