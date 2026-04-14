using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF.Visual;
using System.Linq;

namespace JobSpace.Static.Pdf.Visual
{
    [PdfTool("Візуалізація","настільний календар",Icon = "visual_table_calendar",Order = 20)]
    public class PDF_VisualTableCalendar : IPdfTool
    {
        IFileSystemInfoExt file;

        public bool Configure(PdfJobContext context)
        {
            file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                return true;
            }
            return false;
        }
        public void Execute(PdfJobContext context)
        {
            using (var form = new FormVisualTableCalendar(file))
            {
                form.ShowDialog();
            }
        }
    }
}
