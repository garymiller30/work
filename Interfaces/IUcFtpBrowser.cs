using System;
using System.Collections;
using Interfaces.Ftp;

namespace Interfaces
{
    public interface IUcFtpBrowser
    {
        string GetClientNamesWithNewFiles();
        IUserProfile UserProfile { get; set; }
        void Init();
        event EventHandler<IDownloadTicket> OnCreateOrder;
        event EventHandler<IDownloadTicket> OnAddFilesToOrder;
        event EventHandler<IDownloadTicket> OnCreateOrderFromDir;
        event EventHandler<IDownloadTicket> OnCreateOrderFromDirLikeDescription;//Tuple<string, IDownloadFileParam>
        IFtpState FtpStates { get; set; }
        IDownloadFileParam GetDownloadFileParam(IList modelObjects);
    }
}
