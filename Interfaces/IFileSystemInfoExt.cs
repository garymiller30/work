using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Interfaces.PdfUtils;

namespace Interfaces
{
    public interface IFileSystemInfoExt
    {
        bool IsDir { get; }
        FileFormat Format { get; set; }
        IFileSystemInfo FileInfo { get; set; }
        HashSet<string> UsedColors { get; set; }
        string CreatorApp { get; set; }
        string FullName { get; }
        string Name { get; }
        void RefreshParam(string filePath);
    }
}
