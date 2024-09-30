using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionMethods;
using FtpClient;
using FtpClient.Controllers;
using Interfaces;
using Interfaces.Ftp;
using JobSpace;
using JobSpace.UC;

namespace ActiveWorks
{
    public sealed class FtpManager : IFtpManager
    {
        //public Profile UserProfile { get; set; }
        private readonly IUserProfile _profile;

        public IFtpScriptController FtpScriptController { get; set; }
        public IUcFtpBrowser FtpExplorer { get; set; }

        const string CollectionString = "Ftp";

        List<FtpSettings> _ftpSettingses = new List<FtpSettings>();

        public FtpManager(IUserProfile profile)
        {
            _profile = profile;
            Load();
        }

        private void Load()
        {
            var result = _profile.Base.GetCollection<FtpSettings>(CollectionString);
            if (result != null)
            {
                _ftpSettingses = result;
            }

            FtpScriptController = new FtpScriptController();
            FtpScriptController.Load(_profile.ProfilePath);
        }

        public void InitUcFtpBrowserControls()
        {
            FtpExplorer = new UcFtpBrowser();
            FtpExplorer.UserProfile = _profile;
            ((Control)FtpExplorer).Dock = DockStyle.Fill;
            FtpExplorer.Init();
            FtpExplorer.FtpStates.IsEventAnyChanges = true;
        }

        public void Add(IFtpSettings fs)
        {

            if (_profile.Base.Add(CollectionString, (FtpSettings)fs))
            {
                _ftpSettingses.Add((FtpSettings)fs);
            }
        }

        public void Remove(IFtpSettings fs)
        {
            if (_profile.Base.Remove(CollectionString, (FtpSettings)fs))
            {
                _ftpSettingses.Remove((FtpSettings)fs);
            }
        }

        public void Update(IFtpSettings fs)
        {
            _profile.Base.Update(CollectionString, (FtpSettings)fs);
        }

        public void CreateEvents()
        {
            _profile.Events.Ftp.OnAddFilesToOrder += FtpEventsOnAddFilesToOrder;
            _profile.Events.Ftp.OnCreateOrder += FtpEventsOnCreateOrder;
            _profile.Events.Ftp.OnCreateOrderFromDir += FtpEventsOnCreateOrderFromDir;
            _profile.Events.Ftp.OnCreateOrderFromDirLikeDescription += FtpEventsOnCreateOrderFromDirLikeDescription;

            _profile.Events.Ftp.Init(_profile);
        }


        private void FtpEventsOnCreateOrderFromDirLikeDescription(object sender, IDownloadTicket downloadProperties)
        {
            CreateJobFromFtpDirAsDescription(downloadProperties);//(ICustomer) customer, tuple
        }

        private void FtpEventsOnCreateOrderFromDir(object sender, IDownloadTicket downloadProperties)
        {
            CreateJobFromFtpDir(downloadProperties);
        }

        private void FtpEventsOnCreateOrder(object sender, IDownloadTicket downloadProperties)
        {
            CreateJobFromFtpFile(downloadProperties);
        }

        private void FtpEventsOnAddFilesToOrder(object sender, IDownloadTicket downloadProperties)
        {
            DownloadFilesFromFtpToSelectedJob(downloadProperties);
        }

        private void DownloadFilesFromFtpToSelectedJob(IDownloadTicket downloadProperties)
        {
            if (_profile.Jobs.CurrentJob is IJob job)
            {
                downloadProperties.Job = job;

                StartDownloadFilesFromFtp(downloadProperties);
            }
        }


