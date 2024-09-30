using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ProductPart
    {
        public TemplatePlate TemplatePlate { get; set; } = new TemplatePlate();
        public List<PdfFile> PdfFiles { get; set; } = new List<PdfFile>();
        public ImposRunList RunList { get; set; } = new ImposRunList();

        [Obsolete]
        public TemplateSheet Sheet { get; set; } = new TemplateSheet();

        public List<TemplateSheet> TemplateSheets { get; set; } = new List<TemplateSheet>();
        public List<PrintSheet> PrintSheets { get; set; } = new List<PrintSheet>();

        public ProofParameters Proof {  get; set; } = new ProofParameters();

        public PdfFile AddPdfFile(string filePath)
        {
            var file = new PdfFile(filePath);
            file.Id = PdfFiles.Count;
            PdfFiles.Add(file);
            return file;
        }

        public TemplateSheet CreateTemplateSheet()
        {
            Sheet = new TemplateSheet();

            return Sheet;
        }

        public TemplateSheet CreateTemplateSheet(double w, double h)
        {
            Sheet = new TemplateSheet(w, h);
            return Sheet;
        }

        public void Save(string filePath)
        {
            Save(this,filePath);
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

        public static ProductPart Copy (ProductPart part)
        {
            var imposStr = JsonSerializer.Serialize(part);
            return JsonSerializer.Deserialize<ProductPart>(imposStr);
        }

        public PdfFilePage GetPdfPage(ImposRunPage runPage)
        {
            var file = GetPdfFile(runPage);
            if (runPage.PageIdx > file.Pages.Length) throw new Exception($"No page with index {runPage.PageIdx}");
            return file.Pages[runPage.PageIdx-1];

        }

        public PdfFile GetPdfFile(ImposRunPage runPage)
        {
            PdfFile file = PdfFiles.FirstOrDefault(x => x.Id == runPage.FileId);
            if (file == null) throw new Exception($"No pdf file with Id: {runPage.FileId}");
            return file;
        }
    }
}
