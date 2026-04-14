using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Ftp
{
    public interface IDownloadTicket
    {
        object Sender {get;set;}
        ICustomer Customer {get;set;}
        IJob Job {get;set;}
        IDownloadFileParam DownloadFileParam {get;set;}
        string FtpDir {get;set;}
        string TargetDir { get; set; }
        dynamic OnDownloaded { get; set; }

        double currentProgress {get;set;}
    }
}
