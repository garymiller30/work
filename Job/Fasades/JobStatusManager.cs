using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interfaces;
using JobSpace.Static;
using JobSpace.Statuses;
using Logger;


namespace JobSpace.Fasades
{
    public sealed class JobStatusManager : IJobStatusManager
    {
        private readonly IUserProfile _profile;
        private const string ViewFilterFile = "ViewFilterStatuses.set";
        const string CollectionString = "JobStatuses";

        private ImageList _imageList = new ImageList();
        private List<JobStatus> _statuses = new List<JobStatus>();

        public IStatusesParams OnChangeStatusesParams { get; set; } = new StatusesParams();
        private Dictionary<int, bool> _viewFilters;

        public JobStatusManager(IUserProfile profile)
        {
            _profile = profile;
            OnChangeStatusesParams.UserProfile = profile;
            Load();
        } 

        public void Load()
        {
            var result = _profile.Base.GetCollection<JobStatus>(CollectionString);
            if (result != null)
            {
                _statuses.AddRange(result);
                LoadImages();
                OnChangeStatusesParams.Load();
                LoadViewFilters();
            }


        }

        private void LoadViewFilters()
        {
            _viewFilters = GetViewStatuses();

        }

        public ImageList GetImageList()
        {
            return _imageList;
        }

        private void LoadImages()
        {
            foreach (var statuse in _statuses)
            {
                LoadImage(statuse);
                if (statuse.Img == null) statuse.Img = new Bitmap(16,16);
                _imageList.Images.Add(statuse.Img);
            }
        }

        public IJobStatus GetJobStatusByCode(int statusCode)
        {
            return _statuses.FirstOrDefault(x => x.Code == statusCode);
        }

        private void LoadImage(JobStatus status)
        {
            if (!string.IsNullOrEmpty(status.ImgBase64))
            {
                try
                {
                    status.Img = Image.FromStream(new MemoryStream(Convert.FromBase64String(status.ImgBase64)));
                }
                catch (Exception e)
                {
                    Log.Error(_profile,"JobStatusManager",e.Message);
                }

            }
        }

        public Image GetImageByStatusCode(int statusCode)
        {
            return _statuses.FirstOrDefault(x => x.Code == statusCode)?.Img;
        }

        /// <summary>
        /// створити новий статус
        /// </summary>
        /// <returns></returns>
        public IJobStatus Create()
        {
            var status = new JobStatus {Code = CreateCode()};
            if (!_statuses.Any()) status.IsDefault = true;
            return status;
        }

        private int CreateCode()
        {
            return _statuses.Any() ? _statuses.Max(x => x.Code) + 1 : 1;
        }
        /// <summary>
        /// додати статус до бази
        /// </summary>
        /// <param name="jobStatus"></param>
        public void Add(IJobStatus jobStatus)
        {
            if (_profile.Base.Add(CollectionString,(JobStatus) jobStatus))
            {
                _statuses.Add((JobStatus)jobStatus);
                _imageList.Images.Add(jobStatus.Img);
            }
            else
            {
                Log.Error(_profile,"JobStatusManager","Can't add status to base");
            }
        }
        /// <summary>
        /// видалити статус з бази
        /// </summary>
        /// <param name="jobStatus"></param>
        public void Remove(IJobStatus jobStatus)
        {
            if (_profile.Base.Remove(CollectionString,(JobStatus) jobStatus))
            {
                jobStatus.Img?.Dispose();
                
                _statuses.Remove((JobStatus)jobStatus);

                CreateImageListFromStatusesList();

            }
            else
            {
                Log.Error(_profile,"JobStatusManager","Can't remove status from base");
            }
        }

        private void CreateImageListFromStatusesList()
        {
            _imageList = new ImageList();

            foreach (var statuse in _statuses)
            {
                if (statuse.Img != null)
                    _imageList.Images.Add(statuse.Img);
            }
        }

        public int GetIndexImageByStatusCode(int statusCode)
        {
            var img = _statuses.FirstOrDefault(x => x.Code == statusCode);
            if (img != null)
            {
                return _statuses.IndexOf(img);
            }

            return -1;
        }

        /// <summary>
        /// оновити статус у базі
        /// </summary>
        /// <param name="jobStatus"></param>
        public void Refresh(IJobStatus jobStatus)
        {
            if (!_statuses.Contains((JobStatus)jobStatus))
            {
                Add(jobStatus);
            }
            else 
            {
                try
                {
                    _profile.Base.Update(CollectionString, (JobStatus)jobStatus);
                }
                catch
                {
                    Log.Error(_profile,"JobStatusManager","Can't update status to base");
                }
            }
         
        }

        public void SetImage(IJobStatus jobStatus, string fileName)
        {
            jobStatus.Img?.Dispose();
            try
            {
                jobStatus.Img = Image.FromFile(fileName);

                using (var m = new MemoryStream())
                {
                    jobStatus.Img.Save(m, jobStatus.Img.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    // Convert byte[] to Base64 String
                    jobStatus.ImgBase64 = Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception e)
            {
                Log.Error(_profile,"JobStatusManager",e.Message);   
            }
        }


        public IJobStatus[] GetJobStatuses() => _statuses.Cast<IJobStatus>().ToArray();

        public void SetJobStatuses(IEnumerable<IJobStatus> statuses)
        {
            _statuses = statuses.Cast<JobStatus>().ToList();

            _profile.Base.RemoveAll<JobStatus>(CollectionString);

            foreach (var status in _statuses)
            {
                _profile.Base.Add(CollectionString, (JobStatus)status);
            }

        }

        public void SetDefaultStatus(IJobStatus status)
        {
            _statuses.ForEach(x => x.IsDefault = false);
            status.IsDefault = true;
        }

        private Dictionary<int, bool> GetViewStatuses()
        {
            Dictionary<int, bool> dic;
            var path = Path.Combine(_profile.ProfilePath, ViewFilterFile);
            if (File.Exists(path))
            {
                dic = SaveAndLoad.Load<Dictionary<int, bool>>(path);
            }
            else
            {
                dic = new Dictionary<int, bool>();
            }

            return dic;
        }

        public void SaveViewStatuses(Dictionary<int, bool> dic)
        {
            SaveAndLoad.Save(Path.Combine(_profile.ProfilePath, ViewFilterFile), dic);
        }

        public void SaveViewStatuses()
        {
            SaveAndLoad.Save(Path.Combine(_profile.ProfilePath, ViewFilterFile), _viewFilters);
        }

        public bool IsViewStatusChecked(int statusCode)
        {
            if (_viewFilters.ContainsKey(statusCode))
            {
                return _viewFilters[statusCode];
            }
            

            return false;
        }

        public void ViewStatusChecked(IJobStatus status, bool check)
        {
            if (_viewFilters.ContainsKey(status.Code))
            {
                _viewFilters[status.Code] = check;
            }
            else
            {
                _viewFilters.Add(status.Code,check);
            }
        }

        public int[] GetEnabledViewStatuses()
        {
            return _viewFilters.Where(o => o.Value).Select(x=>x.Key).ToArray();
        }

        public bool IsVisible(int jobStatusCode)
        {
            var visibleStatuses = GetEnabledViewStatuses();
            return visibleStatuses.Contains(jobStatusCode);
        }

        public object GetJobStatusDescriptionByCode(int statusCode)
        {
            return _statuses.FirstOrDefault(x => x.Code == statusCode)?.Name;
        }

        public int GetDefaultStatus()
        {
            var status = _statuses.FirstOrDefault(x => x.IsDefault);
            if (status == null) return 0;
            return status.Code;
        }

 
    }
}
