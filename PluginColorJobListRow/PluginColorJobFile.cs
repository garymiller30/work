using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Profile;
using JobSpace;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginColorJobListRow
{
    public sealed class PluginColorJobFile : IPluginFileBrowser, IPluginInfo
    {
        public string PluginName => "Кольорові рядки";
        public string PluginDescription => "Підсвічує кольором статусу файли";
        public void ShowSettingsDlg()
        {
            //throw new NotImplementedException();
        }

        IJob changingJob;
        int changingStatusCode;

        public IUserProfile UserProfile { get; set; }
        public void FileBrowserFormatRow(IFileBrowser browser, object row)
        {
            if (UserProfile.FileBrowser.Browsers.IndexOf(browser) == 1)
            {

                if (((dynamic)row).Model is IFileSystemInfoExt file)
                {
                    if (file.FileInfo.Extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var color = Settings.GetJobColor(file.FileInfo.Name);
                        if (color != null)
                        {
                            if (color.Back != Color.Transparent)
                                ((dynamic)row).Item.BackColor = color.Back;
                            if (color.Fore != Color.Transparent)
                                ((dynamic)row).Item.ForeColor = color.Fore;
                        }

                    }
                }
            }
        }

        public void FileBrowserSelectObject(IFileBrowser fileBrowser, IFileSystemInfoExt file)
        {
            List<IFileBrowser> fileBrowsers = UserProfile.FileBrowser.Browsers;
            if (fileBrowsers.Count > 1 && fileBrowsers[1] == fileBrowser)
            {
                if (Settings.Get(UserProfile).SelectJobOnClick)
                {
                    //з імені файлу взяти номер замовлення. Номер знаходиться на початку файлу, містить 5 цифр і відділяється від решти нижнім підкресленням. Наприклад: 12345_назва_файлу.pdf
                    var name = file.FileInfo.Name;
                    var jobNumber = name.Split('_').FirstOrDefault();
                    if (jobNumber != null && jobNumber.Length == 5 && jobNumber.All(char.IsDigit))
                    {
                        var jobList = UserProfile.Jobs.JobListControl.GetJobList();
                        //знайти замовлення з таким номером
                        var job = jobList.Cast<IJob>().FirstOrDefault(j => j.Number == jobNumber);
                        if (job != null)
                            UserProfile.Jobs.JobListControl.SelectJob(job);
                    }
                }
            }
        }

        public UserControl GetUserControl()
        {
            return null;
        }

        public void Start()
        {
            if (UserProfile?.Jobs !=null)
                UserProfile.Jobs.OnJobChange += MqController_OnJobChanged;
        }

        private void MqController_OnJobChanged(object sender, IJob job)
        {
            RefreshBrowserUI(job);
        }

        public string GetPluginName()
        {
            return PluginName;
        }

        public void SetCurJob(IJob curJob)
        {

        }

        public void BeforeJobChange(IJob job)
        {
            changingJob = job;
            changingStatusCode = job.StatusCode;
        }

        public void AfterJobChange(IJob job)
        {
            if (changingJob != null && changingJob.Number == job?.Number)
            {
                if (changingStatusCode != job.StatusCode)
                {
                    RefreshBrowserUI(job);
                }
            }
        }

        private void RefreshBrowserUI(IJob job)
        {
            Settings.SetJob(job.Number, job.StatusCode);
            //змінити колір рядка в файловому браузері для файлів цього замовлення
            var fileBrowsers = UserProfile.FileBrowser.Browsers;
            if (fileBrowsers.Count > 1)
            {
                IFileBrowser browser = fileBrowsers[1];
                browser.RefreshUI();
            }
        }
    }
}
