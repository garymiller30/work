using System.Xml.Serialization;

namespace TIFFB2JDF
{
    public class SeparationSpec
    {
        [XmlAttribute("SheetName")]
        public string Name { get; set; } = "CYAN";

        public string GetXml()
        {
            return $"\t\t\t\t\t\t\t<SeparationSpec Name =\"{Name}\"/>\n";
        }
    }
}
