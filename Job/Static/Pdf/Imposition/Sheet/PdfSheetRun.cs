using Job.Static.Pdf.Imposition.Common;
using Job.Static.Pdf.Imposition.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Sheet
{
    public class PdfSheetRun : PdfMasterSheet
    {
        PdfMasterSheet _sheet;

        public PdfSideEnum Side { get; set; } = PdfSideEnum.Front;

        public PdfSubject Subject { get; internal set; }

        public PdfSheetRun(PdfMasterSheet masterSheet)
        {
            _sheet = masterSheet;
            Width = masterSheet.Width;
            Height = masterSheet.Height;
            Gaps = masterSheet.Gaps;
            SheetWorkStyle = masterSheet.SheetWorkStyle;
            SubjectSettings = masterSheet.SubjectSettings;
        }
    }
}
