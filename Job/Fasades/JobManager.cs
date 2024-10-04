// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionMethods;
using FtpClient;
using Interfaces;
using Interfaces.MQ;
using Interfaces.Plugins;
using JobSpace.Controllers;
using JobSpace.Ext;
using JobSpace.Models;
using JobSpace.Static;
using JobSpace.UC;
using JobSpace.UserForms;
using Krypton.Toolkit;
using Logger;
using Ookii.Dialogs.WinForms;


namespace JobSpace.Fasades
{
    public sealed class JobManager : IJobManager
    {
        public IJobSettings Settings { get; set; }
        public IUcJobList JobListControl { get; set; }
        public IJob CurrentJob { get; set; }

        private readonly IUserProfile _profile;

        const string CollectionString = "Jobs";

        public event EventHandler<IJob> OnJobAdd = delegate { };
        public event EventHandler<IJob> OnSetCurrentJob = delegate { };
        public event EventHandler<ICollection> OnJobsAdd = delegate { };

        public event EventHandler<IJob> OnJobChange = delegate { };
        public event EventHandler<IJob> OnJobBeginEdit = delegate { };
        public event EventHandler<IJob> OnJobFinishEdit = delegate { };
        public event EventHandler<IJob> OnDeleteJob = delegate { };

        public void SetCurrentJob(IJob job)
        {
            //if (CurrentJob == job) return;

            CurrentJob = job;
            OnSetCurrentJob(this, job);
        }

        public void CreateJob()
        {
            var job = Factory.CreateJob(_profile);

            var jobParameters = new JobParameters(job);

            using (var faw = new FormAddWork2(_profile, jobParameters, true))
            {
                faw.StartPosition = FormStartPosition.CenterParent;

                if (faw.ShowDialog() == DialogResult.OK)
                {
                    _profile.Plugins.AfterJobChange(jobParameters);
                    // unbind job
                    jobParameters.ApplyToJob();
                    _profile.Jobs.AddJob(job);
                }
                else
                {
                    // почистити плагіни
                    _profile.Plugins.RemoveProcessesByJobId(job.Id);
                }
            }
        }

        public void CreateJob(IPluginNewOrder pluginNewOrder)
        {
            pluginNewOrder.ShowDialogNewOrder(_profile, null);
        }

        public void ApplyStatusViewFilter()
        {
            _jobList = _profile.Base.ApplyViewFilter(_profile.StatusManager.GetEnabledViewStatuses());
            OnJobsAdd(this, _jobList);
        }

        public void Search(string text)
        {
            _jobList = _profile.Base.Search(text);
            OnJobsAdd(this, _jobList);
        }

        public void ApplyDateFilter(DateTime date)
        {
            _jobList = _profile.Base.SearchByDate(date);
            OnJobsAdd(this, _jobList);
        }

        public void RepeatSelectedJob()
        {
            _profile.Jobs.JobListControl.RepeatSelectedJob();
        }

        private List<IJob> _jobList = new List<IJob>();

        public JobManager(IUserProfile profile, IJobSettings settings)
        {
            Settings = settings;
            _profile = profile;

            JobListControl = new UcJobList(_profile) { Dock = DockStyle.Fill };
            Connect(false);
        }

        public string GetFullPathToWorkFolder(IJob job)
        {
            if (job.DontCreateFolder) return string.Empty;

            var customer = _profile.Customers.FindCustomer(job.Customer);
            if (customer == null)
            {
                Log.Error(_profile, "JobManager", $"Customer {job.Customer} not finded");
                return string.Empty;
            }

            if (job.UseCustomFolder)
            {
                return GetFolder(job);
            }

            var jobPath = _profile.Customers.GetCustomerWorkFolder(customer);//  Path.Combine(_profile.Jobs.Settings.WorkPath, customer.Name.Transliteration());

            if (Settings.StoreByYear)
            {
                jobPath = Path.Combine(jobPath, job.Date.Year.ToString());// CreateYearPath(jobPath, job.Date.Year);
            }

            jobPath = Path.Combine(jobPath, GetFolder(job));
            return jobPath;
        }

