using Interfaces.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IJobManager
    {
        IJob CurrentJob { get; set; }
        IJobSettings Settings { get; set; }
        IUcJobList JobListControl { get; set; }
    
        void Connect(bool reconnect);
        string GetFullPathToWorkFolder(IJob job);
        void UpdateJob(IJob job, bool getEvent=false);
        bool AddJob(IJob job);
        //void LoadJobs();
        void ChangeStatusCode(string orderNumber, int statusCode);
        bool ChangeJobDescription(IJob job, string s1);
        bool ChangeJobNumber(IJob job, string number);
        bool RenameJobDirectory(string oldDir, IJob job);
        void Dublicate(IJob job);
        void CreateOrOpenSignaJob(string signaPath, IJob job);
        void LockJob(IJob job);
        void UnlockJob(IJob job);
        void DownloadFromHttp(string link, IJob job);
        void CombineOrdersInOne(IList selectedObjects);
        IEnumerable<IJob> GetJobs();

        event EventHandler<IJob> OnSetCurrentJob;
        event EventHandler<IJob> OnJobAdd;
        event EventHandler<ICollection> OnJobsAdd;
        event EventHandler<IJob> OnJobChange;
        event EventHandler<IJob> OnJobBeginEdit;
        event EventHandler<IJob> OnJobFinishEdit;
        event EventHandler<IJob> OnDeleteJob;

        void SetCurrentJob(IJob job);
        void CreateJob();
        void RepeatSelectedJob();
        void CreateJob(IPluginNewOrder pluginNewOrder);


        void ApplyStatusViewFilter();
        void Search(string text);
        void ApplyDateFilter(DateTime date);
        void DeleteJob(IJob job);
    }
}
