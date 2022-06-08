using System;

namespace Interfaces
{
    public interface IUcJobList
    {
        void CreateEvents();
        void ApplyViewFilter();
        void Search(string text);
        string GetSelectedJobPath();
        //IJob GetSelectedJob();
        void ChangeSelectedJobsStatus(IJobStatus status);
        void CreateJobFromFile(string e);
        IJob ChangeSelectedJobDescription(string e);
        void Close();
        //void CreateJobFromFtpDirAsDescription(ICustomer customer, Tuple<string, IDownloadFileParam> tuple);
        //void CreateJobFromFtpDir(ICustomer customer, Tuple<string, IDownloadFileParam> tuple);
        //void CreateJobFromFtpFile(ICustomer customer, IDownloadFileParam downloadFileParams);
        //void DownloadFilesFromFtpToSelectedJob(IDownloadFileParam downloadFileParams);
        void RepeatSelectedJob();
        void ApplyViewFilter(DateTime date);
        void ApplyJobListFilter(string filterText);

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

    }
}
