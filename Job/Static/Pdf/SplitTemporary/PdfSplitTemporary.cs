using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFlib_dotnet;
using JobSpace.Static.Pdf.Common;

namespace JobSpace.Static.Pdf.SplitTemporary
{
    public class PdfSplitTemporary
    {
        public void Run(string filePath)
        {
            string listFile = Path.ChangeExtension(filePath, ".json");

            if (!File.Exists(listFile)) return;

            string jsonStr = File.ReadAllText(listFile);

            PdfMergeFile[] list = JsonSerializer.Deserialize<PdfMergeFile[]>(jsonStr);

            PDFlib p = null;

            try
            {
                string rootDir = Path.GetDirectoryName(filePath);

                p = new PDFlib();

                int indoc = p.open_pdi_document(filePath, "");

                foreach (PdfMergeFile item in list)
                {
                    string itemFile = Path.Combine(rootDir, item.Name);

                    p.begin_document(itemFile, "");

                    for (int i = item.From; i <= item.To; i++)
                    {
                        p.begin_page_ext(0,0,"");
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
            finally
            {
                p?.Dispose();
            }

        }
    }
}
