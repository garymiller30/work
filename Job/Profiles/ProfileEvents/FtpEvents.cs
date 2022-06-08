using System;
using Interfaces;
using Interfaces.Ftp;

namespace Job.Profiles.ProfileEvents
{
    public class FtpEvents: AbstractEvents, IFtpEvents
    {
        public event EventHandler<IDownloadTicket> OnCreateOrder = delegate { };
        public event EventHandler<IDownloadTicket> OnAddFilesToOrder = delegate { };
        public event EventHandler<IDownloadTicket> OnCreateOrderFromDir = delegate { };
        public event EventHandler<IDownloadTicket> OnCreateOrderFromDirLikeDescription = delegate { };
        public event EventHandler<bool> ChangeStatus = delegate { };

        public override void Init(IUserProfile profile)
        {
            profile.Ftp.FtpExplorer.OnAddFilesToOrder += (sender, param) => OnAddFilesToOrder(sender, param);
            profile.Ftp.FtpExplorer.OnCreateOrder += (sender, param) => OnCreateOrder(sender, param);
            profile.Ftp.FtpExplorer.OnCreateOrderFromDir += (sender, tuple) => OnCreateOrderFromDir(sender, tuple);
            profile.Ftp.FtpExplorer.OnCreateOrderFromDirLikeDescription +=
                (sender, tuple) => OnCreateOrderFromDirLikeDescription(sender, tuple);

            profile.Ftp.FtpExplorer.FtpStates.OnChangeStatus += ChangeStatus;

        }
    }


}
