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
        private readonly List<string> _ignoreFolders = new List<string> { "temp", ".signa", ".preview", ".impos" };

        // Кеш вмісту директорій: шлях директорії → список файлів/папок у ній.
        // Повернення до тієї самої директорії повертає ті самі об'єкти зі збереженими метаданими.
        private readonly Dictionary<string, List<IFileSystemInfoExt>> _dirContents =
            new Dictionary<string, List<IFileSystemInfoExt>>(StringComparer.OrdinalIgnoreCase);

        // Швидкий пошук об'єкта за повним шляхом для обробки подій watcher-а.
        private readonly Dictionary<string, IFileSystemInfoExt> _fileIndex =
            new Dictionary<string, IFileSystemInfoExt>(StringComparer.OrdinalIgnoreCase);

        // LRU-список директорій (не файлів).
        private readonly LinkedList<string> _recentDirs = new LinkedList<string>();
        private const int CacheCapacity = 10;

        readonly NaturalSorting.NaturalFileInfoNameComparer _naturalComparer = new NaturalSorting.NaturalFileInfoNameComparer();

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

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;

            Debug.WriteLine($"- OnChanged: {e.FullPath}");

            if (!_fileIndex.TryGetValue(e.FullPath, out var item)) return;

            item.RefreshParam(e.FullPath);
            OnChanged(this, item);
        }

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted) return;

            if (!_fileIndex.TryGetValue(e.FullPath, out var item)) return;

            _fileIndex.Remove(e.FullPath);

            var dirPath = Path.GetDirectoryName(e.FullPath);
            if (dirPath != null && _dirContents.TryGetValue(dirPath, out var list))
                list.Remove(item);

            if (item.IsDir)
            {
                EvictDirectory(e.FullPath);
                var prefix = e.FullPath + Path.DirectorySeparatorChar;
                var keysToRemove = _dirContents.Keys
                    .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (var key in keysToRemove)
                {
                    EvictDirectory(key);
                }
            }

            OnDeleted(this, item);
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Created) return;

            var name = Path.GetFileName(e.FullPath);
            if (_ignoreFolders.Contains(name?.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                return;

            Debug.WriteLine($"- OnCreated: {e.FullPath}");

            try
            {
                var item = new FileSystemInfoExt(e.FullPath);
                _fileIndex[e.FullPath] = item;

                var dirPath = Path.GetDirectoryName(e.FullPath);
                if (dirPath != null && _dirContents.TryGetValue(dirPath, out var list))
                    list.Add(item);

                OnCreated(this, item);
            }
            catch (Exception ex)
            {
                Log.Error(this, $"WatcherOnCreated: {e.FullPath}", ex.Message);
            }
        }

        private void WatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Renamed) return;

            Debug.WriteLine($"- OnRenamed: {e.OldFullPath} → {e.FullPath}");

            // Видаляємо старий запис
            if (_fileIndex.TryGetValue(e.OldFullPath, out var oldItem))
            {
                _fileIndex.Remove(e.OldFullPath);

                var oldDir = Path.GetDirectoryName(e.OldFullPath);
                if (oldDir != null && _dirContents.TryGetValue(oldDir, out var oldList))
                    oldList.Remove(oldItem);

                if (oldItem.IsDir)
                {
                    EvictDirectory(e.OldFullPath);
                    var prefix = e.OldFullPath + Path.DirectorySeparatorChar;
                    var keysToRemove = _dirContents.Keys
                        .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    foreach (var key in keysToRemove)
                    {
                        EvictDirectory(key);
                    }
                }

                OnDeleted(this, oldItem);
            }

            // Додаємо новий запис
            try
            {
                var newItem = new FileSystemInfoExt(e.FullPath);
                _fileIndex[e.FullPath] = newItem;

                var newDir = Path.GetDirectoryName(e.FullPath);
                if (newDir != null && _dirContents.TryGetValue(newDir, out var newList))
                    newList.Add(newItem);

                OnCreated(this, newItem);
            }
            catch (Exception ex)
            {
                Log.Error(this, $"WatcherOnRenamed: {e.FullPath}", ex.Message);
            }
        }

        public List<IFileSystemInfoExt> GetFiles(string path)
        {
            if (_dirContents.TryGetValue(path, out var cached))
            {
                // Поки watcher не стежив за цією папкою, файли могли змінитись або з'явитись/зникнути.
                // Швидко звіряємо кеш з реальним станом директорії.
                SyncCachedDirectory(path, cached);
                UpdateUsage(path);
                return cached;
            }

            DisableWatcher();

            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();

            var list = new List<IFileSystemInfoExt>();

            var dirs = Directory.GetDirectories(path)
                .Where(y => !_ignoreFolders.Contains(
                    Path.GetFileName(y)?.ToLowerInvariant(),
                    StringComparer.OrdinalIgnoreCase))
                .Select(x => new FileInfo(x).ToFileSystemInfoExt())
                .ToList(); // List<FileSystemInfoExt>
            dirs.Sort(_naturalComparer);

            var files = Directory.GetFiles(path)
                .Select(x => new FileInfo(x).ToFileSystemInfoExt())
                .ToList(); // List<FileSystemInfoExt>
            files.Sort(_naturalComparer);

            list.AddRange(dirs);
            list.AddRange(files);

            foreach (var item in list)
                _fileIndex[item.FileInfo.FullName] = item;

            _dirContents[path] = list;

            SetWatcher(path);
            UpdateUsage(path);

            return list;
        }

        public List<IFileSystemInfoExt> GetDirs(string path)
        {
            if (_dirContents.TryGetValue(path, out var cached))
            {
                SyncCachedDirectory(path, cached);
                UpdateUsage(path);
                return cached.Where(x => x.IsDir).ToList();
            }

            DisableWatcher();

            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();

            var dirs = Directory.GetDirectories(path)
                .Where(y => !_ignoreFolders.Contains(
                    Path.GetFileName(y)?.ToLowerInvariant(),
                    StringComparer.OrdinalIgnoreCase))
                .Select(x => new FileInfo(x).ToFileSystemInfoExt())
                .ToList(); // List<FileSystemInfoExt>
            dirs.Sort(_naturalComparer);

            // Index but don't fully cache directory (GetFiles will do that later)
            foreach (var dir in dirs)
                _fileIndex[dir.FileInfo.FullName] = dir;

            SetWatcher(path);
            UpdateUsage(path);

            return dirs.Cast<IFileSystemInfoExt>().ToList();
        }

        public List<IFileSystemInfoExt> GetAllFiles(string path)
        {
            // For recursive mode don't cache (rare case)
            DisableWatcher();

            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();

            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Select(x => new FileInfo(x).ToFileSystemInfoExt())
                .ToList(); // List<FileSystemInfoExt>
            files.Sort(_naturalComparer);

            SetWatcher(path);

            return files.Cast<IFileSystemInfoExt>().ToList();
        }

        public int GetCountFiles()
        {
            return _fileIndex.Values.Count(x => !x.IsDir);
        }

        // ── Sync ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Звіряє закешований список із реальним вмістом директорії.
        /// Видаляє зниклі файли, додає нові, оновлює FileInfo для змінених.
        /// Не стріляє події — викликається всередині GetFiles, після якого UI
        /// будується заново через OnRefreshDirectory з повним списком.
        /// </summary>
        private void SyncCachedDirectory(string dirPath, List<IFileSystemInfoExt> cached)
        {
            if (!Directory.Exists(dirPath)) return;

            var actualPaths = new HashSet<string>(
                Directory.GetFileSystemEntries(dirPath)
                    .Where(p => {
                        var name = Path.GetFileName(p);
                        return !_ignoreFolders.Contains(name?.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
                    }),
                StringComparer.OrdinalIgnoreCase);

            // 1. Видаляємо записи, які більше не існують
            var deleted = cached
                .Where(x => x?.FileInfo?.FullName == null || !actualPaths.Contains(x.FileInfo.FullName))
                .ToList();

            foreach (var item in deleted)
            {
                cached.Remove(item);
                if (item?.FileInfo?.FullName != null)
                {
                    _fileIndex.Remove(item.FileInfo.FullName);
                }
            }

            // 2. Додаємо файли, які з'явились
            var cachedPaths = new HashSet<string>(
                cached.Select(x => x?.FileInfo?.FullName).Where(name => name != null),
                StringComparer.OrdinalIgnoreCase);

            foreach (var fullPath in actualPaths)
            {
                if (cachedPaths.Contains(fullPath)) continue;

                try
                {
                    var newItem = new FileSystemInfoExt(fullPath);
                    cached.Add(newItem);
                    _fileIndex[fullPath] = newItem;
                }
                catch (Exception ex)
                {
                    Log.Error(this, $"SyncCachedDirectory: {fullPath}", ex.Message);
                }
            }

            // 3. Оновлюємо FileInfo для файлів, які змінились
            foreach (var item in cached)
            {
                if (item == null || item.IsDir || item.FileInfo?.FullName == null) continue;

                try
                {
                    var fi = new FileInfo(item.FileInfo.FullName);
                    if (!fi.Exists) continue;

                    if (fi.LastWriteTime != item.FileInfo.LastWriteTime ||
                        fi.Length != item.FileInfo.Length)
                    {
                        item.RefreshParam(item.FileInfo.FullName);
                    }
                }
                catch { /* файл може бути заблокований або видалений між кроками */ }
            }

            // 4. Сортуємо список: спочатку папки, потім файли, обидва списки в натуральному порядку
            var stringComparer = new NaturalSorting.NaturalStringComparer();
            var sorted = cached
                .OrderBy(x => !x.IsDir)
                .ThenBy(x => x?.FileInfo?.Name ?? string.Empty, stringComparer)
                .ToList();

            cached.Clear();
            cached.AddRange(sorted);
        }

        // ── LRU ────────────────────────────────────────────────────────────────

        private void UpdateUsage(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            var node = _recentDirs.Find(path);
            if (node != null)
                _recentDirs.Remove(node);

            _recentDirs.AddFirst(path);

            while (_recentDirs.Count > CacheCapacity)
            {
                var oldest = _recentDirs.Last.Value;
                _recentDirs.RemoveLast();
                EvictDirectory(oldest);
            }
        }

        private void EvictDirectory(string dirPath)
        {
            if (!_dirContents.TryGetValue(dirPath, out var entries)) return;

            foreach (var entry in entries)
                _fileIndex.Remove(entry.FileInfo.FullName);

            _dirContents.Remove(dirPath);
        }

        // ── Watcher helpers ────────────────────────────────────────────────────

        private void DisableWatcher() => _watcher.Stop();

        private void SetWatcher(string path) => _watcher?.SetWatchFolder(path);
    }
}
