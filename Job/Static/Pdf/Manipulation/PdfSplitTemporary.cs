using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System.IO;
using System.Text.Json;

namespace JobSpace.Static.Pdf.Manipulation
{
    [PdfTool("Маніпуляції з файлом", "• Розділити тимчасово зібраний файл",Icon = "split_temporary_file",Order =5)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public class PdfSplitTemporary : IPdfTool
    {
        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                SplitTemporary(file.FullName);
            }
        }

        public void SplitTemporary(string filePath)
        {
            string listFile = Path.ChangeExtension(filePath, ".json");

            if (!File.Exists(listFile)) return;

            string jsonStr = File.ReadAllText(listFile);

            PdfMergeFile[] list = JsonSerializer.Deserialize<PdfMergeFile[]>(jsonStr);

            using ( PDFlib p = new PDFlib())
            {
                try
                {
                    string rootDir = Path.GetDirectoryName(filePath);
                    int indoc = p.open_pdi_document(filePath, "");

                    foreach (PdfMergeFile item in list)
                    {
                        string itemFile = Path.Combine(rootDir, item.Name);

                        p.begin_document(itemFile, "optimize=true");

                        for (int i = item.From; i <= item.To; i++)
                        {
                            p.begin_page_ext(0, 0, "");
                            int pagehdl = p.open_pdi_page(indoc, i, "cloneboxes");
                            p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                            p.close_pdi_page(pagehdl);
                            p.end_page_ext("");
                        }
                        p.end_document("");
                    }

                    p.close_pdi_document(indoc);
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "PdfSplitTemporary");
                }
            }
        }
    }
}
