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
    [PdfTool("Візуалізація", "Виштовхування при кріпленні на скобу", Icon = "visual_saddle_stitch_creep", Order = 20)]
    public class PDF_VisualSaddleStitchCreep : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            var file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                using (var form = new FormVisualSaddleStitchCreep(file))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Execute(PdfJobContext context)
        {

        }

    }
}
