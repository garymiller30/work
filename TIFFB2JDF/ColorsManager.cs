using System.Collections.Generic;
using System.Linq;

namespace TIFFB2JDF
{
    public static class ColorsManager
    {
        private static readonly List<Color> _colors = new List<Color>();

        public static readonly Dictionary<string, string> ColorsCmyk = new Dictionary<string, string>
        {
            {"CYAN","1 0 0 0" },
            {"MAGENTA","0 1 0 0" },
            {"YELLOW", "0 0 1 0" },
            {"BLACK","0 0 0 1" }
        };

        public static void AddColor(string name)
        {
            var sel = _colors.FirstOrDefault(x => x.Name.Equals(name));
            if (sel == null)
            {
                _colors.Add(new Color(name));
            }

        }


        public static string GetXml()
        {
            return _colors.Aggregate(string.Empty, (current, color) => current + color.GetXml());
        }
    }
}
