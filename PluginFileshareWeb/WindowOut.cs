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
using static System.Windows.Forms.LinkLabel;

namespace PluginFileshareWeb
{
    public partial class WindowOut : UserControl, IPluginInfo
    {
        IJob _curJob = null;
        FileShareWebSettings _settings = null;

        public IUserProfile UserProfile { get; set; }

        public string PluginName => "Швидкі посилання";

        public string PluginDescription => "Популярні файлообмінники";

        public  WindowOut()
        {
            InitializeComponent();
            _ = InitializeAsync();
            webView21.CoreWebView2InitializationCompleted += WebView21_CoreWebView2InitializationCompleted;

        }

        private async Task InitializeAsync()
        {
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-features=msSmartScreenProtection");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(
               browserExecutableFolder: null,
                userDataFolder: Path.Combine(Path.GetTempPath(), $"{Environment.UserName}", "aw_shares"),
                options);
            await webView21.EnsureCoreWebView2Async(environment);

            //CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-features=msSmartScreenProtection"); 
            //CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            //await webView21.EnsureCoreWebView2Async(environment);
        }

        private void WebView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webView21.Source = new Uri("about:blank");
            webView21.Source = new Uri(((ToolStripButton)sender).Tag.ToString());
        }

        public UserControl GetUserControl()
        {
            return this;
        }

        public void Start()
        {
            _settings = UserProfile.Plugins.LoadSettings<FileShareWebSettings>();
            foreach (var link in _settings.Links)
            {
                var button = CreateButton(link);
                toolStrip1.Items.Add(button);
            }
        }

        private ToolStripButton CreateButton(LinkInfo link)
        {
            var button = new ToolStripButton(link.Name);
            button.Tag = link.Url;
            button.Click += toolStripButton1_Click;

            return button;
        }


        public string GetPluginName()
        {
            return PluginName;
        }

        public void SetCurJob(IJob curJob)
        {
            if (curJob == null) { return; }

            if (webView21.CoreWebView2 == null) return;

            _curJob = curJob;
            var folder = UserProfile.Jobs.GetFullPathToWorkFolder(curJob);
            webView21.CoreWebView2.Profile.DefaultDownloadFolderPath = folder;
        }

        public void BeforeJobChange(IJob job)
        {
        }

        public void AfterJobChange(IJob job)
        {
         
        }

        public void ShowSettingsDlg()
        {
            
        }

        private void toolStripButton_Add_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddLink())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _settings.Links.Add(form.LinkInfo);
                    UserProfile.Plugins.SaveSettings(_settings);
                    var button = CreateButton(form.LinkInfo);
                    toolStrip1.Items.Add(button);
                }
            }
        }

        private void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(toolStripTextBoxUrl.Text))
            {
                try
                {
                    webView21.Source = new Uri(EnsureUrlHasProtocol(toolStripTextBoxUrl.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
                
            }
                
        }

        string EnsureUrlHasProtocol(string urlWithoutProtocol)
        {
            if (!urlWithoutProtocol.StartsWith("http://") && !urlWithoutProtocol.StartsWith("https://"))
            {
                return "https://" + urlWithoutProtocol;
            }
            return urlWithoutProtocol;
        }

        private void toolStripTextBoxUrl_Click(object sender, EventArgs e)
        {
            toolStripTextBoxUrl.SelectAll();
        }

        private void tsb_zoomOk_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;

            var res = double.TryParse(tstb_zoomFactor.Text, out var factor);

            if (res)
            webView21.ZoomFactor = factor/100;
        }
    }
}
