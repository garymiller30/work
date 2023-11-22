// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Interfaces.Ftp;

//using EnterpriseDT.Net.Ftp;

namespace FtpClient
{

    /// <summary>
    /// завантаження файлів з ftp
    /// </summary>
    public sealed class Downloader
    {
        private IDownloadTicket _ticket;
        readonly Client _client;

        public event EventHandler<IDownloadTicket> StartDownload = delegate {  };
        public event EventHandler<string> StartDownloadFile = delegate { };
        public event EventHandler<IDownloadTicket> ProcessDownloading = delegate { };
        public event EventHandler<IDownloadTicket> FinishDownload = delegate { };

        //public object Tag;
        
        public Downloader()
        {
            _client = new Client();
        }

        /// <summary>
        /// додати файли на завантаження
        /// </summary>
        /// <param name="files"></param>
        /// <param name="targetDirectory"></param>
        public void AddFile(IDownloadTicket ticket)
        {
            _ticket = ticket;
            
        }

        public void Download()
        {
            StartDownload(this, _ticket);
            var _files = _ticket.DownloadFileParam;

            _client.CreateConnection(_files.Server, _files.User, _files.Password, _files.RootDirectory, _files.ActiveMode, _files.CodePage);

            var files = _files.File.Select(x => x.FullPath).ToArray();
            _client.OnDownloadingProcess += ClientOnOnDownloadingProcess;
            _client.DownloadFiles(files, _ticket.TargetDir);
            _client.OnDownloadingProcess -= ClientOnOnDownloadingProcess;
            
            FinishDownload(this,_ticket);
            _files = null;
        }

        private void ClientOnOnDownloadingProcess(object sender, double e)
        {
            _ticket.currentProgress = e;
            ProcessDownloading(this, _ticket);
        }

        

    }
}
