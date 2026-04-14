namespace Interfaces.PdfUtils
{
    public struct FileFormat
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Bleeds { get; set; }
        public int cntPages { get; set; }
    }
}
