using Interfaces;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
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
        string _curJobDir = null;
        FileShareWebSettings _settings = null;
        CoreWebView2Environment environment;
        CoreWebView2EnvironmentOptions options;
        WebView2 curwebView2 = null;
        double zoomFactor = 80;
        public IUserProfile UserProfile { get; set; }

        public string PluginName => "Швидкі посилання";

        public string PluginDescription => "Популярні файлообмінники v2.0";

        public WindowOut()
        {
            InitializeComponent();
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            options = new CoreWebView2EnvironmentOptions("--disable-features=msSmartScreenProtection");
            environment = await CoreWebView2Environment.CreateAsync(
               browserExecutableFolder: null,
                userDataFolder: Path.Combine(Path.GetTempPath(), $"{Environment.UserName}", "aw_shares"),
                options);
        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {

             await AddTabAsync();

            tabControl1.SelectedTab.Text = ((ToolStripButton)sender).Text;

            curwebView2.Source = new Uri("about:blank");
            curwebView2.Source = new Uri(((ToolStripButton)sender).Tag.ToString());
            
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

            _curJob = curJob;
            _curJobDir = UserProfile.Jobs.GetFullPathToWorkFolder(curJob);

            if (curwebView2?.CoreWebView2 == null) return;
            curwebView2.CoreWebView2.Profile.DefaultDownloadFolderPath = _curJobDir;
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
                    if (curwebView2 == null) { AddTabAsync(); }

                    curwebView2.Source = new Uri(EnsureUrlHasProtocol(toolStripTextBoxUrl.Text));
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
            if (curwebView2.CoreWebView2 == null) return;

            var res = double.TryParse(tstb_zoomFactor.Text, out zoomFactor);

            if (res)
            curwebView2.ZoomFactor = zoomFactor/100;
        }

        private async void tsb_add_tab_Click(object sender, EventArgs e)
        {
            await AddTabAsync();
        }

        private async Task AddTabAsync(string title = "")
        {
            string userDataFolder = Path.Combine(Path.GetTempPath(), $"{Environment.UserName}\\aw_web\\{UserProfile.Settings.ProfileName}");
            if (!Directory.Exists(userDataFolder)) { Directory.CreateDirectory(userDataFolder); }

            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-features=msSmartScreenProtection");

            var task = await CoreWebView2Environment.CreateAsync(
               browserExecutableFolder: null,
                userDataFolder: Path.Combine(userDataFolder),
                options);
            

            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(
               browserExecutableFolder: null,
                userDataFolder: Path.Combine(userDataFolder),
                options);

            curwebView2 = new WebView2();
            curwebView2.ZoomFactor = zoomFactor / 100;
             await curwebView2.EnsureCoreWebView2Async(environment);
            
            if (_curJob != null)
            {
                curwebView2.CoreWebView2.Profile.DefaultDownloadFolderPath = _curJobDir;
            }

            TabPage tabPage = new TabPage(title);
            tabPage.Tag = curwebView2;
            curwebView2.Dock = DockStyle.Fill;
            tabPage.Controls.Add(curwebView2);
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;

            if (tabControl1.SelectedTab.Tag is WebView2 webView2)
            {
                curwebView2 = webView2;
                if (_curJob != null)
                {
                    curwebView2.CoreWebView2.Profile.DefaultDownloadFolderPath = _curJobDir;
                }
                
            }
        }

        private void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Check if the user double-clicked on a tab
                for (int i = 0; i < tabControl1.TabCount; i++)
                {
                    Rectangle tabRect = tabControl1.GetTabRect(i);
                    if (tabRect.Contains(e.Location))
                    {
                        var tabPage = tabControl1.TabPages[i];
                        if (tabPage.Tag is WebView2 webView2)
                        {
                            // Dispose the WebView2 control
                            webView2.Dispose();
                        }
                        // Remove the tab
                        tabControl1.TabPages.RemoveAt(i);

                        break;
                    }
                }
            }
        }
    }
}
