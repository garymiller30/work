using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Sheet
{
    public class PdfSheet
    {
        public double Width { get;set; }
        public double Height { get;set; }

        public Fields Gaps { get; set; } = new Fields();
        public SheetWorkStyleEnum SheetWorkStyle { get; set; } = SheetWorkStyleEnum.SingleSide;
        public PdfSubjectSettings SubjectSettings { get; set; } = new PdfSubjectSettings();
    }
}
