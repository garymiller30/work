using System;
using System.Collections;

namespace Interfaces
{
    public interface IUcJobList
    {
        void CreateEvents();
        //void ApplyViewFilter();
        //void Search(string text);
        string GetSelectedJobPath();
        void ChangeSelectedJobsStatus(IJobStatus status);
        void CreateJobFromFile(string e);
        IJob ChangeSelectedJobDescription(string e);
        void Close();
        void RepeatSelectedJob();
        void SelectJob(IJob job);
        EventHandler<int> OnChangeCountJobs { get; set; }
        /// <summary>
        /// заблокувати роботу
        /// </summary>
        /// <param name="job"></param>
        void LockJob(IJob job);
        /// <summary>
        /// розблокувати роботу
        /// </summary>
        /// <param name="job"></param>
        void UnlockJob(IJob job);
        void ApplyViewListFilterCustomer(string text);
        void ApplyViewListFilterStatuses(int[] statuses);
        void ApplyViewListFilterDate(DateTime date);
        void ApplyViewListFilterText(string text);
        void ApplyJobListFontSettings();
        IEnumerable GetJobList();
    }
}
