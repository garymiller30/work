namespace TIFFB2JDF
{
    
    public class Separation
    {
        public string SeparationName { get; set; }

        public string Status { get; set; } = "Available";

        public LayoutElement Layout { get;set; } = new LayoutElement();

        public Separation(string separationName)
        {
            SeparationName = separationName;
            Layout.Separation.Name = separationName;
        }

        public string GetXml()
        {
            var str = $"\t\t\t\t\t<RunList Separation=\"{SeparationName}\" Status=\"{Status}\">\n";

            str += Layout.GetXml();

            str += "\t\t\t\t\t</RunList>\n";

            return str;
        }
    }
}
