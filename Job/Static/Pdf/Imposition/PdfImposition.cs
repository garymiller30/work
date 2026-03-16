using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF;
using System.Linq;

namespace JobSpace.Static.Pdf.Imposition
{
    [PdfTool("", "Спуск полос",Order = 2,Icon ="imposition")]
    public class PdfImposition : IPdfTool
    {
        ImposInputParam Parameters { get; set; } = new ImposInputParam();
        public bool Configure(PdfJobContext context)
        {
            Parameters.UserProfile = context.UserProfile;
            Parameters.JobFolder = context.FileManager.Settings.CurFolder;
            Parameters.Job = context.UserProfile.Jobs?.CurrentJob;
            Parameters.Files = context.InputFiles.Select(x=>x.FullName).ToList();
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            var form = new FormPdfImposition(Parameters);
            form.Show();
        }
    }
}
