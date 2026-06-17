using JobSpace.Static.Pdf.Imposition.Services;
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

        public override double GetH(TextVariablesService textVariablesService)
        {
            return (double)GetSize(textVariablesService).Height;
        }

        public override double GetW(TextVariablesService textVariablesService)
        {
            return (double)GetSize(textVariablesService).Width;
        }

        SizeF GetSize(TextVariablesService textVariablesService)
        {
            var txt = new StringToken(this, textVariablesService).GetRawString();
            if (string.IsNullOrEmpty(txt)) return SizeF.Empty;

            // Використовуємо стандартний Font
            using var font = new Font(FontName, (float)FontSize, FontStyle.Regular, GraphicsUnit.Point);
            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);

            // Рахуємо суто в пікселях (Pixel), щоб GDI+ не округлював міліметри з похибкою
            graphics.PageUnit = GraphicsUnit.Pixel;

            // Антиаліасинг та GenericTypographic ПРИБИРАЮТЬ магічні відступи по боках
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var stringFormat = StringFormat.GenericTypographic;

            // Отримуємо чистий розмір символів у пікселях
            SizeF pixelSize = graphics.MeasureString(txt, font, PointF.Empty, stringFormat);

            // Переводимо пікселі в міліметри вручну на основі системного DPI (зазвичай 96)
            // Формула: (пікселі / DPI) * 25.4 мм в одному дюймі
            float dpi = graphics.DpiX;
            const float mmPerInch = 25.4f;

            float widthInMm = (pixelSize.Width / dpi) * mmPerInch;
            float heightInMm = (pixelSize.Height / dpi) * mmPerInch;

            return new SizeF(widthInMm, heightInMm);
        }
    }
}
