namespace TIFFB2JDF
{
    public class LayoutElement
    {
        public SeparationSpec Separation { get; set; } = new SeparationSpec();

        public FileSpec File { get; set; } = new FileSpec();

        public string GetXml()
        {
            var str = "\t\t\t\t\t\t<LayoutElement>\n";

            str += Separation.GetXml();
            str += File.GetXml();


            str += "\t\t\t\t\t\t</LayoutElement>\n";
            return str;
        }
    }
}
