using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class PdfMark
    {
        public string Id {get;set; } = Guid.NewGuid().ToString();
        public string Name {get;set; }
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
            return File?.Pages[0].Media.W ?? 0;
        }

        public double GetH()
        {
            return File?.Pages[0].Media.H ?? 0;
        }
    }
}
