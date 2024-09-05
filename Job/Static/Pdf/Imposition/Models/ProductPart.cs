using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models
{
    public class ProductPart
    {

        public List<PdfFile> PdfFiles { get; set; } = new List<PdfFile>();
        public ImposRunList RunList { get; set; } = new ImposRunList();

        public TemplateSheet Sheet { get; set; } = new TemplateSheet();

        public PdfFile AddPdfFile(string filePath)
        {
            var file = new PdfFile(filePath);
            file.Id = PdfFiles.Count;
            PdfFiles.Add(file);
            return file;
        }

        public TemplateSheet AddTemplateSheet()
        {
            Sheet = new TemplateSheet();

            return Sheet;
        }

        public TemplateSheet AddTemplateSheet(double w, double h)
        {
            Sheet = new TemplateSheet(w, h);
            return Sheet;
        }

        public static void Save(ProductPart impos, string fileName)
        {
            var imposStr = JsonSerializer.Serialize(impos);
            File.WriteAllText(fileName, imposStr);
        }

        public static ProductPart Load(string fileName)
        {
            var imposStr = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<ProductPart>(imposStr);
        }

        public PdfFilePage GetPdfPage(ImposRunPage runPage)
        {
            var file = GetPdfFile(runPage);
            if (runPage.PageIdx >= file.Pages.Length) throw new Exception($"No page with index {runPage.PageIdx}");
            return file.Pages[runPage.PageIdx];

        }

        public PdfFile GetPdfFile(ImposRunPage runPage)
        {
            PdfFile file = PdfFiles.FirstOrDefault(x => x.Id == runPage.FileId);
            if (file == null) throw new Exception($"No pdf file with Id: {runPage.FileId}");
            return file;
        }
    }
}
