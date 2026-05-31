using System;
using System.IO;
using Interfaces;

namespace PluginFileSearch
{
    internal sealed class FileSearchResult
    {
        public FileSearchResult(IFileSystemInfoExt file, string customerName)
        {
            File = file;
            CustomerName = customerName ?? string.Empty;
        }

        public IFileSystemInfoExt File { get; }
        public string CustomerName { get; }
        public string Name => File?.FileInfo?.Name ?? string.Empty;
        public string DirectoryName => Path.GetDirectoryName(FullName) ?? string.Empty;
        public string FullName => File?.FileInfo?.FullName ?? string.Empty;
        public string Extension => File?.FileInfo?.Extension ?? string.Empty;
        public long Size => File?.FileInfo?.Length ?? 0;
        public DateTime LastWriteTime => File?.FileInfo?.LastWriteTime ?? DateTime.MinValue;
        public decimal Width => File?.Format.Width ?? 0;
        public decimal Height => File?.Format.Height ?? 0;
        public int Pages => File?.Format.cntPages ?? 0;
    }
}
