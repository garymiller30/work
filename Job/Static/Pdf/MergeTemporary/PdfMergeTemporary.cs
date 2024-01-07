using Job.Static.Pdf.Common;
using Newtonsoft.Json;
using PDFlib_dotnet;
using System.Collections.Generic;
using System.IO;

namespace Job.Static.Pdf.MergeTemporary
{
    public sealed class PdfMergeTemporary
    {
        PdfMergeTemporaryParams _params;

        public PdfMergeTemporary(PdfMergeTemporaryParams param)
        {
            _params = param;
        }


        public void Run()
        {
            string fileName = Path.Combine(Path.GetDirectoryName(_params.Files[0]), $"{Path.GetFileNameWithoutExtension(_params.Files[0])}_merged.pdf");
            PDFlib p = new PDFlib();

            List<PdfMergeFile> mergedList = new List<PdfMergeFile>();

            int idxPage = 1;

            try
            {
                p.begin_document(fileName, "");

                foreach (string file in _params.Files)
                {
                    var indoc = p.open_pdi_document(file, "");
                    var pagecount = p.pcos_get_number(indoc, "length:pages");

                    PdfMergeFile mergeFile = new PdfMergeFile();
                    mergeFile.From = idxPage;

                    idxPage = idxPage + (int)pagecount - 1;
                    mergeFile.To = idxPage;
                    idxPage++;
                    mergeFile.Name = Path.GetFileName(file);
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

                using (var textWriter =  new StringWriter())
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(textWriter, mergedList);

                    string mergeFilePath = Path.ChangeExtension(fileName,".json");

                    File.WriteAllText(mergeFilePath, textWriter.ToString());

                }
                
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "PdfMerger");
            }
            finally { p?.Dispose(); }
        }
    }
}

