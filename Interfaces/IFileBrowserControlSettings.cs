using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IFileBrowserControlSettings
    {
        string Title { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        //StringCollection IgnoreFolders { get; set; }
        string RootFolder { get; set; }
        string CurFolder { get; set; }
        bool ScanFiles { get; set; }
        float FontSize { get; set; }
        string FontName { get; set; }
        FontStyle FontStyle { get; set; }
       int RowHeight { get; set; }
        bool IsRoot { get; }
    }
}
