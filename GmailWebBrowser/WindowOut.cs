using System;
using System.Windows.Forms;
using Interfaces;

namespace GmailWebBrowser
{
    public partial class WindowOut : UserControl,IPluginInfo
    {
        //private WindowOut _windowOut;

#pragma warning disable CS0169 // The field 'WindowOut._getCurJob' is never used
        private Func<object> _getCurJob;
#pragma warning restore CS0169 // The field 'WindowOut._getCurJob' is never used
        private Func<object> _getCurJobPath;

        public WindowOut()
        {
            InitializeComponent();

            //this.SizeChanged += Resize;

            webView21.CoreWebView2InitializationCompleted+= WebView21OnCoreWebView2Ready;



        }

        private void WebView21OnCoreWebView2Ready(object sender, EventArgs e)
        {
            webView21?.CoreWebView2?.Navigate("https://gmail.com");
        }


        public void SetUserProfile(IUserProfile profile)
        {
            UserProfile = profile;
        }

        public IUserProfile UserProfile { get; set; }

        public UserControl GetUserControl()
        {
            return this;// _windowOut ?? (_windowOut = new WindowOut());
        }

        public void Start()
        {
            
        }

        public string GetPluginName()
        {
            return "Почта";
        }

        public void SetCurJobCallBack(object curJob)
        {
            //throw new NotImplementedException();
        }

        public void SetCurJobPathCallBack(object curJobPath)
        {
            //var obj = (Func<object>)curJobPath;
            //((WindowOut)GetUserControl())._getCurJobPath = obj;
        }

        public void SetCurJob(IJob curJob)
        {
            //throw new NotImplementedException();
        }

        public void BeforeJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }

        public void AfterJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }





        private void ToolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webView21?.CoreWebView2?.Navigate(toolStripTextBox1.Text);

                e.Handled = true;
            }
        }

        public string PluginName { get; } = "Пошта";
        public string PluginDescription { get; } = "WebView2";
        public void ShowSettingsDlg()
        {
            throw new NotImplementedException();
        }
    }
}
