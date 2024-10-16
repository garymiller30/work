using Interfaces;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginGMail
{
    public partial class WindowOut : UserControl, IPluginInfo
    {

        IJob _job;

        public WindowOut()
        {
            InitializeComponent();
            webView21.Source = new Uri("https://gmail.com");
            webView21.CoreWebView2InitializationCompleted += WebView21_CoreWebView2InitializationCompleted;
        }

        private void WebView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webView21.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;
        }

        private void CoreWebView2_DownloadStarting(object sender, CoreWebView2DownloadStartingEventArgs e)
        {
            var downloadState = new DownloadState()
            {
                Job = _job,
                DownloadOperation = e.DownloadOperation
            };


            downloadState.DownloadOperation.StateChanged += delegate (object s, object ev)
            {
                switch (downloadState.DownloadOperation.State)
                {
                    case CoreWebView2DownloadState.InProgress:
                        break;
                    case CoreWebView2DownloadState.Interrupted: break;
                    case CoreWebView2DownloadState.Completed:
                        if (string.IsNullOrEmpty(downloadState.Job.Description))
                        {
                            var fileName = Path.GetFileNameWithoutExtension(downloadState.DownloadOperation.ResultFilePath);
                            UserProfile.Jobs.ChangeJobDescription(downloadState.Job, fileName);
                        }
                        break;
                    default: break;
                }
            };
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
            try
            {
                _job = curJob;
                var folder = UserProfile.Jobs.GetFullPathToWorkFolder(curJob);
                webView21.CoreWebView2.Profile.DefaultDownloadFolderPath = folder;

            }
            catch
            {

            }
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

        private void tsb_ok_Click(object sender, EventArgs e)
        {
            var res = double.TryParse(toolStripTextBox1.Text, out double factor);

            if (res)
            {
                webView21.ZoomFactor = factor / 100;
            }
        }
    }
}
