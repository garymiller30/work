using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models.Marks
{
    public class PdfMark
    {
        public PointD Front { get; set; }
        public PointD Back { get; set; }

        public PdfMarkParameters Parameters { get; set; } = new PdfMarkParameters();
        public PdfFile File { get; set; }
        public double Angle { get; set; } = 0;

        public PdfMark()
        {

        }

        public PdfMark(PdfFile file)
        {
            File = file;
        }

        public PdfMark(string filePath)
        {
            File = new PdfFile(filePath);
        }

        public double GetW()
        {
            return File.Pages[0].Media.W;
        }

        public double GetH()
        {
            return File.Pages[0].Media.H;
        }
    }
}
