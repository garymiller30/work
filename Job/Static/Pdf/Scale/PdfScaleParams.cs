﻿using Job.Static.Pdf.Common;

namespace Job.Static.Pdf.Scale
{
    public class PdfScaleParams
    {
        public ScaleByEnum ScaleBy { get; set; } = ScaleByEnum.Mediabox;
        public ScaleVariantEnum ScaleVariant { get; set; } = ScaleVariantEnum.Proportial;
        public TargetSize TargetSize { get; set; } = new TargetSize();
    }
}