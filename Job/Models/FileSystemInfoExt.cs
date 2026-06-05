using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Interfaces;
using Interfaces.PdfUtils;

namespace JobSpace.Models
{
    [Serializable]
    public class FileSystemInfoExt : INotifyPropertyChanged, IFileSystemInfoExt
    {
        private IFileSystemInfo _fileInfo = new Interfaces.Classes.FileInfo();
        private ColorSpaces _usedColorSpace;
        private string _creatorApp;
        private FileFormat _format = new FileFormat();

        public FileSystemInfoExt() { }

        public FileSystemInfoExt(FileSystemInfo systemInfo)
        {
            CopyParams(systemInfo);
        }

        public FileSystemInfoExt(string fullPath) : this(new FileInfo(fullPath)) { }

        private void CopyParams(FileSystemInfo si)
        {
            _fileInfo.Extension = si.Extension;
            _fileInfo.Exists = si.Exists;
            _fileInfo.Name = si.Name;
            _fileInfo.Attributes = si.Attributes;
            _fileInfo.FullName = si.FullName;
            _fileInfo.LastWriteTime = si.LastWriteTime;

            IsDir = (_fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;

            if (!IsDir && si is System.IO.FileInfo fileSi)
                _fileInfo.Length = fileSi.Length;
        }

        public IFileSystemInfo FileInfo
        {
            get => _fileInfo;
            set => SetProperty(ref _fileInfo, value);
        }

        public FileFormat Format
        {
            get => _format;
            set => SetProperty(ref _format, value);
        }

        public ColorSpaces UsedColorSpace
        {
            get => _usedColorSpace;
            set => SetProperty(ref _usedColorSpace, value);
        }

        public string CreatorApp
        {
            get => _creatorApp;
            set => SetProperty(ref _creatorApp, value);
        }

        public bool IsDir { get; private set; }
        public string FullName => _fileInfo?.FullName;
        public string Name => _fileInfo?.Name;
        public HashSet<string> UsedColors { get; set; } = new HashSet<string>();

        public void RefreshParam(string fullPath)
        {
            var si = new FileInfo(fullPath);
            if (si.Exists)
            {
                CopyParams(si);
                OnPropertyChanged(nameof(FileInfo));
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(Name));
            }
        }

        public void RefreshParam(FileSystemInfoExt other)
        {
            if (other == null) return;

            FileInfo = other.FileInfo;
            Format = other.Format;
            UsedColorSpace = other.UsedColorSpace;
            CreatorApp = other.CreatorApp;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj) =>
            obj is FileSystemInfoExt other && string.Equals(FullName, other.FullName, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() =>
            StringComparer.OrdinalIgnoreCase.GetHashCode(FullName ?? string.Empty);
    }
}
