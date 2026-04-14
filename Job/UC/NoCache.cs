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
        private List<string> _ignoreFolders = new List<string>(){"temp",".signa" };

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
        }

        private void WatcherOnError(object sender, ErrorEventArgs e)
        {
            OnError(this, e);
        }

        private void WatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Debug.WriteLine($"- OnRenamed: from {e.OldName} to {e.Name} e.FullPath: {e.FullPath}");

                var newItem = _files.FirstOrDefault(x =>
                    x.FileInfo.FullName.Equals(e.FullPath, StringComparison.CurrentCultureIgnoreCase));

                if (newItem == null) // такого нема
                {
                    var oldItem = _files.FirstOrDefault(x =>
                        x.FileInfo.FullName.Equals(e.OldFullPath, StringComparison.CurrentCultureIgnoreCase));

                    if (oldItem != null)
                    {
                        _files.Remove(oldItem);
                        OnDeleted(this, oldItem);
                    }

                    newItem = new FileSystemInfoExt(e.FullPath);
                    _files.Add(newItem);
                    OnCreated(this, newItem);
                }
                else
                {
                    OnChanged(this, newItem);
                }
            }

        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {

                // temp пропускаємо
                if (!_ignoreFolders.Contains(e.Name.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase)) 
                {
                    Debug.WriteLine($"- OnCreated: e.FullPath: {e.FullPath}");
                    try
                    {
                        var fsie = new FileSystemInfoExt(e.FullPath);
                        _files.Add(fsie);
                        OnCreated(this, fsie);

                    }
                    catch (Exception exception)
                    {
                        Log.Error(this, $"WatcherOnCreated : {e.FullPath}", exception.Message);
                    }
                }

            }
        }

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                //Debug.WriteLine($"- OnDeleted: e.FullPath: {e.FullPath}");
                var oldItem = _files.FirstOrDefault(x => x.FileInfo.FullName.Equals(e.FullPath, StringComparison.InvariantCultureIgnoreCase));
                if (oldItem != null)
                {
                    OnDeleted(this, oldItem);
                    _files.Remove(oldItem);
                }
            }

        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                Debug.WriteLine($"- OnChanged: e.FullPath: {e.FullPath}");

                var oldItem = _files.FirstOrDefault(x =>
                    x.FileInfo.FullName.Equals(e.FullPath, StringComparison.InvariantCultureIgnoreCase));
                if (oldItem != null)
                {
                    oldItem.RefreshParam(e.FullPath);
                    OnChanged(this, oldItem);
                }

            }

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
