using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual
{
    [PdfTool("Візуалізація","Розрахунок товщини корінця для палітурки",Order = 20,Icon = "visual_cover_spine")]
    public class PDF_Cover_CalcRoot : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormVisualCoverRootCalc())
            {
                form.ShowDialog();
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            
        }
    }
}
