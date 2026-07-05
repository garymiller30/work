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
    [PdfTool("Додати", "Додати тираж до імені файлу", Order = 2, Icon = "add_tirag", Description = "Додати тираж до імені файлу", IsBackgroundTask = true)]
    public class PdfAddTirag : IPdfTool, IPdfToolAsync
    {
        List<FileTirag>? fileTirags;
        private static readonly Regex TiragRegex = new Regex(@"#(\d+)\.", RegexOptions.Compiled);
        public bool Configure(PdfJobContext context)
        {
            return ConfigureTool(context);
        }

        private bool ConfigureTool(PdfJobContext context)
        {
            if (context.InputFiles.Count > 1)
            {
                using var form = new FormEnterTirag(context.InputFiles);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    fileTirags = form.fileTirags;
                    return true;
                }
            }
            else
            {
                using var form = new FormTirag();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    fileTirags = new List<FileTirag>();
                    fileTirags.Add(new FileTirag(context.InputFiles[0], form.Tirag));
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ConfigureAsync(PdfJobContext context)
        {
            return ConfigureTool(context);
        }

        public void Execute(PdfJobContext context)
        {
            if (fileTirags == null || !fileTirags.Any()) return;

            foreach (var file in fileTirags)
            {
                //var reg = new Regex(@"#(\d+)\.");
                var match = TiragRegex.Match(file.FileInfo.Name);

                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileInfo.Name);
                string extension = file.FileInfo.FileInfo.Extension;
                string baseName = match.Success
                    ? fileNameWithoutExt.Substring(0, match.Index)
                    : fileNameWithoutExt;
                var targetFile = $"{baseName}#{file.Tirag}{extension}";

                context.FileManager.MoveFileOrDirectoryToCurrentFolder(file.FileInfo, targetFile);
            }
        }
    }
}
