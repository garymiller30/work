using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.PdfScale
{
    internal static class ExtendTargetSizeHelpers
    {
        public static double WidthInch(this TargetSize targetSize) => targetSize.Width * PdfScaler.mn;
        public static double HeigthInch(this TargetSize targetSize) => targetSize.Height * PdfScaler.mn;
        public static double BleedInch(this TargetSize targetSize) => targetSize.Bleed * PdfScaler.mn;

        public static double WidthWithBleedInch(this TargetSize targetSize) => (targetSize.Width + targetSize.Bleed * 2) * PdfScaler.mn;
        public static double HeightWithBleedInch(this TargetSize targetSize) => (targetSize.Height + targetSize.Bleed * 2) * PdfScaler.mn;
    }

}
