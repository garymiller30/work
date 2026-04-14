using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobSpace.Profiles
{
    [Serializable]
    public class JobListSettings : IJobListSettings
    {
        [NonSerialized]
        Font _font;

        [XmlIgnore]
        
        public Font UserFont
        {
            get => GetFont();
            set
            {
                _font = value;
                FontName = _font.Name;
                FontSize = _font.Size;
                FontStyle = _font.Style;
            }
        }

        public Font GetFont()
        {
            string fontName = string.IsNullOrEmpty(FontName) ? "Microsoft Sans Serif" : FontName;
            float fontSize = FontSize == 0 ? 8.25f : FontSize;


            if (_font == null)
            {
                _font = new Font(fontName, fontSize, FontStyle);
            }
            return _font;
        }

        [Browsable(false)]
        public string FontName { get; set; }
        [Browsable(false)]
        public float FontSize { get; set; }
        [Browsable(false)]
        public FontStyle FontStyle { get; set; }
    }
}
