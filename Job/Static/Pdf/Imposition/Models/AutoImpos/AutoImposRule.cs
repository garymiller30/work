using System;

namespace JobSpace.Static.Pdf.Imposition.Models.AutoImpos
{
    public class AutoImposRule
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Customer { get; set; }
        public string CategoryId { get; set; }
        public decimal? PageWidth { get; set; }
        public decimal? PageHeight { get; set; }
        public decimal PageFormatTolerance { get; set; } = 1;
        public int? ExactPageCount { get; set; }
        public int? PageCountMultipleOf { get; set; }
        public string PrintSheetTemplateFile { get; set; }
        public int Priority { get; set; }
        public bool OpenEditorBeforeExport { get; set; } = true;
    }
}
