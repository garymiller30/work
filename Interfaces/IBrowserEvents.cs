using System;

namespace Interfaces
{
    public interface IBrowserEvents
    {
        event EventHandler OnCustomButtonClick;
        event EventHandler<string[]> OnFtpUpload;
        event EventHandler<string> OnDropHttpLink;
        event EventHandler<string> OnChangeJobDescription;
        event EventHandler<string> OnMoveFileToArchive;
        event EventHandler<IJob> OnChangeStatus;
        event EventHandler<string> OnCreateOrderFromFile;
        //event EventHandler<IJob,IDownloadFileParam> OnDownloadFilesFromFtp;

        void Init(IUserProfile profile);
    }
}
