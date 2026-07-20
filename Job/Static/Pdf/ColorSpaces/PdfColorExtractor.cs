using iText.IO.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.ColorSpaces
{
    public class PdfColorExtractor
    {
        public static List<string> ExtractColorsFromPage(PdfPage page)
        {
            List<string> colors = new List<string>();

            if (page == null)
            {
                return colors;
            }

            try
            {
                ColorExtractionListener listener = new ColorExtractionListener();
                PdfCanvasProcessor processor = new PdfCanvasProcessor(listener);
                processor.ProcessPageContent(page);
                colors.AddRange(listener.GetUniqueColors());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сталася помилка під час обробки сторінки PDF: {ex.Message}");
            }

            return colors;
        }
    }
}
