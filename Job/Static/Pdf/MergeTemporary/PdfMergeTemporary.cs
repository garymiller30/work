using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JobSpace.Static.Pdf.MergeTemporary
{
    [PdfTool("","• З'єднати файли в один (тимчасово)",Icon = "merge_in_temporary_file")]
    public sealed class PdfMergeTemporary : IPdfTool
    {

        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            bool success = false;

            var files = context.InputFiles;

            string fileName = Path.Combine(Path.GetDirectoryName(files[0].FullName), $"{Path.GetFileNameWithoutExtension(files[0].FullName)}_merged.pdf");
            PDFlib p = new PDFlib();

            List<PdfMergeFile> mergedList = new List<PdfMergeFile>();

            int idxPage = 1;

            try
            {
                p.begin_document(fileName, "optimize=true");

                foreach (var file in files)
                {
                    var indoc = p.open_pdi_document(file.FullName, "");
                    var pagecount = p.pcos_get_number(indoc, "length:pages");

                    PdfMergeFile mergeFile = new PdfMergeFile();
                    mergeFile.From = idxPage;

                    idxPage = idxPage + (int)pagecount - 1;
                    mergeFile.To = idxPage;
                    idxPage++;
                    mergeFile.Name = Path.GetFileName(file.FullName);
                    mergedList.Add(mergeFile);

                    for (int i = 1; i <= pagecount; i++)
                    {
                        var page = p.open_pdi_page(indoc, i, "cloneboxes");
                        p.begin_page_ext(0, 0, "");
                        p.fit_pdi_page(page, 0, 0, "cloneboxes");
                        p.close_pdi_page(page);
                        p.end_page_ext("");
                    }
                    p.close_pdi_document(indoc);
                }
                p.end_document("");

                var str = JsonSerializer.Serialize(mergedList);
                string mergeFilePath = Path.ChangeExtension(fileName, ".json");

                File.WriteAllText(mergeFilePath, str);

                success = true;
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "PdfMerger");
            }
            finally { p?.Dispose(); }

           
        }

    }
}

