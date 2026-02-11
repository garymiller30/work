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
    
        void SubscribeEvents();

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

        EventHandler<IJob> OnSetCurrentJob { get; set; }
        EventHandler<IJob> OnJobAdd { get; set; }
        EventHandler<ICollection> OnJobsAdd { get;set;}
        EventHandler<IJob> OnJobChange { get; set; }
        EventHandler<IJob> OnJobBeginEdit { get; set; }
        EventHandler<IJob> OnJobFinishEdit { get; set; }
        EventHandler<IJob> OnDeleteJob { get; set; }

        void SetCurrentJob(IJob job);
        void CreateJob();
        void RepeatSelectedJob();
        void CreateJob(IPluginNewOrder pluginNewOrder);
        void DeleteJob(IJob job);
        void ApplyViewListFilterCustomer(string text);
        void ApplyViewListFilterStatuses(int[] statuses);
        void ApplyViewListFilterDate(DateTime date);
        void ApplyViewListFilterText(string text);
    }
}
