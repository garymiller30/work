using System;
using Interfaces.Ftp;

namespace Interfaces
{
    public interface IFtpFileExt
    {
        FtpFileExtStatus Status { get; set; }

        string Name { get; set; }
        string FullPath { get; set; }
        bool IsDir { get; set; }
        long Size { get; set; }
        DateTime LastModified { get; set; }

        void Refresh(IFtpFileExt fileNew);
    }
}
