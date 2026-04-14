namespace TIFFB2JDF
{

    

    public class Color
    {
 

        public string Name { get; set; }
        public string CMYK { get; set; } = "0 1 1 0";

        public Color(string name)
        {
            Name = name;
            if (ColorsManager.ColorsCmyk.ContainsKey(Name))
            {
                CMYK = ColorsManager.ColorsCmyk[Name];
            }

        }

        public string GetXml()
        {
            return $"\t\t\t<Color CMYK=\"{CMYK}\" Name=\"{Name}\"/>\n";
        }

    }
}
