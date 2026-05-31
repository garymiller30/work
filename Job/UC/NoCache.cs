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

        // Список для відстеження останніх використаних шляхів (LRU)
        private readonly LinkedList<string> _recentPaths = new LinkedList<string>();
        private const int CacheCapacity = 10; // Максимальна кількість кешованих папок/шляхів

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
                    var oldItem = _files.FirstOrDefault(x => x.FileInfo.FullName.Equals(e.OldFullPath, StringComparison.CurrentCultureIgnoreCase));
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

                        // Оновлюємо використання нового шляху
                        UpdateUsage(_fileCache.Keys.FirstOrDefault(k => k == e.FullPath));
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

                    // Оновлюємо використання шляху, який змінився
                    UpdateUsage(e.FullPath);
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

                        // Оновлюємо використання нового шляху (якщо це папка)
                        if (fsie.IsDir)
                        {
                            UpdateUsage(e.FullPath);
                        }
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

                    // Видаляємо шлях з історії використання, оскільки він більше не існує
                    RemovePathFromUsage(e.FullPath);
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

                    // Оновлюємо використання шляху, який змінився
                    UpdateUsage(e.FullPath);
                }
            }

        }


        NaturalSorting.NaturalFileInfoNameComparer _naturalCompaper = new NaturalSorting.NaturalFileInfoNameComparer();

        /// <summary>
        /// Оновлює порядок використання шляху (LRU). Переміщує шлях на початок списку, якщо він існує.
        /// </summary>
        private void UpdateUsage(string path)
        {
            if (path == null || string.IsNullOrWhiteSpace(path)) return;

            // 1. Видаляємо старий запис, якщо він є
            var node = _recentPaths.Find(path);
            if (node != null)
            {
                _recentPaths.Remove(node);
            }

            // 2. Додаємо на початок списку
            _recentPaths.AddFirst(path);

            // 3. Обмежуємо розмір кешу, видаляючи найстаріший елемент (з кінця)
            while (_recentPaths.Count > CacheCapacity)
            {
                var oldestPath = _recentPaths.Last.Value;
                _recentPaths.RemoveLast();

                // Видаляємо відповідний запис з основного кешу, щоб звільнити пам'ять
                if (_fileCache.ContainsKey(oldestPath))
                {
                    _fileCache.Remove(oldestPath);
                }
            }
        }

        /// <summary>
        /// Видаляє шлях з історії використання (використовується при видаленні файлу/папки).
        /// </summary>
        private void RemovePathFromUsage(string path)
        {
             var node = _recentPaths.Find(path);
            if (node != null)
            {
                _recentPaths.Remove(node);
            }
        }


        public List<IFileSystemInfoExt> GetFiles(string path)
        {
            // 1. Перевірка кешу: Якщо шлях є в кеші, повертаємо дані з нього та оновлюємо використання.
            if (_fileCache.ContainsKey(path))
            {
                UpdateUsage(path);
                return _files; // Повертаємо вже завантажений список
            }

            DisableWatcher();

            // 2. Якщо в кеші немає, виконуємо сканування та заповнюємо кеш/список папок
            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();
           
            _files.Clear(); // Очищаємо список для нового шляху

            // Скануємо та заповнюємо кеш/список папок
            var dirs = Directory.GetDirectories(path)
                .Where(y => !_ignoreFolders.Contains(Path.GetFileName(y).ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                .Select(x => new FileInfo(x).ToFileSystemInfoExt())
                .ToList();

            foreach (var dir in dirs)
            {
                 _fileCache[dir.FileInfo.FullName] = dir; // Кешуємо знайдені папки
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


            // 3. Встановлюємо спостерігач та оновлюємо використання шляху
            SetWatcher(path);
            UpdateUsage(path);

            return _files;
        }

        public List<IFileSystemInfoExt> GetDirs(string path)
        {
            // 1. Перевірка кешу: Якщо шлях є в кеші, повертаємо дані з нього та оновлюємо використання.
            if (_fileCache.ContainsKey(path))
            {
                UpdateUsage(path);
                var cachedDir = _fileCache[path];
                return new List<IFileSystemInfoExt> { cachedDir };
            }

            DisableWatcher();

            // 2. Якщо в кеші немає, виконуємо сканування та заповнюємо кеш/список папок
            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();

            _files.Clear(); // Очищаємо список для нового шляху

            var dirs = Directory.GetDirectories(path)
                 .Where(y => !_ignoreFolders.Contains(Path.GetFileName(y).ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                 .Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            
            foreach (var dir in dirs)
            {
                _fileCache[dir.FileInfo.FullName] = dir; // Кешуємо знайдені папки
                _files.Add(dir);
            }

            dirs.Sort(_naturalCompaper);

            // 3. Встановлюємо спостерігач та оновлюємо використання шляху
            SetWatcher(path);
            UpdateUsage(path);

            return dirs.Cast<IFileSystemInfoExt>().ToList();
        }

        public List<IFileSystemInfoExt> GetAllFiles(string path)
        {
            // 1. Перевірка кешу: Якщо шлях є в кеші, повертаємо дані з нього та оновлюємо використання.
            if (_fileCache.ContainsKey(path))
            {
                UpdateUsage(path);
                return _files; // Повертаємо вже завантажений список
            }

            DisableWatcher();

            // 2. Якщо в кеші немає, виконуємо сканування та заповнюємо кеш/список файлів рекурсивно
            if (!Directory.Exists(path)) return new List<IFileSystemInfoExt>();

            _files.Clear(); // Очищаємо список для нового шляху

            // Скануємо та заповнюємо кеш/список файлів рекурсивно
            var f = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Select(x => new FileInfo(x).ToFileSystemInfoExt()).ToList();
            
            foreach (var file in f)
            {
                _fileCache[file.FileInfo.FullName] = file; // Додаємо до кешу
                _files.Add(file);
            }

            f.Sort(_naturalCompaper);

            // 3. Встановлюємо спостерігач та оновлюємо використання шляху
            SetWatcher(path);
            UpdateUsage(path);

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
