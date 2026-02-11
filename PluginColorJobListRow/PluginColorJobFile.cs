using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace PluginColorJobListRow
{
    public sealed class PluginColorJobFile : IPluginFileBrowser
    {
        public string PluginName => "";
        public string PluginDescription => "Підсвічує кольором статусу файли";
        public void ShowSettingsDlg()
        {
            //throw new NotImplementedException();
        }

        public IUserProfile UserProfile { get; set; }
        public void FileBrowserFormatRow(IFileBrowser browser, object row)
        {
            if (UserProfile.FileBrowser.Browsers.IndexOf(browser) == 1)
            {
                
                if (((dynamic) row).Model is IFileSystemInfoExt file)
                {
                    if (file.FileInfo.Extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var color = Settings.GetJobColor(file.FileInfo.Name);
                        if ( color != null)
                        {
                            if (color.Back != Color.Transparent )
                                ((dynamic)row).Item.BackColor = color.Back;
                            if (color.Fore != Color.Transparent )
                                ((dynamic)row).Item.ForeColor = color.Fore;
                        }

                    }
                }
            }
        }

        public void FileBrowserSelectObject(IFileSystemInfoExt file)
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
            //throw new NotImplementedException();
        }
    }
}
