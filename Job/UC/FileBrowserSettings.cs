using Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Job.UC
{
    [Serializable]
    public sealed class FileBrowserSettings : IFileBrowserControlSettings, INotifyPropertyChanged
    {
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

        [Obsolete]
        public static bool UseViewer { get; set; }
        [Obsolete]
        public static string Viewer { get; set; }
        [Obsolete]
        public static string ViewerCommandLine { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