        private string GetFolder(IJob job)
        {
            if (job.UseCustomFolder && !string.IsNullOrEmpty(job.Folder))
            {
                return job.Folder;
            }
            return GetFolderName(job);
        }

        private string GetFolderName(IJob job)
        {
            var number = string.IsNullOrEmpty(job.Number) ? "XXXX" : job.Number;
            var category = _profile.Categories.GetCategoryNameById(job.CategoryId);
            if (string.IsNullOrEmpty(category))
            {
                return $"#{number}_{job.Customer.Transliteration()}_{job.Description.Transliteration()}";
            }

            return $"#{number}_{job.Customer.Transliteration()}_{category.Transliteration()}_{job.Description.Transliteration()}";
        }

        public void Connect(bool reconnect)
        {
            if (!reconnect)
                FollowToRabbit();
        }

        private void FollowToRabbit()
        {
            if (_profile.Plugins == null) return;
            _profile.Plugins.MqController.OnJobAdd += MQ_OnJobAdd;
            _profile.Plugins.MqController.OnJobBeginEdit += MQ_OnJobBeginEdit;
            _profile.Plugins.MqController.OnJobFinishEdit += MQ_OnJobFinishEdit;
            _profile.Plugins.MqController.OnJobChanged += MQ_OnJobChanged;
        }

        public void CreateOrOpenSignaJob(string signaJobsPath, IJob j)
        {
            if (!j.IsSignaJobExist(_profile) || Control.ModifierKeys == Keys.Shift)
                CreateSignaJobRev2(_profile, j);
            else
                OpenSignaJob(_profile, j);
        }

        void MQ_OnJobChanged(object sender, object id)
        {
            var ff = _jobList.FirstOrDefault(x => x.Id.ToString().Equals(id));

            if (ff != null)
            {
                var o = _profile.Base.GetById<Job>(CollectionString, id);
                ff.Update(o);
                OnJobChange(this, ff);
            }
        }

        void MQ_OnJobFinishEdit(object sender, object id)
        {
            var o = _profile.Base.GetById<Job>(CollectionString, id);
            if (o != null)
            {
                var ff = _jobList.FirstOrDefault(x => x.Id.Equals(o.Id));
                if (ff != null)
                    OnJobFinishEdit(this, ff);
            }
        }

        void MQ_OnJobBeginEdit(object sender, object id)
        {
            var o = _profile.Base.GetById<Job>(CollectionString, id);
            if (o != null)
            {
                var ff = _jobList.FirstOrDefault(x => x.Id.Equals(o.Id));
                if (ff != null)
                    OnJobBeginEdit(this, ff);
            }

        }

        void MQ_OnJobAdd(object sender, object id)
        {
            var o = _profile.Base.GetById<Job>(CollectionString, id);
            if (o != null)
            {
                _jobList.Add(o);
                OnJobAdd(this, o);
            }

        }

