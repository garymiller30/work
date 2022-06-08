using System;
using System.Collections.Generic;
using Interfaces.Plugins;

namespace Interfaces
{
    public interface IPluginManager
    {
        IEnumerable<IPluginFormAddWork> GetPluginFormAddWorks();
        string[] GetPluginNames();
        void InvokeMethod(string pluginName, string methodName, Func<object> param);
        object InvokeMethod(string pluginName, string methodName);
        void InvokeMethod(string pluginName, string methodName, object param);
        //void SetCurJob(IJob job);
        void BeforeJobChange(IJob job);
        void AfterJobChange(IJob job);
        void FileBrowserFormatRow(IFileBrowser browser, object row);
        void JobListFormatRow(object row);
        void FileBrowserSelectObject(IFileSystemInfoExt file);
        IPluginBase[] GetPluginsBase();
        IPluginNewOrder[] GetPluginsNewOrder();
        IMailPluginController Mail { get; set; }

        IMqController MqController { get; set; }
        T LoadSettings<T>(IPluginBase plugin) where T : class, new();
        
        void SaveSettings<T>(IPluginBase plugin, T settings) where T : class;
        
        void Load();

        T LoadSettings<T>() where T : class, new();
        void SaveSettings<T>(T settings) where T : class;
        void PlaySound(AvailableSound soundType,object param);
    }
}
