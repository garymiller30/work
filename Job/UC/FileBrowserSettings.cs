using Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JobSpace.UC
{
    [Serializable]
    public sealed class FileBrowserSettings : IFileBrowserControlSettings, INotifyPropertyChanged
    {
        [NonSerialized]
        private Font _font;
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
        public string RootFolder { get; set; }
        public string CurFolder { get; set; }
        public bool ScanFiles { get; set; }
        public bool ShowGroups { get; set; }
        /// <summary>
        /// показати всі файли у вкладених папках
        /// </summary>
        public bool ShowAllFilesWithoutDir { get; set; }

        public string[] IgnoreFolders { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsRoot => RootFolder.Equals(CurFolder);
        [Browsable(false)]
        public float FontSize { get; set; } = 8.25f;
        [Browsable(false)]
        public string FontName { get; set; }
        [Browsable(false)]
        public FontStyle FontStyle { get; set; }

        public Font UserFont
        {
            get => GetOLVFont();
            set
            {
                _font = value;
                FontName = _font.Name;
                FontSize = _font.Size;
                FontStyle = _font.Style;
            }
        }

        public int RowHeight { get ; set; }

        Font GetOLVFont()
        {
            float fontSize = 8.25f;
            string fontName = "Microsoft Sans Serif";


            if (FontSize == 0)
            {
                FontSize = fontSize;
            }
            if (string.IsNullOrEmpty(FontName))
            {
                FontName = fontName;
            }

            if (_font == null || (_font.Name != FontName || _font.Size != FontSize))
            {
                _font = new Font(FontName, FontSize, FontStyle);
            }
            return _font;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
