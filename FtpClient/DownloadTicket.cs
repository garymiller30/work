using Interfaces;
using Interfaces.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient
{
    public sealed class DownloadTicket : IDownloadTicket
    {
        public object Sender{get;set; }
        public ICustomer Customer {get;set;}
        public IDownloadFileParam DownloadFileParam {get;set; }
        public string FtpDir { get; set; }
        public IJob Job {get;set; }
        public string TargetDir {get;set; }
        public dynamic OnDownloaded {get;set; }
        public double currentProgress {get;set; }
    }
}
