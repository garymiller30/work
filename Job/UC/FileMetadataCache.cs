using Interfaces;
using System;
using System.Collections.Generic;

namespace JobSpace.UC
{
    /// <summary>
    /// Кеш метаданих файлів (ширина, висота, кількість сторінок тощо).
    /// Живе весь час роботи браузера і не прив'язаний до конкретної папки.
    /// Метадані вважаються актуальними, якщо LastWriteTime і Length файлу не змінились
    /// з моменту останнього сканування.
    /// </summary>
    public sealed class FileMetadataCache
    {
        private readonly Dictionary<string, Entry> _cache =
            new Dictionary<string, Entry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Повертає true, якщо метадані для цього файлу вже є в кеші
        /// і файл не змінювався з моменту останнього сканування.
        /// </summary>
        public bool IsUpToDate(IFileSystemInfoExt file)
        {
            if (file == null || file.IsDir || file.FileInfo == null)
                return false;

            var path = file.FileInfo.FullName;
            var lastWrite = file.FileInfo.LastWriteTime;
            var length = file.FileInfo.Length;

            return _cache.TryGetValue(path, out var entry)
                && entry.LastWriteTime == lastWrite
                && entry.Length == length;
        }

        /// <summary>
        /// Позначає метадані файлу як актуальні (викликати після GetExtendedFileInfoFormat).
        /// </summary>
        public void MarkUpToDate(IFileSystemInfoExt file)
        {
            if (file == null || file.IsDir || file.FileInfo == null)
                return;

            _cache[file.FileInfo.FullName] = new Entry(
                file.FileInfo.LastWriteTime,
                file.FileInfo.Length);
        }

        /// <summary>
        /// Інвалідує запис для конкретного файлу (файл змінено або видалено ззовні).
        /// </summary>
        public void Invalidate(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath))
                _cache.Remove(fullPath);
        }

        private readonly struct Entry
        {
            public readonly DateTime LastWriteTime;
            public readonly long Length;

            public Entry(DateTime lastWriteTime, long length)
            {
                LastWriteTime = lastWriteTime;
                Length = length;
            }
        }
    }
}
