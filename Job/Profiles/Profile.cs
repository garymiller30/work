// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.IO;
using System.Text;
using ActiveWorks;
using ExtensionMethods;
using Interfaces;
using Interfaces.Script;
using JobSpace.CustomerNotify;
using JobSpace.Data;
using JobSpace.Fasades;
using JobSpace.Menus;
using MailNotifier;
using Newtonsoft.Json;
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
        public ISearchHistory SearchHistory { get; set; }
        public IScriptEngine ScriptEngine {get;set;}

        public override string ToString()
        {
            return Settings.ProfileName ?? "Unknown";
        }

        public void InitProfile()
        {
            LoadPlugins();
            ScriptEngine = new PythonScriptEngine(this);
            LoadSettingsFromDisk();
            LoadSettingsFromBase();
            
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
            SearchHistory = new SearchHistory(this);
            MenuManagers = new MenuManager(this);
            FileBrowser = new FileBrowsers(this);
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
                //MailNotifier?.StopWatching();
                Plugins?.MqController?.Disconnect();
                FileBrowser?.SaveSettings();
                StatusManager?.SaveViewStatuses();
            }
        }


        public void SaveSettings<T>(T settings) where T : class
        {
            var str = JsonConvert.SerializeObject(settings, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

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
                var s = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                return s;
            }
            catch
            {
                return new T();
            }
        }

    }
}
