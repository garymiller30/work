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
        IUserProfile _profile;
        bool isInitialized = false;
        List<ToolStripLabel> labels = new List<ToolStripLabel>();

        public WindowOut()
        {
            InitializeComponent();

        }

        private async void Init()
        {
            if (UserProfile == null) { return; }
            webView21.CoreWebView2InitializationCompleted += WebView21_CoreWebView2InitializationCompleted;

            await InitializeAsync();

            webView21.Source = new Uri("https://gmail.com");

            SetZoom();
            SetDefaultDownloadFolder();
        }

        private async Task InitializeAsync()
        {
            if (isInitialized) { return; }

            string userDataFolder = Path.Combine(Path.GetTempPath(), $"{Environment.UserName}\\aw_gmail\\{UserProfile.Settings.ProfileName}");
            if (!Directory.Exists(userDataFolder)) { Directory.CreateDirectory(userDataFolder); }

            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-features=msSmartScreenProtection");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(
               browserExecutableFolder: null,
                userDataFolder: Path.Combine(userDataFolder),
                options);

            await webView21.EnsureCoreWebView2Async(environment);

        }

        private void SetDefaultDownloadFolder()
        {
            if (_job == null || UserProfile == null || UserProfile.Jobs == null)
            {

                return;
            }

            var folder = UserProfile.Jobs.GetFullPathToWorkFolder(_job);
            if (webView21?.CoreWebView2?.Profile != null && Directory.Exists(folder))
            {
                webView21.CoreWebView2.Profile.DefaultDownloadFolderPath = folder;
                SetDownloadPathLabel();
            }
        }

        private void SetDownloadPathLabel()
        {
            foreach (var lbl in labels)
            {
                toolStrip2.Items.Remove(lbl);
            }

            var lblPath = new ToolStripLabel()
            {
                Text = _job.Customer,
                AutoSize = true,
                BackColor = Color.LightBlue
            };
            labels.Add(lblPath);
            toolStrip2.Items.Add(lblPath);

            var lblSep = new ToolStripLabel()
            {
                Text = " > ",
                AutoSize = true
            };
            labels.Add(lblSep);
            toolStrip2.Items.Add(lblSep);
            var lblFolder = new ToolStripLabel()
            {
                Text = _job.Description,
                AutoSize = true
            };
            labels.Add(lblFolder);
            toolStrip2.Items.Add(lblFolder);
        }

        private void WebView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) { MessageBox.Show($"{e.InitializationException}"); }
            else
            {
                webView21.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;
                isInitialized = true;
            }
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

        public IUserProfile UserProfile
        {
            get => _profile;
            set
            {
                _profile = value;
            }
        }

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
            _job = curJob;
            SetDefaultDownloadFolder();
        }

        public void ShowSettingsDlg()
        {
            //throw new NotImplementedException();
        }

        public void Start()
        {
            Init();
            //throw new NotImplementedException();
        }

        private void tsb_ok_Click(object sender, EventArgs e)
        {

        }

        private void tsb_okZoom_Click(object sender, EventArgs e)
        {
            SetZoom();
        }

        private void SetZoom()
        {
            var res = double.TryParse(toolStripTextBox1.Text, out double factor);

            if (res)
            {
                webView21.ZoomFactor = factor / 100;
            }
        }

        private void tsb_start_Click(object sender, EventArgs e)
        {
            Init();
        }
    }
}
