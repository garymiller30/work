using JobSpace.Static.Pdf.Common;

namespace JobSpace.Static.Pdf.Scale
{
    public class PdfScaleParams
    {
        public ScaleByEnum SourceScaleFrom { get;set;} = ScaleByEnum.Mediabox;
        public ScaleByEnum ScaleBy { get; set; } = ScaleByEnum.Mediabox;
        public ScaleVariantEnum ScaleVariant { get; set; } = ScaleVariantEnum.Proportial;
        public TargetSize TargetSize { get; set; } = new TargetSize();
    }
}
