using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition.Models.AutoImpos;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.UserForms.PDF;
using System.Linq;

namespace JobSpace.Static.Pdf.Imposition
{
    [PdfTool("", "Спуск полос", Order = 50, Icon = "imposition")]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public class PdfImposition : IPdfTool
    {
        ImposInputParam Parameters { get; set; } = new ImposInputParam();
        AutoImposMatch AutoImposMatch { get; set; }

        public bool Configure(PdfJobContext context)
        {
            Parameters.UserProfile = context.UserProfile;
            Parameters.JobFolder = context.FileManager.Settings.CurFolder;
            Parameters.Job = context.UserProfile.Jobs?.CurrentJob;
            Parameters.Files = context.InputFiles.Select(x => x.FullName).ToList();

            var profile = context.UserProfile as Profile;
            if (profile != null)
            {
                var autoImposService = new AutoImposService(profile);
                AutoImposMatch = autoImposService.TryFindBestMatch(Parameters, context.InputFiles);
            }

            return true;
        }

        public void Execute(PdfJobContext context)
        {
            var form = new FormPdfImposition(Parameters);
            if (AutoImposMatch?.HasTemplate == true)
            {
                form.ApplyAutoImposTemplate(AutoImposMatch);
            }

            form.Show();
        }
    }
}
