using Interfaces;
using Interfaces.Profile;
using Krypton.Navigator;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        List<ToolStripButton> toolStripButtons = new List<ToolStripButton>();
        KryptonNavigator _tab_control;
        double zoomFactor = 80;
        public IUserProfile UserProfile { get; set; }

        public string PluginName => "Швидкі посилання";

        public string PluginDescription => "Популярні файлообмінники v2.0";

        public WindowOut()
        {
            InitializeComponent();
            ApplyModernChrome();
            _tab_control = kryptonNavigator1;
            _tab_control.SelectedPageChanged += tControl_SelectedIndexChanged;
            //_tab_control.MouseDoubleClick += tControl_MouseDoubleClick;
            _ = InitializeAsync();
        }

        private void ApplyModernChrome()
        {
            BackColor = Color.FromArgb(248, 250, 252);
            toolStripContainer1.BackColor = BackColor;
            toolStripContainer1.TopToolStripPanel.BackColor = Color.FromArgb(245, 247, 250);
            toolStripContainer1.ContentPanel.BackColor = BackColor;

            toolStrip1.RenderMode = ToolStripRenderMode.Professional;
            toolStrip1.Renderer = new FileshareToolStripRenderer();

            StyleCommandButton(tsb_add_tab, Color.FromArgb(37, 99, 235), Color.White);
            StyleCommandButton(tsb_go, Color.FromArgb(22, 163, 74), Color.White);
            StyleCommandButton(tsb_zoomOk, Color.FromArgb(226, 232, 240), Color.FromArgb(15, 23, 42));
            StyleCommandButton(toolStripButton_Add, Color.FromArgb(226, 232, 240), Color.FromArgb(15, 23, 42));

            toolStripTextBoxUrl.BorderStyle = BorderStyle.FixedSingle;
            tstb_zoomFactor.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleCommandButton(ToolStripButton button, Color backColor, Color foreColor)
        {
            button.AutoSize = false;
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            button.TextAlign = ContentAlignment.MiddleCenter;
        }

        //private void tControl_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        // Check if the user double-clicked on a tab
        //        for (int i = 0; i < _tab_control.TabCount; i++)
        //        {
        //            Rectangle tabRect = _tab_control.GetTabRect(i);
        //            if (tabRect.Contains(e.Location))
        //            {
        //                var tabPage = _tab_control.TabPages[i];
        //                if (tabPage.Tag is WebView2 webView2)
        //                {
        //                    LinkInfo link = (LinkInfo)webView2.Tag;
        //                    _settings.OpenOnStart.Remove(link);
        //                    UserProfile.Plugins.SaveSettings(_settings);
        //                    // Dispose the WebView2 control
        //                    webView2.Dispose();
        //                }
        //                // Remove the tab
        //                _tab_control.TabPages.RemoveAt(i);

        //                break;
        //            }
        //        }
        //    }
        //}

        private void tControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tab_control.SelectedPage == null) return;

            if (_tab_control.SelectedPage.Tag is WebView2 webView2)
            {
                curwebView2 = webView2;
                if (_curJob != null)
                {
                    curwebView2.CoreWebView2.Profile.DefaultDownloadFolderPath = _curJobDir;
                }

            }
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
            LinkInfo link = (LinkInfo)((ToolStripButton)sender).Tag;

            await AddTabAsync();
            WebView2 webView = (WebView2)_tab_control.SelectedPage.Tag;
            //збережемо посилання в тег вкладки
            webView.Tag = link;
            _tab_control.SelectedPage.Text = link.Name;



            curwebView2.Source = new Uri("about:blank");
            curwebView2.Source = new Uri(link.Url);
            curwebView2.ZoomFactorChanged += (s, ev) =>
            {
                link.ZoomFactor = curwebView2.ZoomFactor;
                tstb_zoomFactor.Text = (link.ZoomFactor * 100).ToString("N0");
                UserProfile.Plugins.SaveSettings(_settings);
            };
            _settings.OpenOnStart.Add(link);
            UserProfile.Plugins.SaveSettings(_settings);
        }


        public UserControl GetUserControl()
        {
            return this;
        }

        public  void Start()
        {
            _settings = UserProfile.Plugins.LoadSettings<FileShareWebSettings>();
            _settings.Normalize();

            AddingLinksToToolStrip();
            OpenSavedTabs();
        }

        private async void OpenSavedTabs()
        {
            foreach (var link in _settings.OpenOnStart)
            {
                await AddTabAsync();
                WebView2 webView = (WebView2)_tab_control.SelectedPage.Tag;
                //збережемо посилання в тег вкладки
                webView.Tag = link;
                _tab_control.SelectedPage.Text = link.Name;

                curwebView2.Source = new Uri("about:blank");
                curwebView2.Source = new Uri(link.Url);
                curwebView2.ZoomFactor = link.ZoomFactor;
                curwebView2.ZoomFactorChanged += (s, ev) =>
                {
                    link.ZoomFactor = curwebView2.ZoomFactor;
                    tstb_zoomFactor.Text = (link.ZoomFactor * 100).ToString("N0");
                    UserProfile.Plugins.SaveSettings(_settings);
                };
            }
        }

        private void AddingLinksToToolStrip()
        {
            foreach (var btn in toolStripButtons)
            {
                toolStrip1.Items.Remove(btn);
            }

            foreach (var link in _settings.Links)
            {
                var button = CreateButton(link);
                toolStripButtons.Add(button);
                toolStrip1.Items.Add(button);
            }
        }

        private ToolStripButton CreateButton(LinkInfo link)
        {
            var button = new ToolStripButton(link.Name);
            button.Tag = link;
            button.ToolTipText = link.Url;
            button.Margin = new Padding(6, 0, 0, 0);
            button.Padding = new Padding(8, 0, 8, 0);
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
            using (var form = new FormSettings(_settings))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _settings = form.Settings;
                    UserProfile.Plugins.SaveSettings(_settings);
                    AddingLinksToToolStrip();
                }
            }
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

        private async void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(toolStripTextBoxUrl.Text))
            {
                try
                {
                    if (curwebView2 == null) { await AddTabAsync(); }

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
            curwebView2 = await CreateWebViewTabAsync(title);
        }

        private async Task<WebView2> CreateWebViewTabAsync(string title = "")
        {
            string userDataFolder = Path.Combine(Path.GetTempPath(), $"{Environment.UserName}\\aw_web\\{UserProfile.Settings.ProfileName}");
            if (!Directory.Exists(userDataFolder)) { Directory.CreateDirectory(userDataFolder); }

            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-features=msSmartScreenProtection");

            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(
               browserExecutableFolder: null,
                userDataFolder: Path.Combine(userDataFolder),
                options);

            var webView = new WebView2();
            webView.ZoomFactor = zoomFactor / 100;
            await webView.EnsureCoreWebView2Async(environment);

            webView.CoreWebView2.ServerCertificateErrorDetected += (s, e) =>
            {
                e.Action = CoreWebView2ServerCertificateErrorAction.AlwaysAllow;
            };
            webView.CoreWebView2.DocumentTitleChanged += (s, e) =>
            {
                if (webView.CoreWebView2 != null)
                {
                    var tab = webView.Parent as KryptonPage;
                    if (tab != null)
                    {
                        tab.Text = webView.CoreWebView2.DocumentTitle;
                    }
                }
            };
            webView.CoreWebView2.NewWindowRequested += WebView_NewWindowRequested;
            webView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;

            if (_curJob != null)
            {
                webView.CoreWebView2.Profile.DefaultDownloadFolderPath = _curJobDir;
            }

            KryptonPage tabPage = new KryptonPage(title);
            tabPage.Tag = webView;
            webView.Dock = DockStyle.Fill;
            tabPage.Controls.Add(webView);
            _tab_control.Pages.Add(tabPage);
            _tab_control.SelectedPage = tabPage;

            return webView;
        }

        private async void WebView_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();

            try
            {
                var newWebView = await CreateWebViewTabAsync("Нова вкладка");
                e.NewWindow = newWebView.CoreWebView2;
                e.Handled = true;
            }
            finally
            {
                deferral.Complete();
            }
        }
       
        private void kryptonNavigator1_CloseAction(object sender, CloseActionEventArgs e)
        {
            if (e.Action == CloseButtonAction.RemovePageAndDispose)
            {
                if (e.Item.Tag is WebView2 webView2)
                {
                    LinkInfo link = (LinkInfo)webView2.Tag;
                    _settings.OpenOnStart.Remove(link);
                    UserProfile.Plugins.SaveSettings(_settings);
                    // Dispose the WebView2 control
                    webView2.Dispose();
                }
            }
        }

        private sealed class FileshareToolStripRenderer : ToolStripProfessionalRenderer
        {
            public FileshareToolStripRenderer()
                : base(new FileshareColorTable())
            {
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                {
                    e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
                }
            }

            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
            {
                var button = e.Item as ToolStripButton;
                if (button == null)
                {
                    base.OnRenderButtonBackground(e);
                    return;
                }

                var bounds = new Rectangle(Point.Empty, e.Item.Size);
                bounds.Inflate(-1, -3);
                var fill = button.BackColor;

                if (button.Pressed)
                {
                    fill = ControlPaint.Dark(fill, 0.08f);
                }
                else if (button.Selected)
                {
                    fill = ControlPaint.Light(fill, 0.08f);
                }

                using (var path = RoundedRectangle(bounds, 6))
                using (var brush = new SolidBrush(fill))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
            }

            private static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
            {
                int diameter = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        private sealed class FileshareColorTable : ProfessionalColorTable
        {
            public override Color ToolStripGradientBegin => Color.FromArgb(245, 247, 250);
            public override Color ToolStripGradientMiddle => Color.FromArgb(245, 247, 250);
            public override Color ToolStripGradientEnd => Color.FromArgb(245, 247, 250);
            public override Color ButtonSelectedGradientBegin => Color.FromArgb(226, 232, 240);
            public override Color ButtonSelectedGradientMiddle => Color.FromArgb(226, 232, 240);
            public override Color ButtonSelectedGradientEnd => Color.FromArgb(226, 232, 240);
        }
    }
}
