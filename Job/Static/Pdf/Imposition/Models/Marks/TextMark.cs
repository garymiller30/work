using System;
using System.Collections.Generic;
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
            // Assuming 1 point = 0.352778 mm
            return FontSize * pointToMm;
        }

        public override double GetW()
        {
            using (var graphics = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1)))
            {
                var font = new System.Drawing.Font(FontName, (float)FontSize);
                var size = graphics.MeasureString(Text, font);
                return size.Width * pointToMm; // Convert points to mm
            }
        }
    }
}
