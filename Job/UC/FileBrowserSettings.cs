using Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Job.UC
{
    [Serializable]
    public sealed class FileBrowserSettings : IFileBrowserControlSettings, INotifyPropertyChanged
    {
        private string _title;
        private string[] _ignoreFolders;

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

        //[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public string[] IgnoreFolders { get;set;}
        [Obsolete]
        public static bool UseViewer { get; set; }
        [Obsolete]
        public static string Viewer { get; set; }
        [Obsolete]
        public static string ViewerCommandLine { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsRoot => RootFolder.Equals(CurFolder);

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