        private void CreateJobFromFtpDirAsDescription(IDownloadTicket downloadProperties)//ICustomer customer, Tuple<string, IDownloadFileParam> tuple
        {
            var job = Factory.CreateJob(_profile);
            job.Number = downloadProperties.DownloadFileParam.OrderNumber ?? job.Number;

            job.Customer = downloadProperties.Customer.Name;
            job.Description = downloadProperties.FtpDir;
            _profile.Plugins.AfterJobChange(job);
            if (_profile.Jobs.AddJob(job))
            {
                downloadProperties.Job = job;
                StartDownloadFilesFromFtp(downloadProperties);
            }
        }
        private void CreateJobFromFtpDir(IDownloadTicket downloadProperties)//ICustomer customer, Tuple<string, IDownloadFileParam> tuple
        {
            var job = Factory.CreateJob(_profile);

            job.Customer = downloadProperties.Customer.Name;
            job.Number = downloadProperties.FtpDir;
            job.Description = Path.GetFileNameWithoutExtension(downloadProperties.DownloadFileParam.File[0].Name);
            _profile.Plugins.AfterJobChange(job);
            if (_profile.Jobs.AddJob(job))
            {
                downloadProperties.Job = job;
                StartDownloadFilesFromFtp(downloadProperties);
            }
        }

        private void CreateJobFromFtpFile(IDownloadTicket downloadProperties)
        {
            var job = Factory.CreateJob(_profile);

            job.Customer = downloadProperties.Customer.Name;
            job.Number = downloadProperties.DownloadFileParam.OrderNumber ?? job.Number;
            _profile.Plugins.AfterJobChange(job);
            //job.Number = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
            job.Description = Path.GetFileNameWithoutExtension(downloadProperties.DownloadFileParam.File[0].Name);


            if (_profile.Jobs.AddJob(job))
            {

                downloadProperties.Job = job;

                StartDownloadFilesFromFtp(downloadProperties);
            }
        }

        private void StartDownloadFilesFromFtp(IDownloadTicket ticket)//IJob job, IDownloadFileParam list
        {
            // есть файлы на закачку
            if (ticket.DownloadFileParam.File.Count > 0)
            {
                var downloader = new Downloader();

                ticket.TargetDir = _profile.Jobs.GetFullPathToWorkFolder(ticket.Job);


                downloader.AddFile(ticket);
                downloader.StartDownload += Downloader_StartDownload;
                downloader.FinishDownload += Downloader_FinishDownload;
                downloader.ProcessDownloading += Downloader_ProcessDownloading;

                _profile.Jobs.LockJob(ticket.Job);


                DownloadService.AddToQuery(downloader);
            }
        }

        private void Downloader_ProcessDownloading(object sender, IDownloadTicket e)
        {

            if (e.Job != null)
            {
                var newValue = (int)e.currentProgress;
                if (e.Job.ProgressValue != newValue)
                {
                    e.Job.ProgressValue = newValue;
                    _profile.Jobs.UpdateJob(e.Job);

                }
            }
        }
        private void Downloader_StartDownload(object sender, IDownloadTicket e)
        {
            if (e.Job != null)
            {
                e.Job.IsJobInProcess = true;
                e.Job.ProgressValue = 0;

                _profile.Jobs.UpdateJob(e.Job);
            }
        }
        private void Downloader_FinishDownload(object sender, IDownloadTicket e)
        {
            if (e.Job != null)
            {
                e.Job.IsJobInProcess = false;
                e.Job.ProgressValue = 0;
                _profile.Jobs.UpdateJob(e.Job);

                try
                {
                    if (e.OnDownloaded != null)
                    {
                        e.OnDownloaded(e);
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, ee.Source);
                }

                _profile.Jobs.UnlockJob(e.Job);
            }
        }


        public ICollection<IFtpSettings> GetCollection()
        {
            return _ftpSettingses.Cast<IFtpSettings>().ToList();
        }

        public IFtpSettings GetParamByName(string server)
        {
            return _ftpSettingses.FirstOrDefault(x => x.Name.Equals(server));
        }

        public IFtpSettings this[int index]
        {
            set { _ftpSettingses[index] = (FtpSettings)value; }
            get { return _ftpSettingses[index]; }
        }
    }
}
