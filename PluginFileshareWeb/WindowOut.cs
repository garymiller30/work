using Interfaces;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public WindowOut()
        {
            InitializeComponent();
            webView21.CoreWebView2InitializationCompleted += WebView21_CoreWebView2InitializationCompleted;

        }

        private void WebView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
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

    }
}
