using Job.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginColorJobListRow
{
    public sealed class ColorJobListRowSettings
    {
        public Dictionary<int, Color> ColorDictionary { get; set; } = new Dictionary<int, Color>();

        public Color GetColor(int status)
        {
            if (ColorDictionary.ContainsKey(status))
                
                return ColorDictionary[status] == Color.Transparent ? ThemeController.Back : ColorDictionary[status];

            return ThemeController.Back; //default color
        }

        public void SetColor(int status, Color color)
        {
            if (ColorDictionary.ContainsKey(status))
            {
                ColorDictionary[status] = color;
            }
            else
            {
                ColorDictionary.Add(status,color);
            }
        }



    }
}
