using Interfaces.Script;

namespace Interfaces
{
    public interface IUserProfile
    {
        string ProfilePath { get; set; }
        IProfileEvents Events { get; set; }
        IBaseManager Base { get; set; }
        IProfileSettings Settings { get; set; }
        IFileBrowsers FileBrowser { get; set; }
        IJobManager Jobs { get; set; }
        IMail MailNotifier { get; set; }
        ICustomerManager Customers { get; set; }
        ICategoryManager Categories { get; set; }
        //IMQManager MQ { get; set; }
        IJobStatusManager StatusManager { get; set; }
        IMenuManager MenuManagers { get; set; }
        IPluginManager Plugins { get; set; }
        IFtpManager Ftp { get; set; }
        ICustomerMailNotifyManager CustomersNotifyManager { get; set; }
        ISearchHistory SearchHistory { get; set; }
        IScriptEngine ScriptEngine {get;set;}
        
        void InitProfile();
        void Exit();

        T LoadSettings<T>() where T : class, new();
        void SaveSettings<T>(T settings) where T : class;
    }
}
