namespace Job.Static.PdfScale
{
    public class TargetSize
    {
        public double Width { get; set; } = 210;
        public double Height { get; set; } = 297;
        public double Bleed { get; set; } = 3;

        public double WidthWithBleed => Width + Bleed * 2;
        public double HeightWithBleed => Height + Bleed * 2;
    }
}
