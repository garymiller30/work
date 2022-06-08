using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IFileBrowserControlSettings
    {
        string Title { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        string RootFolder { get; set; }
        string CurFolder { get; set; }
        bool ScanFiles { get; set; }
    }
}