        public bool AddJob(IJob job)
        {
            var fulPath = GetFullPathToWorkFolder(job);
            var j = _jobList.Where(x => GetFullPathToWorkFolder(x).Equals(fulPath));

            if (j.Any())
            {
                var dialog = new TaskDialog
                {
                    WindowTitle = @"Увага!",
                    MainIcon = TaskDialogIcon.Custom,
                    //CustomMainIcon = Properties.Resources.emotion_misdoubt,
                    MainInstruction =
                    $"У {job.Customer} робота з номером {job.Number} вже існує. Все одно створити?"
                };
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.Yes));
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.No));

                if (dialog.ShowDialog().ButtonType == ButtonType.No)
                {
                    return false;
                }
            }

            CreateJobFolder(job);

            _jobList.Add((Job)job);

            MongoAddJob((Job)job);

            //Save();
            OnJobAdd(this, (Job)job); //event

            return true;
        }

        void CreateJobFolder(IJob job)
        {
            if (!job.DontCreateFolder)
            {

                var description = job.Description;
                var cnt = 1;
                string path = GetFullPathToWorkFolder(job);

                while (Directory.Exists(path))
                {
                    job.Description = $"{description}_{cnt}";
                    path = GetFullPathToWorkFolder(job);
                    cnt++;
                }
                try
                {
                    Directory.CreateDirectory(path);
                    Clipboard.SetText(path);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Виникла помилка при створенні папки");
                }
            }
        }

        void MongoAddJob(IJob job)
        {
            if (_profile.Base.Add(CollectionString, (Job)job.GetJob()))
            {
                _profile.Plugins.MqController.PublishChanges(MessageEnum.JobAdd, job.Id);
            }
        }

        void MongoDeleteJob(IJob job)
        {
            if (_profile.Base.Remove(CollectionString, (Job)job.GetJob()))
            {
                _profile.Plugins.MqController.PublishChanges(MessageEnum.JobDelete, job.Id);
            }
        }

        public void UpdateJob(IJob job, bool getEvent = false)
        {
            try
            {
                _profile.Base.Update(CollectionString, (Job)job.GetJob());
                _profile.Plugins.MqController.PublishChanges(MessageEnum.JobChanged, job.Id);
                if (getEvent) OnJobChange(this, (Job)job.GetJob());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "JobManager:UpdateJob", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(_profile, "JobManager:UpdateJob", e.Message);
            }

        }

        public IEnumerable<IJob> GetJobs()
        {
            return _jobList.ToArray();
        }

        /// <summary>
        /// получить день недели
        /// </summary>
        /// <param name="baseDate"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetWeekDate(DateTime baseDate, DayOfWeek dayOfWeek)
        {
            //var date = DateTime.Now;
            var day = (int)baseDate.DayOfWeek == 0 ? 7 : (int)baseDate.DayOfWeek;
            var dayOfW =
                dayOfWeek == 0 ? 7 : (int)dayOfWeek;

            return baseDate.AddDays(dayOfW - day);

        }

        /// <summary>
        /// дублювати завдання
        /// </summary>
        /// <param name="job"></param>
        public void Dublicate(IJob job)
        {
            if (job != null)
            {
                using (var form = new FormEditText())
                {
                    form.Text = "новий номер замовлення";
                    form.EditText = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";

                    if (form.ShowDialog() == DialogResult.OK)
                    {

                        var nj = job.Duplicate();

                        nj.PreviousOrder = nj.Number;
                        nj.Number = form.EditText;

                        if (nj.PreviousOrder.Equals(nj.Number))
                        {
                            nj.Description += "_(COPY)";
                        }

                        nj.Date = DateTime.Now;
                        // set default status
                        nj.StatusCode = _profile.StatusManager.GetDefaultStatus();

                        nj.UseCustomFolder = true;
                        nj.Folder = _profile.Jobs.GetFullPathToWorkFolder(job);

                        _profile.Plugins.AfterJobChange(nj);

                        MongoAddJob(nj);

                        DuplicateSignaJob(job, nj);

                        OnJobAdd(this, nj);
                    }
                }
            }
        }

        public void CreateSignaJobRev2(IUserProfile profile, IJob job)
        {
            var destFile = Extensions.GetSignaFilePath(job, profile);

            if (File.Exists(destFile))
            {
                if (MessageBox.Show("Файл з таким ім'ям існує. Замінити?", "Питання", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    File.Delete(destFile);
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destFile));

            File.Copy(@"SignaShablon\shablon2.sdf", destFile, true);

            var format = GetFormatFile(job);
            var customer = job.Customer.Transliteration();
            var description = job.Description.Transliteration();
            var category = _profile.Categories.GetCategoryNameById(job.CategoryId)?.Transliteration();

            var signaDesc = string.IsNullOrEmpty(category) ? description : $"{category}_{description}";

            new SignaController(destFile).SetJobNumber(job.Number)
                .SetCustomer(customer)
                .SetJobDescription(signaDesc)
                .SetPageWidth(format.Item1)
                .SetPageHeight(format.Item2)
                .Save();

            Process.Start(destFile);
            // }
        }

        private Tuple<decimal, decimal> GetFormatFile(IJob j)
        {
            var path = _profile.Jobs.GetFullPathToWorkFolder(j);

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*.pdf");
                if (files.Any())
                {
                    var f = new FileSystemInfoExt(files[0]);
                    f.GetExtendedFileInfoFormat();

                    if (f.Format.Width != 0 && f.Format.Height != 0)
                    {
                        return new Tuple<decimal, decimal>(f.Format.Width, f.Format.Height);
                    }
                }
            }

            return new Tuple<decimal, decimal>(210, 297);
        }

        public void OpenSignaJob(IUserProfile profile, IJob job)
        {
            var destFile = Extensions.GetSignaFilePath(job, profile);

            if (File.Exists(destFile))
            {
                Process.Start(destFile);
            }
        }

        public string GetSignaJobFilePath(IJob job)
        {
            return Extensions.GetSignaFilePath(job, _profile);
        }

        public string GetSignaJobFilePath(IJob job,string oldNumber)
        {
            return Extensions.GetSignaFilePath(job, _profile, oldNumber);
        }

        public void DuplicateSignaJob(IJob sourceJob, IJob targetJob)
        {
            if (sourceJob.IsSignaJobExist(_profile))// IsSignaJobExist(signaJobsPath, sourceJob))
            {
                if (targetJob is IJob j && sourceJob is IJob source)
                {
                    var sourceFile = Extensions.GetSignaFilePath(sourceJob, _profile);
                    var destFile = Extensions.GetSignaFilePath(targetJob, _profile);

                    File.Copy(sourceFile, destFile, true);

                    new SignaController(destFile).ChangeSignaOrderNumber(destFile, j.Number);
                }
            }
        }

        public void DownloadFromHttp(string link, IJob job)
        {
            HttpDownloader.Download(link, _profile.Jobs.GetFullPathToWorkFolder(job));
        }

        public bool ChangeJobDescription(IJob job, string newDesc)
        {
            var oldPath = _profile.Jobs.GetFullPathToWorkFolder(job);

            var save = job.Description;

            job.Description = newDesc;
            //job.Description = job.Description;

            if (RenameJobDirectory(oldPath, job))
            {
                _profile.Jobs.UpdateJob(job, true);
                return true;
            }
            job.Description = save;
            return false;
        }

        public bool RenameJobDirectory(SignaChangeOrderNumberParams param)
        {
            JobParameters oldJob = new JobParameters(param.Job);
            JobParameters newJob = new JobParameters(param.Job);
            newJob.Number = param.NewNumber;

            var oldPath = GetFullPathToWorkFolder(oldJob);
            var newPath = GetFullPathToWorkFolder(newJob);

            if (string.Compare(oldPath, newPath, ignoreCase: true, CultureInfo.InvariantCulture) != 0)
            {
                var parent = Path.GetDirectoryName(newPath);
                if (!Directory.Exists(parent)) Directory.CreateDirectory(parent);

                Retry:
                try
                {
                    Directory.Move(oldPath, newPath);

                }
                catch
                {
                    var message = FileUtil.GetNamesWhoBlock(oldPath);
                    if (MessageBox.Show($"Тека {oldPath} заблокована такими програмами: {message}", "Теку заблоковано", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                    {
                        goto Retry;
                    }
                    return false;
                }
            }

            return true;

        }

        public bool RenameJobDirectory(string oldDir, IJob job)
        {

            if (job.UseCustomFolder) return true;
            if (job.DontCreateFolder) return true;

            var newDir = _profile.Jobs.GetFullPathToWorkFolder(job);

            if (!oldDir.Equals(newDir))
            {
                var parent = Path.GetDirectoryName(newDir);
                if (!Directory.Exists(parent)) Directory.CreateDirectory(parent);

                Retry:
                try
                {
                    Directory.Move(oldDir, newDir);

                }
                catch
                {
                    var message = FileUtil.GetNamesWhoBlock(oldDir);
                    if (MessageBox.Show($"Тека {oldDir} заблокована такими програмами: {message}", "Теку заблоковано", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                    {
                        goto Retry;
                    }
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// змінити номер замовлення
        /// </summary>
        /// <param name="job"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ChangeJobNumber(IJob job, string number)
        {
            JobParameters newJob = new JobParameters(job);
            newJob.Number = number;

            _profile.Plugins.AfterJobChange(newJob);
        


            SignaChangeOrderNumberParams signaChangeOrderNumberParams = new SignaChangeOrderNumberParams
            {
                OldNumber = job.Number,
                NewNumber = newJob.Number,
                Job = job,
                Profile = _profile,
            };

            if (RenameJobDirectory(signaChangeOrderNumberParams))
            {
                RenameSignaJob(signaChangeOrderNumberParams);
                
                job.Number = newJob.Number;
                _profile.Jobs.UpdateJob(job, getEvent: true);

                return true;
            }

            return false;
        }

        private void RenameSignaJob(SignaChangeOrderNumberParams param)
        {
            JobParameters oldJob = new JobParameters(param.Job);
            JobParameters newJob = new JobParameters(param.Job);
            newJob.Number = param.NewNumber;

            string oldPath;
            string newPath = GetSignaJobFilePath(newJob);

            if (param.Profile.Settings.GetJobSettings().UseJobFolder)
            {
                oldPath = GetSignaJobFilePath(newJob,param.OldNumber);
            }
            else
            {
                oldPath = GetSignaJobFilePath(oldJob);
            }

            if (!File.Exists(oldPath)) return;
            if (oldPath.Equals(newPath)) return;

            if (File.Exists(newPath)) { File.Delete(newPath); }

            File.Move(oldPath, newPath);
            new SignaController(newPath).ChangeSignaOrderNumber(newPath, param.NewNumber);
        }

        public void ChangeStatusCode(string orderNumber, int statusCode)
        {
            var j = _profile.Base.GetByOrderNumber(orderNumber);
            if (j != null)
            {
                j.StatusCode = statusCode;
                _profile.Jobs.UpdateJob(j, true);
            }
        }

        public void LockJob(IJob job)
        {
            _profile.Jobs.JobListControl.LockJob(job);
            _profile.Plugins.MqController.PublishChanges(MessageEnum.JobBeginEdit, job.Id);
        }

        public void UnlockJob(IJob job)
        {
            _profile.Jobs.JobListControl.UnlockJob(job);
            _profile.Plugins.MqController.PublishChanges(MessageEnum.JobFinishEdit, job.Id);
        }
        /// <summary>
        /// об'єднати вибрані замовлення в одне
        /// </summary>
        /// <param name="selectedObjects"></param>
        public void CombineOrdersInOne(IList selectedObjects)
        {
            if (selectedObjects.Count > 1)
            {
                using (var form = new FormCombineOrders())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var customer = ((Job)selectedObjects[0]).Customer;

                        var job = Factory.CreateJob(_profile);
                        job.Number = form.OrderNumber;
                        job.Customer = customer;

                        _profile.Plugins.AfterJobChange(job);
                        _profile.Jobs.AddJob(job);

                        var dir = _profile.Jobs.GetFullPathToWorkFolder(job);

                        foreach (Job o in selectedObjects)
                        {
                            var targetDir = Path.Combine(dir, $"{o.Number}_{o.Description}");
                            //create folder
                            //Directory.CreateDirectory(targetDir);

                            //move files
                            var sourceDir = _profile.Jobs.GetFullPathToWorkFolder(o);

                            Microsoft.VisualBasic.FileSystem.Rename(sourceDir, targetDir);

                            MongoDeleteJob(o);
                            _jobList.Remove(o);
                            OnDeleteJob(this, o);

                        }
                    }
                }
            }
        }

        public void DeleteJob(IJob job)
        {
            MongoDeleteJob(job);
        }

    }
}
