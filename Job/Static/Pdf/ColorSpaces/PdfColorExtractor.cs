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

        public static List<string> ExtractColorsFromPage(string pdfPath, int pageNumber)
        {
            List<string> colors = new List<string>();
            PdfDocument pdfDoc = null;

            try
            {
                pdfDoc = new PdfDocument(new PdfReader(pdfPath));

                if (pageNumber < 1 || pageNumber > pdfDoc.GetNumberOfPages())
                {
                    Console.WriteLine($"Помилка: Номер сторінки {pageNumber} недійсний для документа.");
                    return colors; // Повернути порожній список
                }

                PdfPage page = pdfDoc.GetPage(pageNumber);
                colors.AddRange(ExtractColorsFromPage(page));
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Помилка читання файлу: {ioEx.Message}");
                // Додаткова обробка помилки, якщо потрібно
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сталася помилка під час обробки PDF: {ex.Message}");
                // Додаткова обробка помилки
            }
            finally
            {
                pdfDoc?.Close(); // Завжди закриваємо документ
            }

            return colors;
        }
    }
}
