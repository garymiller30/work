using Interfaces.Plugins;
using Job.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginColorJobListRow
{
    public sealed class ColorJobListRowSettings
    {
        public Dictionary<int, RowColor> ColorDictionary { get; set; } = new Dictionary<int, RowColor>();


        public IRowColor GetColor(int status)
        {

            var color = new RowColor();

            if (ColorDictionary.ContainsKey(status))
            {
                if (ColorDictionary[status].Fore != Color.Transparent) color.Fore = ColorDictionary[status].Fore;
                if (ColorDictionary[status].Back != Color.Transparent) color.Back = ColorDictionary[status].Back;
            }
            return color;
        }

        public void SetColor(int status, RowColor color)
        {
            if (ColorDictionary.ContainsKey(status))
            {
                ColorDictionary[status] = color;
            }
            else
            {
                ColorDictionary.Add(status, color);
            }
        }

        public void SetFore(int status, Color fore)
        {
            if (ColorDictionary.ContainsKey(status))
            {
                ColorDictionary[status].Fore = fore;
            }
            else
            {
                ColorDictionary.Add(status, new RowColor { Fore = fore});
            }
        }

        public void SetBack(int status, Color back)
        {
            if (ColorDictionary.ContainsKey(status))
            {
                ColorDictionary[status].Back = back;
            }
            else
            {
                ColorDictionary.Add(status, new RowColor { Back = back });
            }
        }
    }
}
