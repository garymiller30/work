using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("","Додати формат до імені файлу",Icon = "add_page_size")]
    public class PdfAddFormatToFileName : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                var info = PdfHelper.GetPageInfo(file.FileInfo.FullName);
                string newFileName = $"{Path.GetFileNameWithoutExtension(file.FileInfo.FullName)}_{info.Trimbox.wMM():N0}x{info.Trimbox.hMM():N0}{Path.GetExtension(file.FileInfo.FullName)}";
                File.Move(file.FileInfo.FullName, Path.Combine(Path.GetDirectoryName(file.FileInfo.FullName), newFileName));
            }
        }
    }
}
