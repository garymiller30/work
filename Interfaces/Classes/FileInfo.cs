using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Classes
{
    public class FileInfo : IFileSystemInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTime LastWriteTime { get; set; }
        public FileAttributes Attributes { get; set; }
        public long Length { get; set; }
        public string Extension { get; set; }
        public bool Exists { get; set; }
    }
}
