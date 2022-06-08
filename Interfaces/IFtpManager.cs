using System.Collections.Generic;

namespace Interfaces
{
    public interface IFtpManager
    {
        IFtpSettings GetParamByName(string server);
        IUcFtpBrowser FtpExplorer { get; set; }
        IFtpScriptController FtpScriptController { get; set; }

        ICollection<IFtpSettings> GetCollection();
        IFtpSettings this[int index] { get; set; }

        //void SetList(IEnumerable<IFtpScript> list);
        void InitUcFtpBrowserControls();

        void Add(IFtpSettings fs);
        void Remove(IFtpSettings fs);
        void Update(IFtpSettings fs);
        void CreateEvents();

        //ConnectionSettings ConvertFromFtpSettingToConnectionSettings(IFtpSettings settings);
    }
}
