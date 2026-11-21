using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF.Visual;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Visual
{
    [PdfTool(
        "Візуалізація",
        "конструктор квартального календаря",
        Description = "Конструктор візуалізації квартального календаря з окремих PDF-компонентів, пружин та люверса",
        Icon = "quartal_calendar",
        Order = 25)]
    public class PDF_VisualQuarterlyCalendar : IPdfTool
    {
        private PdfJobContext? _context;

        public bool Configure(PdfJobContext context)
        {
            _context = context;
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            if (context?.InputFiles != null && context.InputFiles.Count > 0)
            {
                using var form = new FormVisualQuarterlyCalendar(context.InputFiles);
                form.ShowDialog();
            }
            else
            {
                using var form = new FormVisualQuarterlyCalendar();
                form.ShowDialog();
            }
        }
    }
}
