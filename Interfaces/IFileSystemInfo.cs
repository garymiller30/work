using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IFileSystemInfo
    {
        string Name { get;set; }
        string FullName { get;set;}
        string Extension { get;set;}
        DateTime LastWriteTime { get;set;}
        FileAttributes Attributes { get;set;}
        long Length { get;set;}
        bool Exists { get;set;}

    }
}
