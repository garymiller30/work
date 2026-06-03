// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Interfaces;
using JobSpace.Models;
using JobSpace.Static;
using Logger;

namespace JobSpace.UC
{
    public sealed class NoCache : ICache<IFileSystemInfoExt>
    {
        private readonly IWatcher _watcher;
        private List<string> _ignoreFolders = new List<string>() { "temp", ".signa", ".preview", ".impos" };
        private readonly System.Timers.Timer _debounceTimer;

        readonly List<IFileSystemInfoExt> _files = new List<IFileSystemInfoExt>();

        public event EventHandler<IFileSystemInfoExt> OnChanged = delegate { };
        public event EventHandler<IFileSystemInfoExt> OnDeleted = delegate { };
        public event EventHandler<IFileSystemInfoExt> OnCreated = delegate { };
        public event EventHandler<IFileSystemInfoExt> OnRenamed = delegate { };
        public event ErrorEventHandler OnError = delegate { };

        public NoCache(IWatcher watcher)
        {
            _watcher = watcher;

            _watcher.OnChanged += WatcherOnChanged;
            _watcher.OnDeleted += WatcherOnDeleted;
            _watcher.OnCreated += WatcherOnCreated;
            _watcher.OnRenamed += WatcherOnRenamed;
            _watcher.OnError += WatcherOnError;

            _debounceTimer = new System.Timers.Timer(300);
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += DebounceTimer_Elapsed;
        }

        private void DebounceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            OnChanged(this, null);
        }

        private void TriggerDebounce()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void WatcherOnError(object sender, ErrorEventArgs e)
        {
            OnError(this, e);
        }

        private void WatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                TriggerDebounce();
            }
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                if (!_ignoreFolders.Contains(e.Name.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                {
                    TriggerDebounce();
                }
            }
        }

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                TriggerDebounce();
            }
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                TriggerDebounce();
            }
        }

        ~NoCache()
        {
            _debounceTimer?.Dispose();
        }


        NaturalSorting.NaturalFileInfoNameComparer _naturalCompaper = new NaturalSorting.NaturalFileInfoNameComparer();


        public List<IFileSystemInfoExt> GetFiles(string path)
        {
            DisableWatcher();

            _files.Clear();

            if (!Directory.Exists(path)) return _files;

            var dirs = Directory.GetDirectories(path)
                .Where(y => !_ignoreFolders.Contains(Path.GetFileName(y).ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                .Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            dirs.Sort(_naturalCompaper);


            _files.AddRange(dirs);
            var f = Directory.GetFiles(path).Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            f.Sort(_naturalCompaper);

            _files.AddRange(f);


            SetWatcher(path);

            return _files;
        }

        public List<IFileSystemInfoExt> GetDirs(string path)
        {

            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();

            var dirs = Directory.GetDirectories(path)
                 .Where(y => !_ignoreFolders.Contains(Path.GetFileName(y).ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                 .Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            dirs.Sort(_naturalCompaper);

            return dirs.Cast<IFileSystemInfoExt>().ToList();
        }

        public List<IFileSystemInfoExt> GetAllFiles(string path)
        {
            DisableWatcher();

            _files.Clear();

            if (!Directory.Exists(path)) return _files;

            var f = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            f.Sort(_naturalCompaper);

            _files.AddRange(f);

            SetWatcher(path);

            return _files;
        }


        private void DisableWatcher()
        {
            _watcher.Stop();
        }

        private void SetWatcher(string path)
        {
            _watcher?.SetWatchFolder(path);
        }

        public int GetCountFiles()
        {
            return _files.Count(x => !x.IsDir);
        }
    }
}
