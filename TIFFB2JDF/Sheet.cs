using System.Collections.Generic;
using System.Xml.Serialization;

namespace TIFFB2JDF
{
    [XmlRoot("RunList")]
    public class Sheet
    {
        public string SheetName { get; set; } = "1";

        public List<Side> Sides { get; set; } = new List<Side>();

        public Side AddSide(Side.Sides nameSide)
        {
            var s = new Side(nameSide);
            Sides.Add(s);
            return s;
        }


        public string GetXml()
        {
            var str = $"\t\t\t<RunList SheetName=\"{SheetName}\">\n";

            foreach (var side in Sides)
            {
                str += side.GetXml();
            }

            str += "\t\t\t</RunList>\n";

            return str;
        }
    }
}
