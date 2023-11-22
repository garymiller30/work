// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using Interfaces;

namespace Job.Profiles.ProfileEvents
{
    public sealed class BrowserEvents : AbstractEvents, IBrowserEvents
    {
        public event EventHandler OnCustomButtonClick = delegate { };
        public event EventHandler<string[]> OnFtpUpload = delegate { };
        public event EventHandler<string> OnDropHttpLink = delegate { };
        public event EventHandler<string> OnChangeJobDescription = delegate { };
        public event EventHandler<string> OnMoveFileToArchive = delegate { };
        public event EventHandler<IJob> OnChangeStatus = delegate { };
        public event EventHandler<string> OnCreateOrderFromFile = delegate { };

        public override void Init(IUserProfile profile)
        {
            for (int i = 0; i < profile.FileBrowser.Browsers.Count; i++)
            {
                var fb = profile.FileBrowser.Browsers[i];

                if (i == 0)
                {
                    fb.OnCustomButtonClick += (sender, args) => OnCustomButtonClick(sender, args);
                    fb.OnDropHttpLink += (sender, s) => OnDropHttpLink(sender, s);
                }

                fb.OnFtpUpload += (sender, strings) => OnFtpUpload(sender, strings);
                fb.OnChangeJobDescription += (sender, s) => OnChangeJobDescription(sender, s);
                fb.OnMoveFileToArchive += (sender, s) => OnMoveFileToArchive(sender, s);
                fb.OnChangeStatus += (sender, job) => OnChangeStatus(sender, job);
                fb.OnCreateOrderFromFile += (sender, file) => OnCreateOrderFromFile(sender, file);
            }
        }

    }


}
