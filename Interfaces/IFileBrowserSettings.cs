using System.Collections.Generic;

namespace Interfaces
{
    public interface IFileBrowserSettings
    {
        bool UseViewer { get; set; }
        string Viewer { get; set; }
        string ViewerCommandLine { get; set; }
        
        List<string> CustomButtonPath { get; set; }

        List<string> FolderNamesForCreate { get; set; }
    }
}