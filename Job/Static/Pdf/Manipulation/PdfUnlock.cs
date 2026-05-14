using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using Logger;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Manipulation
{
    [PdfTool("Маніпуляції з файлом", "Розблокувати PDF", Icon = "pdf_unlock", Description = "Створити копію PDF без обмежень на друк або редагування", Order = 5, IsBackgroundTask = true)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public sealed class PdfUnlock : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            if (context.InputFiles.Any(IsPdfFile))
            {
                return true;
            }

            MessageBox.Show(
                "Виберіть хоча б один PDF файл.",
                "Розблокувати PDF",
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
                    Unlock(file.FullName);
                }
                catch (BadPasswordException ex)
                {
                    Log.Error(this, "PdfUnlock", $"Не вдалося відкрити файл з паролем \"{file.FullName}\": {ex.Message}");
                }
                catch (Exception ex)
                {
                    Log.Error(this, "PdfUnlock", $"Не вдалося розблокувати \"{file.FullName}\": {ex.Message}");
                }
            }
        }

        public string Unlock(string filePath)
        {
            string outputPath = GetOutputPath(filePath);

            using (var reader = new PdfReader(filePath).SetUnethicalReading(true))
            using (var writer = new PdfWriter(outputPath, new WriterProperties().UseSmartMode()))
            using (var pdfDocument = new PdfDocument(reader, writer))
            {
                pdfDocument.GetCatalog().GetPdfObject().Remove(PdfName.Perms);
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
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string outputPath = Path.Combine(directory, $"{fileName}_unlocked.pdf");

            if (!File.Exists(outputPath))
            {
                return outputPath;
            }

            int index = 1;
            do
            {
                outputPath = Path.Combine(directory, $"{fileName}_unlocked_{index}.pdf");
                index++;
            }
            while (File.Exists(outputPath));

            return outputPath;
        }
    }
}
