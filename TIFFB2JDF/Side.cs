using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TIFFB2JDF
{
    [XmlRoot("RunList")]
    public class Side
    {
        public enum Sides
        {
            Front,
            Back
        }

        readonly List<Color> _colors = new List<Color>();

        public Sides SideName { get; set; } = Sides.Front;

        public List<Separation> Separations { get; set; } = new List<Separation>();


        public Separation AddSeparation(string name,string fileName)
        {

            var fixname = name.ToUpper();

            var sep = new Separation(fixname);

            ColorsManager.AddColor(fixname);

            _colors.Add(new Color(fixname));
            sep.Layout.File.Url = fileName;
            Separations.Add(sep);
            return sep;
        }


        public Side(Sides sideName)
        {
            SideName = sideName;
        }


        public string GetXml()
        {
            var str = $"\t\t\t\t<RunList Side=\"{Enum.GetName(typeof(Sides),SideName)}\">\n";

            foreach (var separation in Separations)
            {
                str += separation.GetXml();
            }


            str += "\t\t\t\t</RunList>\n";

            return str;
        }


        
    }
}
