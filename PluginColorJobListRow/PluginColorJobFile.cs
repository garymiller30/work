using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace PluginColorJobListRow
{
    public class PluginColorJobFile : IPluginFileBrowser
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
                        ((dynamic) row).Item.BackColor = Settings.GetJobColor(file.FileInfo.Name);
                        
                    }
                }
            }
        }

        public void FileBrowserSelectObject(IFileSystemInfoExt file)
        {
            //throw new NotImplementedException();
        }
    }
}
