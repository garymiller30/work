using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Logger;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Manipulation
{
    [PdfTool("Маніпуляції з файлом", "MediaBox = TrimBox", Icon = "pdf_trimbox", Description = "Створити копію PDF, у якій MediaBox кожної сторінки дорівнює TrimBox", Order = 5, IsBackgroundTask = true)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public sealed class PdfSetMediaBoxToTrimBox : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            if (context.InputFiles.Any(IsPdfFile))
            {
                return true;
            }

            MessageBox.Show(
                "Виберіть хоча б один PDF файл.",
                "MediaBox = TrimBox",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles.Where(IsPdfFile))
            {
                try
                {
                    SetMediaBoxToTrimBox(file.FullName);
                }
                catch (BadPasswordException ex)
                {
                    Log.Error(this, "PdfSetMediaBoxToTrimBox", $"Не вдалося відкрити файл з паролем \"{file.FullName}\": {ex.Message}");
                }
                catch (Exception ex)
                {
                    Log.Error(this, "PdfSetMediaBoxToTrimBox", $"Не вдалося змінити MediaBox для \"{file.FullName}\": {ex.Message}");
                }
            }
        }

        public string SetMediaBoxToTrimBox(string filePath)
        {
            string outputPath = GetOutputPath(filePath);

            using (var reader = new PdfReader(filePath).SetUnethicalReading(true))
            using (var writer = new PdfWriter(outputPath, new WriterProperties().UseSmartMode()))
            using (var pdfDocument = new PdfDocument(reader, writer))
            {
                int pageCount = pdfDocument.GetNumberOfPages();

                for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
                {
                    PdfPage page = pdfDocument.GetPage(pageNumber);
                    Rectangle trimBox = page.GetTrimBox();

                    if (trimBox == null)
                    {
                        Log.Warning(this, "PdfSetMediaBoxToTrimBox", $"Сторінка {pageNumber} у \"{filePath}\" не має TrimBox. MediaBox не змінено.");
                        continue;
                    }

                    page.SetMediaBox(new Rectangle(trimBox.GetX(), trimBox.GetY(), trimBox.GetWidth(), trimBox.GetHeight()));
                }
            }

            return outputPath;
        }

        private static bool IsPdfFile(Interfaces.IFileSystemInfoExt file)
        {
            return file?.FileInfo != null &&
                   !file.IsDir &&
                   string.Equals(file.FileInfo.Extension, ".pdf", StringComparison.InvariantCultureIgnoreCase);
        }

        private static string GetOutputPath(string filePath)
        {
            string directory = System.IO.Path.GetDirectoryName(filePath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string outputPath = System.IO.Path.Combine(directory, $"{fileName}_mediabox_trimbox.pdf");

            if (!File.Exists(outputPath))
            {
                return outputPath;
            }

            int index = 1;
            do
            {
                outputPath = System.IO.Path.Combine(directory, $"{fileName}_mediabox_trimbox_{index}.pdf");
                index++;
            }
            while (File.Exists(outputPath));

            return outputPath;
        }
    }
}
