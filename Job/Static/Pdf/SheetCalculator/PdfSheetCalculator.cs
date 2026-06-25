using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.SheetCalculator.Views;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.SheetCalculator
{
    [PdfTool("", "Калькулятор розкладки листів", Order = 55, Icon = "layout_calculator", Description = "Система розрахунку тиражів та розкладки виробів на друкарських листах")]
    public class PdfSheetCalculator : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            var form = new FormSheetCalculator();
            form.Show();
        }
    }
}
