using Interfaces.FileBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
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
        Task RefreshAsync(string selectFileName = null);
        void DirectoryUp();
        void SetRootDirectory(string rootDir);
        List<IFileSystemInfoExt> GetFiles(string path);
        List<IFileSystemInfoExt> GetDirs();
        void GetTempFolder();

        void MoveFilesToTrash(IFileSystemInfoExt[] files);

        FileBrowserSettings Settings { get; set; }
        IToolbarSettings LoadToolbarSettings();
        void SaveToolbarSettings(IToolbarSettings settings);
        List<ToolInfo>LoadPdfTools();


        void MoveFolderContentsToHere(IFileSystemInfoExt folder, bool appendFolderName);
        Task GetAllFilesWithoutDir();
        void MoveTo(IFileSystemInfoExt file, string targetDir);
        void PasteFromClipboardLikeCopy(string[] strings);
        void PasteFromClipboardWithSpecificName(string v, IFileSystemInfoExt targetfile);
    }
}
