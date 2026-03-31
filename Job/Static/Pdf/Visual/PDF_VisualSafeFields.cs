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
    [PdfTool("Візуалізація","безпечні поля", Description = "показує безпечні поля документа", Icon = "visual_safe_fields")]
    public class PDF_VisualSafeFields : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            if (context.InputFiles == null || context.InputFiles.Count == 0)
                return false;
            
            using (var form = new FormVisualSafeFields(context.InputFiles[0]))
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
