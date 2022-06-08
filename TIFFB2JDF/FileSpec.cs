namespace TIFFB2JDF
{
    public class FileSpec
    {
        public string MimeType { get; set; } = "application/tiff";

        public string Url { get; set; }

        public string GetXml()

        {

            var fix = Url.Replace("~", "%7E");

            return $"\t\t\t\t\t\t\t<FileSpec MimeType=\"{MimeType}\" URL=\"file:{fix}\"/>\n";
        }
    }
}
