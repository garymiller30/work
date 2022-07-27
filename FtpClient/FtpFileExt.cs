// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using FluentFTP;
using System;
using Interfaces;
using Interfaces.Ftp;

namespace FtpClient
{
    [Serializable]
    public class FtpFileExt : IFtpFileExt
    {
        public FtpFileExtStatus Status { get; set; } = FtpFileExtStatus.New;

        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDir { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }

        public FtpFileExt(FtpListItem file)
        {
            Name = file.Name;
            FullPath = file.FullName;
            IsDir = file.Type == FtpObjectType.Directory;
            Size = file.Size;
            LastModified = file.Modified;
        }

        public void Refresh(IFtpFileExt fileNew)
        {
            Name = fileNew.Name;
            FullPath = fileNew.FullPath;
            IsDir = fileNew.IsDir;
            Size = fileNew.Size;
            LastModified = fileNew.LastModified;
        }
    }
}