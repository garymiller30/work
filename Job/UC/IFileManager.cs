using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;

namespace Job.UC
{
    public interface IFileManager
    {
        event EventHandler<IFileSystemInfoExt> OnAddFile;
        event EventHandler<IFileSystemInfoExt> OnDeleteFile;
        event EventHandler<IFileSystemInfoExt> OnChangeFile;
        event EventHandler<IFileSystemInfoExt> OnChangeDirectory;
        event EventHandler OnChangeRootDirectory;
        event EventHandler<List<IFileSystemInfoExt>> OnRefreshDirectory;
        event EventHandler<IFileSystemInfoExt> OnSelectParent;
        event EventHandler<string> OnSelectFileName;
        event EventHandler<string> OnError;

        bool UseNaturalSorting { get; set; }

        void LoadSettings(string settingFile);
        void SaveSettings(string settingFile);

        int GetCountFiles();
        void OpenFileOrFolder(IFileSystemInfoExt fileOrDirectory);
        void DeleteFilesAndDirectories(IEnumerable<IFileSystemInfoExt> files);
        void PasteFromClipboard(string[] files);
        void CreateDirectoryInCurrentFolder(string name);
        
        void MoveFileOrDirectoryToCurrentFolder(IFileSystemInfoExt file, string newName);
        void RefreshAsync(string selectFileName = null);
        void DirectoryUp();
        void SetRootDirectory(string rootDir);
        //void CreateJdf(IEnumerable<IFileSystemInfoExt> files);
        List<IFileSystemInfoExt> GetFiles(string path);
        void GetTempFolder();

        void MoveFilesToTrash(IFileSystemInfoExt[] files);

        FileBrowserSettings Settings { get; set; }

        void MoveFolderContentsToHere(IFileSystemInfoExt folder);
        void GetAllFilesWithoutDir();
        //IFileManager UseWatcher(IWatcher watcher);
        //IFileManager UseCache(ICache cache);
    }
}
