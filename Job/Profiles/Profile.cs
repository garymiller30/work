using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using ActiveWorks;
using ExtensionMethods;
using Interfaces;
using Interfaces.Script;
using JobSpace.CustomerNotify;
using JobSpace.Data;
using JobSpace.Fasades;
using JobSpace.Menus;
using MailNotifier;
using Plugins;
using PythonEngine;

namespace JobSpace.Profiles
{
    public sealed class Profile : IUserProfile
    {
        public bool IsInitialized { get; private set; }
        public string ProfilePath { get; set; }
        public IProfileSettings Settings { get; set; } = new ProfileSettings();
        public IProfileEvents Events { get; set; } = new ProfileEvents.ProfileEvents();
        public IBaseManager Base { get; set; }
        public IJobManager Jobs { get; set; }
        public IPluginManager Plugins { get; set; }
        public ICustomerManager Customers { get; set; }
        public IFtpManager Ftp { get; set; }
        public ICustomerMailNotifyManager CustomersNotifyManager { get; set; }
        public IMail MailNotifier { get; set; }
        public ICategoryManager Categories { get; set; }
        public IMenuManager MenuManagers { get; set; }
        public IFileBrowsers FileBrowser { get; set; }
        public IJobStatusManager StatusManager { get; set; }

        public SearchManager SearchManager { get;set; }
        public ISearchHistory SearchHistory { get; set; }
        public IScriptEngine ScriptEngine {get;set;}

        public override string ToString()
        {
            return Settings.ProfileName ?? "Unknown";
        }

        public void InitProfile()
        {
            Stopwatch _sw = new Stopwatch();

            Logger.Log.Info(this, "завантажуємо плагіни: ", Settings.ProfileName);
            _sw.Start();
            LoadPlugins();
            _sw.Stop();
            Logger.Log.Info(this, "завантаження плагінів: ", _sw.ElapsedMilliseconds);
            _sw.Reset();
            _sw.Start();
            Logger.Log.Info(this, "ініціалізація Python: ", Settings.ProfileName);
            ScriptEngine = new PythonScriptEngine(this);
            _sw.Stop();
            Logger.Log.Info(this, "ініціалізація Python: ", _sw.ElapsedMilliseconds);
            _sw.Reset();

            Logger.Log.Info(this, "завантаження налаштувань з диску: ", Settings.ProfileName);
            _sw.Start();
            LoadSettingsFromDisk();
            _sw.Stop();
            Logger.Log.Info(this, "завантаження налаштувань з диску: ", _sw.ElapsedMilliseconds);
            _sw.Reset();
            Logger.Log.Info(this, "завантаження налаштувань з бази даних: ", Settings.ProfileName);
            _sw.Start();
            LoadSettingsFromBase();
            _sw.Stop();
            Logger.Log.Info(this, "завантаження налаштувань з бази даних: ", _sw.ElapsedMilliseconds);

            IsInitialized = true;
        }

        private void LoadPlugins()
        {
            Plugins = new PluginManager(this,Path.Combine(ProfilePath, "Plugins"));
            Plugins.Load();
        }

        private void LoadSettingsFromBase()
        {
            Base = new BaseManager(new MongoRepository(), Settings.GetBaseSettings());
            if (Base.Connect())
            {
                Customers = new CustomerManager(this);
                Ftp = new FtpManager(this);
                Ftp.InitUcFtpBrowserControls();
                Categories = new CategoryManager(this);
                MailNotifier = new Mail(this, Settings.GetMail());
                StatusManager = new JobStatusManager(this);
                SearchManager = new SearchManager(this);
                CustomersNotifyManager = new CustomerMailNotifyManager(this);
                Jobs = new JobManager(this, Settings.GetJobSettings());
            }
            else
            {
                Logger.Log.Error(this, "LoadSettingsFromBase","Can't connect to base");
            }
         }

        private void LoadSettingsFromDisk()
        {
            Stopwatch _sw = new Stopwatch();
            _sw.Start();
            Logger.Log.Info(this, "завантаження налаштувань з диску: SearchHistory", Settings.ProfileName);
            SearchHistory = new SearchHistory(this);
            _sw.Stop();
            Logger.Log.Info(this, "завантаження налаштувань з диску: SearchHistory", _sw.ElapsedMilliseconds);
            _sw.Reset();
            _sw.Start();
            Logger.Log.Info(this, "завантаження налаштувань з диску: MenuManagers", Settings.ProfileName);
            MenuManagers = new MenuManager(this);
            _sw.Stop();
            Logger.Log.Info(this, "завантаження налаштувань з диску: MenuManagers", _sw.ElapsedMilliseconds);
            _sw.Reset();
            Logger.Log.Info(this, "завантаження налаштувань з диску: FileBrowser", Settings.ProfileName);
            _sw.Start();
            FileBrowser = new FileBrowsers(this);
            _sw.Stop();
            Logger.Log.Info(this, "завантаження налаштувань з диску: FileBrowser", _sw.ElapsedMilliseconds);

        }

  
        /// <summary>
        /// визивається один раз при створенні профілю
        /// </summary>
        /// <param name="rootProfiles"></param>
        public void InitProfilePath(string rootProfiles)
        {
            if (!string.IsNullOrEmpty(Settings.ProfileName))
            {
                ProfilePath = Path.Combine(rootProfiles, Settings.ProfileName);
                Directory.CreateDirectory(ProfilePath);
            }
        }

        public void Exit()
        {
            if (IsInitialized)
            {
                Jobs?.JobListControl.Close();
                //MailNotifier?.StopWatching();
                Plugins?.MqController?.Disconnect();
                FileBrowser?.SaveSettings();
                StatusManager?.SaveViewStatuses();
            }
        }


        public void SaveSettings<T>(T settings) where T : class
        {
            var str = JsonSerializer.Serialize(settings); 

            var pluginSettingsPath =
                Path.Combine(ProfilePath, $"{typeof(T).Name}.json");

            File.WriteAllText(pluginSettingsPath, str, Encoding.Unicode);
        }

        public T LoadSettings<T>() where T : class, new()
        {
            var pluginSettingsPath =
                Path.Combine(ProfilePath, $"{typeof(T).Name}.json");

            if (!File.Exists(pluginSettingsPath)) return new T();

            try
            {
                var str = File.ReadAllText(pluginSettingsPath, Encoding.Unicode);
                var s = JsonSerializer.Deserialize<T>(str);
                //var s = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings
                //{
                //    TypeNameHandling = TypeNameHandling.Auto
                //});
                return s;
            }
            catch
            {
                return new T();
            }
        }

    }
}
