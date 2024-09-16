using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models.Marks
{
    public class MarksContainer
    {
        public List<PdfMark> Pdf { get; set; } = new List<PdfMark>();
        public List<TextMark> Text { get; set; } = new List<TextMark>();

        public void Add(params PdfMark[] marks)
        {
            Pdf.AddRange(marks);
        }
        public void Add(params TextMark[] marks)
        {
            Text.AddRange(marks);
        }
    }
}
