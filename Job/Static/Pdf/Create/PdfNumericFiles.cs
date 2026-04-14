using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.UserForms.PDF;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити", "нумеровані файли (1. 2. 3...)", Order = 10, Icon = "numeric_files")]
    public class PdfNumericFiles : IPdfTool
    {
        List<IFileSystemInfoExt> sortedFiles;
        int startNumber = 1;
        int cntNumbers = 1;
        public bool Configure(PdfJobContext context)
        {
            if (context.InputFiles.Count > 1)
            {
                using (var form = new FormCreateNumericFiles(context.InputFiles))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        sortedFiles = form.SortedFiles;
                        startNumber = form.StartFrom - 1;
                        cntNumbers = form.CntNumbers;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {

            for (int i = 0; i < sortedFiles.Count; i++)
            {
                File.Move(
                    sortedFiles[i].FullName,
                    Path.Combine(
                        Path.GetDirectoryName(sortedFiles[i].FullName), $"{(startNumber + i).ToString($"D0{cntNumbers}", CultureInfo.InvariantCulture)}.{Path.GetFileName(sortedFiles[i].FullName)}"
                        )
                    );
            }
        }
    }
}
