using JobSpace.Static.Pdf.Common;

namespace JobSpace.Static.Pdf.Manipulation.Scale
{
    public class PdfScaleParams
    {
        public ScaleByEnum SourceScaleFrom { get; set; } = ScaleByEnum.Mediabox;
        public ScaleByEnum ScaleBy { get; set; } = ScaleByEnum.Mediabox;
        public ScaleVariantEnum ScaleVariant { get; set; } = ScaleVariantEnum.Proportial;
        public ScaleModeEnum ScaleMode { get; set; } = ScaleModeEnum.FitToTargetSize;
        public bool AutoMatchOrientation { get; set; } = true;
        public double ScalePercent { get; set; } = 100.0;
        public TargetSize TargetSize { get; set; } = new TargetSize();
    }
}
