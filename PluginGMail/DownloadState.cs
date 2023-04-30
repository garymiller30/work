using Interfaces;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginGMail
{
    internal class DownloadState
    {
        public IJob Job { get; set; }
        public CoreWebView2DownloadOperation DownloadOperation { get; set; }
    }
}

