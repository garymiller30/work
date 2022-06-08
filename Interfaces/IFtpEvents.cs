using Interfaces.Ftp;
using System;

namespace Interfaces
{
    public interface IFtpEvents
    {
        event EventHandler<IDownloadTicket> OnCreateOrder;
        event EventHandler<IDownloadTicket> OnAddFilesToOrder;
        event EventHandler<IDownloadTicket> OnCreateOrderFromDir;
        event EventHandler<IDownloadTicket> OnCreateOrderFromDirLikeDescription;
        event EventHandler<bool> ChangeStatus;
        void Init(IUserProfile profile);
    }
}
