using BrightIdeasSoftware;
using ExtensionMethods;
using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Dlg;
using JobSpace.UserForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static JobSpace.UserForms.FormEnterTirag;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("", "Додати тираж", Order = 2, Icon = "add_tirag", Description = "Додати тираж до імені файлу",IsBackgroundTask =true)]
    public class PdfAddTirag : IPdfTool,IPdfToolAsync
    {
        List<FileTirag> fileTirags;

        public bool Configure(PdfJobContext context)
        {
            if (context.InputFiles.Count > 1)
            {
 
                using (var form = new FormEnterTirag(context.InputFiles))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        fileTirags = form.fileTirags;
                        return true;
                    }
                }
            }
            else
            {
                using (var form = new FormTirag())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        fileTirags = new List<FileTirag>();
                        fileTirags.Add(new FileTirag { FileInfo = context.InputFiles[0], Tirag = form.Tirag });
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> ConfigureAsync(PdfJobContext context)
        {
            if (context.InputFiles.Count > 1)
            {
                var form = new FormEnterTirag(context.InputFiles);
                if (await form.ShowDialogAsync() == DialogResult.OK)
                {
                    fileTirags = form.fileTirags;
                    return true;
                }
            }
            else
            {
                using (var form = new FormTirag())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        fileTirags = new List<FileTirag>();
                        fileTirags.Add(new FileTirag { FileInfo = context.InputFiles[0], Tirag = form.Tirag });
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in fileTirags)
            {
                var reg = new Regex(@"#(\d+)\.");
                var match = reg.Match(file.FileInfo.Name);
                string targetFile;
                if (match.Success)
                {
                    targetFile =
                        $"{Path.GetFileNameWithoutExtension(file.FileInfo.Name).Substring(0, match.Index)}#{file.Tirag}{file.FileInfo.FileInfo.Extension}";
                }
                else
                {
                    targetFile = $"{Path.GetFileNameWithoutExtension(file.FileInfo.Name)}#{file.Tirag}{file.FileInfo.FileInfo.Extension}";
                }

                context.FileManager.MoveFileOrDirectoryToCurrentFolder(file.FileInfo, targetFile);
            }
        }
    }
}
