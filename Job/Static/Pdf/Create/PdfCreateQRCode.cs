using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити","штрих-код QR код",Icon = "create_qr",Order =10)]
    public class PdfCreateQRCode : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            using (var form = new FormPdfCreateQRCode())
            {
                form.SetTargetFolder(context.FileManager.Settings.CurFolder);
                form.ShowDialog();
            }
        }
    }
}
