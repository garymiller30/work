using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Interfaces;
using Interfaces.PdfUtils;

namespace JobSpace.Models
{
    [Serializable]
    public  class FileSystemInfoExt : INotifyPropertyChanged, IFileSystemInfoExt
    {
        private IFileSystemInfo _fileInfo = new Interfaces.Classes.FileInfo();
        private ColorSpaces _usedColorSpace;

        private FileFormat _format = new FileFormat();


        public FileSystemInfoExt()
        {

        }

        public FileSystemInfoExt(FileSystemInfo systemInfo)
        {
            CopyParams(systemInfo);
            //_fileInfo = systemInfo;
            
        }

        private void CopyParams(FileSystemInfo si)
        {
            _fileInfo.Extension = si.Extension;
            _fileInfo.Exists = si.Exists;
            _fileInfo.Name = si.Name;
            _fileInfo.Attributes = si.Attributes;
            SetIsDir();
            _fileInfo.FullName = si.FullName;
            _fileInfo.LastWriteTime = si.LastWriteTime;
            if (!IsDir)
                _fileInfo.Length = ((System.IO.FileInfo)si).Length;
        }

        public FileSystemInfoExt(string fullPath)
        {
            CopyParams(new FileInfo(fullPath));
           
        }

        private void SetIsDir()
        {
            IsDir = (_fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public IFileSystemInfo FileInfo
        {
            get => _fileInfo;
            set
            {
                _fileInfo = value;
                OnPropertyChanged();
            }
        }

        public FileFormat Format
        {
            get => _format;
            set { _format = value; OnPropertyChanged(); }
        }

        public ColorSpaces UsedColorSpace
        {
            get => _usedColorSpace;
            set { _usedColorSpace = value; OnPropertyChanged(); }
        }

        public bool IsDir { get; set; }

        public void RefreshParam(string fullPath)
        {
            var fsie = new FileSystemInfoExt(fullPath);
            if (fsie.FileInfo.Exists)
            {
                RefreshParam(fsie);
                OnPropertyChanged("FileInfo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RefreshParam(FileSystemInfoExt newFileSystemInfoExt)
        {
            _fileInfo = newFileSystemInfoExt.FileInfo;
            _format = newFileSystemInfoExt.Format;
            _usedColorSpace = newFileSystemInfoExt.UsedColorSpace;

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

