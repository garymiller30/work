using System.Drawing;
using System.IO;
using Interfaces.PdfUtils;

namespace Interfaces
{
    public interface IFileSystemInfoExt
    {
        bool IsDir { get; set; }
        FileFormat Format { get; set; }
        IFileSystemInfo FileInfo { get; set; }
        ColorSpaces UsedColorSpace { get; set; }
        string CreatorApp { get; set; }

        void RefreshParam(string filePath);
    }
}
