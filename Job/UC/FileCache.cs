using System;
using System.Collections.Generic;
using System.IO;
using Interfaces;

namespace Job.UC
{
    public sealed class FileCache : ICache<IFileSystemInfoExt>
    {
#pragma warning disable CS0067 // The event 'FileCache.OnChanged' is never used
        public event EventHandler<IFileSystemInfoExt> OnChanged;
#pragma warning restore CS0067 // The event 'FileCache.OnChanged' is never used
#pragma warning disable CS0067 // The event 'FileCache.OnDeleted' is never used
        public event EventHandler<IFileSystemInfoExt> OnDeleted;
#pragma warning restore CS0067 // The event 'FileCache.OnDeleted' is never used
#pragma warning disable CS0067 // The event 'FileCache.OnCreated' is never used
        public event EventHandler<IFileSystemInfoExt> OnCreated;
#pragma warning restore CS0067 // The event 'FileCache.OnCreated' is never used
#pragma warning disable CS0067 // The event 'FileCache.OnRenamed' is never used
        public event EventHandler<IFileSystemInfoExt> OnRenamed;
#pragma warning restore CS0067 // The event 'FileCache.OnRenamed' is never used
#pragma warning disable CS0067 // The event 'FileCache.OnError' is never used
        public event ErrorEventHandler OnError;
#pragma warning restore CS0067 // The event 'FileCache.OnError' is never used

        public List<IFileSystemInfoExt> GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        public int GetCountFiles()
        {
            throw new NotImplementedException();
        }

        public List<IFileSystemInfoExt> GetAllFiles(string path)
        {
            throw new NotImplementedException();
        }
    }
}
