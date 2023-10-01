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
            //throw new NotImplementedException();
        }
    }
}
