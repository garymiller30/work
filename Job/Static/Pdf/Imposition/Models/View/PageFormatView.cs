using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.View
{
    public class PageFormatView
    {
        private PdfFile _file;
        

        public PageFormatView(PdfFile pdfFile)
        {
            this._file = pdfFile;
            Width = (decimal)_file.Pages[0].Trim.W;
            Height = (decimal)_file.Pages[0].Trim.H;
            Bleed = (decimal)_file.Pages[0].Bleed;
            Margins.Set(Bleed);
        }

        public int FileId { get => _file.Id; }
        public decimal Width { get;set;}
        public decimal Height { get;set;}
        public decimal Bleed { get;set;}
        public ClipBox Margins { get; set; } = new ClipBox();
        public override string ToString()
        {
            return $"{FileId}";
        }

        public TemplatePage ToTemplatePage()
        {
            var tp = new TemplatePage()
            {
                W = (double)Width,
                H = (double)Height,
            };
            tp.Bleeds.Set(Bleed);
            tp.Margins.Set(Margins);
            return tp;
        }
    }
}
