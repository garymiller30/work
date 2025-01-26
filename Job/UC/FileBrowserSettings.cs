using Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JobSpace.UC
{
    [Serializable]
    public sealed class FileBrowserSettings : IFileBrowserControlSettings, INotifyPropertyChanged
    {
        private string _title;
        //private string[] _ignoreFolders;

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
        public bool ShowGroups { get;set; }
        /// <summary>
        /// показати всі файли у вкладених папках
        /// </summary>
        public bool ShowAllFilesWithoutDir { get;set; }

        public string[] IgnoreFolders { get;set;}

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsRoot => RootFolder.Equals(CurFolder);

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
