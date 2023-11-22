using System.Collections.Generic;

namespace Interfaces
{
    public interface IFileBrowsers
    {
        //void LoadSettings(IUserProfile profile);
        void SaveSettings();
        void SetCustomButtonPath();
        //void InitToolStripUtils();
        //void LoadSettings();

        List<IFileBrowser> Browsers { get; set; }

        void CreateEvents();
        void InitBrowserToolStripUtils();
    }
}
