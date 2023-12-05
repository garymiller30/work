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

namespace PluginFileshareWeb
{
    public partial class WindowOut : UserControl, IPluginInfo
    {
        public IUserProfile UserProfile { get; set; }

        public string PluginName => "Файлообмінники";

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
        }

        public string GetPluginName()
        {
            return PluginName;
        }

        public void SetCurJob(IJob curJob)
        {
            
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
    }
}
