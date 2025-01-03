using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class TextMark : MarkAbstract
    {
        public string Text { get; set; }
        public double FontSize { get; set; } = 12;
        public string FontName { get; set; } = "Arial";
        public MarkColor Color { get; set; } = new MarkColor();

        public TextMarkParameters Parameters { get; set; } = new TextMarkParameters();

        public override double GetH()
        {
            return 3;
        }

        public override double GetW()
        {
            return 6;
        }
    }
}
