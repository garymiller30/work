using System.Collections.Generic;
using System.Windows.Forms;

namespace Interfaces
{
    public interface IJobStatusManager
    {
        int GetDefaultStatus();
        int GetIndexImageByStatusCode(int statusCode);
        IJobStatus[] GetJobStatuses();
        IStatusesParams OnChangeStatusesParams { get; set; }
        object GetJobStatusDescriptionByCode(int statusCode);
        //void Load();
        ImageList GetImageList();
        IJobStatus GetJobStatusByCode(int statusCode);
        void SetImage(IJobStatus jobStatus, string fileName);
        IJobStatus Create();
        void Add(IJobStatus jobStatus);
        void Remove(IJobStatus jobStatus);
        void Refresh(IJobStatus jobStatus);
        void SetDefaultStatus(IJobStatus status);
        //Dictionary<int, bool> GetViewStatuses();
        void SaveViewStatuses(Dictionary<int, bool> dic);
        void SaveViewStatuses();

        bool IsViewStatusChecked(int statusCode);
        void ViewStatusChecked(IJobStatus status, bool check);

        int[] GetEnabledViewStatuses();

        bool IsVisible(int jobStatusCode);
    }
}
