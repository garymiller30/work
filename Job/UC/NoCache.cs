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
        private List<string> _ignoreFolders = new List<string>(){"temp",".signa",".preview",".impos" };

        // Кеш для швидкого доступу до метаданих файлів та папок. Ключ - повний шлях.
        private Dictionary<string, IFileSystemInfoExt> _fileCache = new Dictionary<string, IFileSystemInfoExt>(StringComparer.InvariantCultureIgnoreCase);

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

                // 1. Видаляємо старий елемент з кешу та списку
                if (_fileCache.Remove(e.OldFullPath))
                {
                    var oldItem = _fileCache.Values.FirstOrDefault(x => x.FileInfo.FullName.Equals(e.OldFullPath, StringComparison.CurrentCultureIgnoreCase));
                    if (oldItem != null)
                    {
                        _files.Remove(oldItem);
                        OnDeleted(this, oldItem);
                    }
                }

                // 2. Перевіряємо, чи існує новий елемент у кеші (можливо, він був просто перейменований)
                var newItem = _fileCache.Values.FirstOrDefault(x =>
                    x.FileInfo.FullName.Equals(e.FullPath, StringComparison.CurrentCultureIgnoreCase));

                if (newItem == null) 
                {
                    // 3. Додаємо новий елемент до кешу та списку
                    try
                    {
                        var newItemInstance = new FileSystemInfoExt(e.FullPath);
                        _fileCache[e.FullPath] = newItemInstance;
                        _files.Add(newItemInstance);
                        OnCreated(this, newItemInstance);
                    }
                    catch (Exception ex)
                    {
                         Log.Error(this, $"WatcherOnRenamed: Failed to create new item {e.FullPath}", ex.Message);
                    }
                }
                else
                {
                    // 4. Оновлюємо метадані і сповіщаємо про зміну
                    newItem.RefreshParam(e.FullPath);
                    _files.Remove(newItem); // Видаляємо старий об'єкт зі списку, щоб додати оновлений
                    _files.Add(newItem);
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
                        _fileCache[e.FullPath] = fsie; // Додаємо до кешу
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
                // Видаляємо з кешу та списку
                if (_fileCache.Remove(e.FullPath))
                {
                    var oldItem = _files.FirstOrDefault(x => x.FileInfo.FullName.Equals(e.FullPath, StringComparison.InvariantCultureIgnoreCase));
                    if (oldItem != null)
                    {
                        OnDeleted(this, oldItem);
                        _files.Remove(oldItem);
                    }
                }
            }

        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                Debug.WriteLine($"- OnChanged: e.FullPath: {e.FullPath}");

                // Оновлюємо метадані в кеші та списку
                var cachedItem = _fileCache.Values.FirstOrDefault(x =>
                    x.FileInfo.FullName.Equals(e.FullPath, StringComparison.InvariantCultureIgnoreCase));
                
                if (cachedItem != null)
                {
                    // Оновлюємо об'єкт у кеші
                    cachedItem.RefreshParam(e.FullPath); 

                    // Перезаписуємо в списку _files для коректного відображення змін
                    var index = _files.IndexOf(cachedItem);
                    if (index != -1)
                    {
                        _files[index] = cachedItem;
                        OnChanged(this, cachedItem);
                    }
                }
            }

        }


        NaturalSorting.NaturalFileInfoNameComparer _naturalCompaper = new NaturalSorting.NaturalFileInfoNameComparer();


        public List<IFileSystemInfoExt> GetFiles(string path)
        {
            DisableWatcher();

            // Очищаємо кеш та список перед повним скануванням, якщо це не повторний запит до того ж шляху
            if (!_fileCache.ContainsKey(path)) 
            {
                _files.Clear();
                _fileCache.Clear();
            }


            if (!Directory.Exists(path)) return _files;
           
            // Скануємо та заповнюємо кеш/список папок
            var dirs = Directory.GetDirectories(path)
                .Where(y => !_ignoreFolders.Contains(Path.GetFileName(y).ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                .Select(x => new FileInfo(x).ToFileSystemInfoExt())
                .ToList();

            foreach (var dir in dirs)
            {
                 _fileCache[dir.FileInfo.FullName] = dir;
                 _files.Add(dir);
            }
            dirs.Sort(_naturalCompaper);


            // Скануємо та заповнюємо кеш/список файлів
            var f = Directory.GetFiles(path).Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();

            foreach (var file in f)
            {
                _fileCache[file.FileInfo.FullName] = file; // Додаємо до кешу
                _files.Add(file);
            }
            f.Sort(_naturalCompaper);


            SetWatcher(path);

            return _files;
        }

        public List<IFileSystemInfoExt> GetDirs(string path)
        {
            // Якщо шлях вже в кеші, повертаємо його вміст (якщо він є папкою)
            if (_fileCache.TryGetValue(path, out var cachedDir) && cachedDir.IsDir)
            {
                return new List<IFileSystemInfoExt> { cachedDir };
            }

            DisableWatcher();

            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();

            var dirs = Directory.GetDirectories(path)
                 .Where(y => !_ignoreFolders.Contains(Path.GetFileName(y).ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                 .Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            
            foreach (var dir in dirs)
            {
                _fileCache[dir.FileInfo.FullName] = dir; // Кешуємо знайдені папки
            }

            dirs.Sort(_naturalCompaper);

            SetWatcher(path);

            return dirs.Cast<IFileSystemInfoExt>().ToList();
        }

        public List<IFileSystemInfoExt> GetAllFiles(string path)
        {
            DisableWatcher();

            // Очищаємо кеш та список перед повним скануванням, якщо це не повторний запит до того ж шляху
            if (!_fileCache.ContainsKey(path)) 
            {
                _files.Clear();
                _fileCache.Clear();
            }


            if (!Directory.Exists(path)) return _files;

            // Скануємо та заповнюємо кеш/список файлів рекурсивно
            var f = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            
            foreach (var file in f)
            {
                _fileCache[file.FileInfo.FullName] = file; // Додаємо до кешу
                _files.Add(file);
            }

            f.Sort(_naturalCompaper);

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
            // Використовуємо кеш для швидкого підрахунку, якщо він доступний
            return _fileCache.Values.Count(x => !x.IsDir);
        }
    }
}
