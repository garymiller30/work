using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class PdfMark : MarkAbstract
    {

        public PdfMarkParameters Parameters { get; set; } = new PdfMarkParameters();
        public PdfFile File { get; set; }

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

        public override double GetW()
        {
            return File?.Pages[0].Media.W ?? 0;
        }

        public override double GetH()
        {
            return File?.Pages[0].Media.H ?? 0;
        }
    }
}
