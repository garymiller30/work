using System;
using System.Drawing;

namespace Job.UserForms
{
    public class TrimBoxResult
    {
        public TrimBoxResultEnum ResultType { get; set; } = TrimBoxResultEnum.byTrimbox;
        public RectangleF TrimBox { get; set; }
        public double Bleed { get; set; }
        public SpreadBox Spread { get; set; }
    }
}
