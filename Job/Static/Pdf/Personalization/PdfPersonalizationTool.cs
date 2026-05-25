using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Personalization
{
    [PdfTool("Персоналізація", "PDF з CSV/TSV", Icon = "personalization", Description = "Персоналізація PDF незалежними шарами з попереднім переглядом", Order = 1)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public sealed class PdfPersonalizationTool : IPdfTool
    {
        private PdfPersonalizationSettings _settings;

        public bool Configure(PdfJobContext context)
        {
            string basePdf = context.InputFiles
                .FirstOrDefault(x => string.Equals(Path.GetExtension(x.FullName), ".pdf", System.StringComparison.OrdinalIgnoreCase))
                ?.FullName;

            using (var form = new FormPdfPersonalization(basePdf))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _settings = form.Settings;
                    return true;
                }
            }

            return false;
        }

        public void Execute(PdfJobContext context)
        {
            new PdfPersonalizationRenderer().Render(_settings);
        }
    }
}
