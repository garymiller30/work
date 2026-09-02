using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Personalization
{
    [PdfTool("Персоналізація", "PDF з CSV/TSV", Icon = "personalization", Description = "Персоналізація PDF незалежними шарами з попереднім переглядом", Order = 1)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public sealed class PdfPersonalizationTool : IPdfTool
    {
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".json", ".pdf"
    };

        private PdfPersonalizationSettings? _settings;

        public bool Configure(PdfJobContext context)
        {

            // Отримуємо перший файл одразу через pattern matching (C# 11+)
            if (context?.InputFiles?.FirstOrDefault() is not { } firstFile)
                return false;

            // Швидка перевірка розширення
            var ext = Path.GetExtension(firstFile.FileInfo.FullName); // Безпечніше брати розширення з FullName або імені
            if (!AllowedExtensions.Contains(ext))
                return false;

            // Спрощений `using` (C# 8+)
            using var form = new FormPdfPersonalization(firstFile.FullName);
            if (form.ShowDialog() != DialogResult.OK)
                return false;

            _settings = form.Settings;
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            if (_settings != null)
            {
                new PdfPersonalizationRenderer().Render(_settings);
            }
        }
    }
}
