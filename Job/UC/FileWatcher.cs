// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System.IO;

namespace Job.UC
{
    public class FileWatcher : IWatcher
    {
        private readonly FileSystemWatcher _watcher;


        public event FileSystemEventHandler OnChanged = delegate{};
        public event FileSystemEventHandler OnDeleted = delegate{};
        public event FileSystemEventHandler OnCreated = delegate{};
        public event RenamedEventHandler OnRenamed = delegate{};
        public event ErrorEventHandler OnError = delegate{};


        public FileWatcher()
        {
            _watcher = new FileSystemWatcher
            {
                EnableRaisingEvents = false,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                               | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*"
            };


            _watcher.Renamed += _watcher_Renamed;
            _watcher.Created += WatcherOnCreated;
            _watcher.Error += WatcherOnError;
            _watcher.Deleted += WatcherOnDeleted;
            _watcher.Changed += WatcherOnChanged;
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            OnChanged(this, e);
        }

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            OnDeleted(this, e);
        }

        private void WatcherOnError(object sender, ErrorEventArgs e)
        {
            OnError(this, e);
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            OnCreated(this, e);
        }

        private void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            OnRenamed(this, e);
        }

        public void SetWatchFolder(string folder)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Path = folder;
            _watcher.EnableRaisingEvents = true;
            
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

        ~FileWatcher()
        {
            _watcher?.Dispose();
        }
    }
}
