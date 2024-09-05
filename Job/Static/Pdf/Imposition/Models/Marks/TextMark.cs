using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models.Marks
{
    public class TextMark
    {
        public PointD Front { get; set; }
        public PointD Back { get; set; }
        public string Text { get; set; }
        public double Angle { get; set; }
        public double FontSize { get; set; } = 12;
        public string FontName { get; set; } = "Arial";
        public MarkColor Color { get; set; } = new MarkColor();

        public TextMarkParameters Parameters { get; set; } = new TextMarkParameters();

        public double GetH()
        {
            return 0;
        }

        public double GetW()
        {
            return 0;
        }
    }
}
