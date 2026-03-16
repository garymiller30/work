using Interfaces.FileBrowser;
using Interfaces.Plugins;
using System.Globalization;
using System.IO;
using System.Linq;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити","нумеровані файли",Order =10,Icon ="numeric_files")]
    public class PdfNumericFiles : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            var arr = context.InputFiles.Select(x => x.FullName).ToArray();
            int count = context.InputFiles.Count;
            int numCnt = $"{count}".Length;


            for (int i = 1; i <= count; i++)
            {

                File.Move(arr[i - 1],
                    Path.Combine(
                        Path.GetDirectoryName(arr[i - 1]), $"{i.ToString($"D0{numCnt}", CultureInfo.InvariantCulture)}.{Path.GetFileName(arr[i - 1])}"
                        )
                    );
            }

        }
    }
}
