using System;
using System.Collections.Generic;
using Interfaces;

namespace JobSpace.Profiles
{
    [Serializable]
    //[XmlRoot("FileBrowserSettings")]
    public sealed class FileBrowserSettings : IFileBrowserSettings
    {
        public bool UseViewer { get; set; }
        public string Viewer { get; set; }
        public string ViewerCommandLine { get; set; }

        public List<string> CustomButtonPath { get; set; } = new List<string>();
        public List<string> FolderNamesForCreate { get;set; } = new List<string>();
    }
}
