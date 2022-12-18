using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginGMail
{
    public partial class WindowOut : UserControl, IPluginInfo
    {
        public WindowOut()
        {
            InitializeComponent();
            webView21.Source = new Uri("https://gmail.com");
        }

        public IUserProfile UserProfile { get; set; }

        public string PluginName => "GMail";

        public string PluginDescription => "доступ до сайту gmail.com";

        public void AfterJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }

        public void BeforeJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }

        public string GetPluginName()
        {
            return PluginName;
            //throw new NotImplementedException();
        }

        public UserControl GetUserControl()
        {
            return this;
            //throw new NotImplementedException();
        }

        public void SetCurJob(IJob curJob)
        {
            if (curJob == null) { return; }
            var folder = UserProfile.Jobs.GetFullPathToWorkFolder(curJob);
            webView21.CoreWebView2.Profile.DefaultDownloadFolderPath = folder;
            //throw new NotImplementedException();
        }

        public void ShowSettingsDlg()
        {
            //throw new NotImplementedException();
        }

        public void Start()
        {
            //throw new NotImplementedException();
        }
    }
}
