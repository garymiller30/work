using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IFileBrowser
    {
        void SaveSettings();
        void InitToolStripUtils(int idx);
        void LoadSettings();
        void SetRootFolder(string directory);
        //IJob CurrentJob { get; set; }
        void LockUI(bool locked);
        //IUserProfile UserProfile { get; set; }
        string DefaultSettingsFolder { get; set; }
        void SetCustomButtonPath(string[] customPath);
        IFileBrowserControlSettings GetSettings();
        List<IFileSystemInfoExt> GetFilesFromDirectory(string path);

        event EventHandler OnCustomButtonClick;
        event EventHandler<string[]> OnFtpUpload;
        event EventHandler<string> OnDropHttpLink;
        event EventHandler<string> OnChangeJobDescription;
        event EventHandler<string> OnMoveFileToArchive;
        event EventHandler<IJob> OnChangeStatus;
        event EventHandler<string> OnCreateOrderFromFile;

    }
}
