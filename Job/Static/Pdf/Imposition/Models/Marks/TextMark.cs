using JobSpace.Static.Pdf.Imposition.Services.TextVariables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class TextMark : MarkAbstract
    {
        const double pointToMm = 0.352778;
        public string Text { get; set; }
        public double FontSize { get; set; } = 12;
        public string FontName { get; set; } = "Arial";
        public MarkColor Color { get; set; } = new MarkColor();

        public TextMarkParameters Parameters { get; set; } = new TextMarkParameters();

        public override double GetH()
        {
            return (double)GetSize().Height;
        }

        public override double GetW()
        {
            return (double)GetSize().Width;
        }

        SizeF GetSize()
        {
            using (var graphics = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1)))
            {
                graphics.PageUnit = System.Drawing.GraphicsUnit.Millimeter;
                var font = new System.Drawing.Font(FontName, (float)FontSize);
                var txt = new StringToken(this).GetRawString();
                var size = graphics.MeasureString(txt, font);
                return size; 
            }
        }
    }
}
