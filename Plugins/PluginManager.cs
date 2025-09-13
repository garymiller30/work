// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using ExtensionMethods;
using Interfaces;
using Interfaces.Plugins;
using Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plugins
{
    public sealed class PluginManager : IPluginManager
    {
        private readonly IUserProfile _profile;
        private readonly string _pluginsPath;
        private readonly string _pluginsSettingsPath;

        readonly Dictionary<string, IPluginInfo> _plugins = new Dictionary<string, IPluginInfo>();
        readonly List<IPluginFileBrowser> _pluginsFileBrowser = new List<IPluginFileBrowser>();
        readonly List<IPluginFormAddWork> _pluginFormAddWorks = new List<IPluginFormAddWork>();
        readonly List<IPluginNewOrder> _pluginNewOrders = new List<IPluginNewOrder>();
        private readonly List<IPluginJobList> _pluginJobLists = new List<IPluginJobList>();

        private IPluginPlaySound _pluginPlaySound;
        public IPluginNewOrder[] GetPluginsNewOrder()
        {
            return _pluginNewOrders.ToArray();
        }

        public IMailPluginController Mail { get; set; } = new MailController();
        public IMqController MqController { get; set; } = new MqController();

        public PluginManager(IUserProfile profile, string pluginDir)
        {
            _profile = profile;
            _profile.Events.Jobs.OnSetCurrentJob += Jobs_OnSetCurrentJob;

            _pluginsPath = pluginDir;
            _pluginsSettingsPath = Path.Combine(_pluginsPath, "Settings");
            if (!Directory.Exists(_pluginsSettingsPath)) Directory.CreateDirectory(_pluginsSettingsPath);

            //Load(pluginDir);
        }

        private void Jobs_OnSetCurrentJob(object sender, IJob e)
        {
            SetCurJob(e);
        }

        public void Load()
        {
            Load(_pluginsPath);
        }

        public void Load(string path)
        {
            if (Directory.Exists(path))
            {
                Stopwatch _sw = new Stopwatch();



                var files = new DirectoryInfo(path).GetFiles("*.dll");

                foreach (var fi in files)
                {
                    _sw.Start();
                    Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Loading: ", fi.Name);
                    try
                    {
                        var assembly = Assembly.LoadFile(fi.FullName);
                        var types = assembly.GetTypes();

                        // шукаємо тип
                        foreach (var type in types)
                        {
                            if (typeof(IPluginInfo).IsAssignableFrom(type))
                            {
                                var obj = (IPluginInfo)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                var name = obj.GetPluginName();

                                if (!_plugins.ContainsKey(name))
                                {
                                    _plugins.Add(name, obj);
                                    Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", obj.PluginName);
                                }
                            }
                            else if (typeof(IPluginFileBrowser).IsAssignableFrom(type))
                            {
                                var obj = (IPluginFileBrowser)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                _pluginsFileBrowser.Add(obj);
                                Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", obj.PluginName);
                            }
                            else if (typeof(IPluginFormAddWork).IsAssignableFrom(type))
                            {
                                var obj = (IPluginFormAddWork)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                _pluginFormAddWorks.Add(obj);
                                Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", obj.Name);
                            }
                            else if (typeof(IPluginMail).IsAssignableFrom(type))
                            {
                                var obj = (IPluginMail)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                Mail.Add(obj);
                                Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", obj.PluginName);
                            }

                            if (typeof(IPluginNewOrder).IsAssignableFrom(type))
                            {
                                var obj = (IPluginNewOrder)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                _pluginNewOrders.Add(obj);
                                Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", obj.PluginName);
                            }

                            else if (typeof(IPluginPlaySound).IsAssignableFrom(type))
                            {
                                var obj = (IPluginPlaySound)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                _pluginPlaySound = obj;
                                Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", obj.PluginName);
                            }

                            else if (typeof(IPluginJobList).IsAssignableFrom(type))
                            {
                                var obj = (IPluginJobList)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                _pluginJobLists.Add(obj);
                                Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", obj.PluginName);
                            }
                            else if (typeof(IMqPlugin).IsAssignableFrom(type))
                            {
                                Debug.WriteLine("add IMqPlugin");
                                var obj = (IMqPlugin)Activator.CreateInstance(type);
                                obj.UserProfile = _profile;
                                MqController.Add(obj);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(this, $"({_profile.Settings.ProfileName}) Plugins Manager", $"{fi.Name}: Error load assembly: {e.Message}");

                        if (e is ReflectionTypeLoadException typeLoadException)
                        {
                            var loaderExceptions = typeLoadException.LoaderExceptions;
                            Log.Error(this, $"({_profile.Settings.ProfileName}) Plugins Manager",
                                loaderExceptions.Select(x => $"{x.Message}, ").Aggregate((a, n) => $"{a}{n}"));
                            //MessageBox.Show(loaderExceptions.Select(x=>x.Message+ "\n").Aggregate((a,n)=>a+n));
                        }

                    }
                    finally
                    {
                        _sw.Stop();
                        Log.Info(this, $"({_profile.Settings.ProfileName}) Plugin Manager : Load : ", $"{fi.Name} - {_sw.ElapsedMilliseconds} ms");
                        _sw.Reset();

                    }

                    // Получаем assemly из файла

                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }

        }

        public void BeforeJobChange(IJob job)
        {
            foreach (var plugin in _plugins)
            {
                //Log.Info(this,"Plugin Manager : BeforeJobChange - Start : ",plugin.Value.PluginName);
                plugin.Value.BeforeJobChange(job);
                //Log.Info(this,"Plugin Manager : BeforeJobChange - End : ",plugin.Value.PluginName);
                //InvokeMethod(plugin.Key, "BeforeJobChange",job);
            }
        }

        public void AfterJobChange(IJob job)
        {
            foreach (var plugin in _plugins)
            {
                //Log.Info(this,"Plugin Manager : AfterJobChange - Start : ",plugin.Value.PluginName);
                plugin.Value.AfterJobChange(job);
                //Log.Info(this,"Plugin Manager : AfterJobChange - End : ",plugin.Value.PluginName);
                //InvokeMethod(plugin.Key, "AfterJobChange",job);
            }
        }

        public int Count()
        {
            return _plugins.Count;
        }

        public object InvokeMethod(string pluginName, string methodName)
        {
            if (_plugins.ContainsKey(pluginName))
            {
                var method = _plugins[pluginName].GetType().GetMethod(methodName);
                if (method != null)
                {
                    return method.Invoke(_plugins[pluginName], null);
                }
            }
            return null;
        }

        public void InvokeMethod(string pluginName, string methodName, Func<object> param)
        {
            if (_plugins.ContainsKey(pluginName))
            {

                var method = _plugins[pluginName].GetType().GetMethod(methodName);
                if (method != null)
                {
                    method.Invoke(_plugins[pluginName], new object[] { param });
                }
            }
        }

        public void InvokeMethod(string pluginName, string methodName, object param)
        {
            if (_plugins.ContainsKey(pluginName))
            {

                var method = _plugins[pluginName].GetType().GetMethod(methodName);
                if (method != null)
                {
                    method.Invoke(_plugins[pluginName], new[] { param });
                }
            }
        }

        public void SetProperty(string pluginName, string propertyName, object value)
        {
            if (_plugins.ContainsKey(pluginName))
            {
                var property = _plugins[pluginName].GetType().GetProperty(propertyName);
                property.SetValue(_plugins[pluginName], Convert.ChangeType(value, property.PropertyType), null);
            }
        }

        public string[] GetPluginNames()
        {
            return _plugins.Keys.ToArray();
        }

        private void SetCurJob(IJob job)
        {
            foreach (var plugin in _plugins)
            {
                plugin.Value.SetCurJob(job);
            }
        }

        public void FileBrowserFormatRow(IFileBrowser browser, object row)
        {
            foreach (var plugin in _pluginsFileBrowser)
            {
                plugin.FileBrowserFormatRow(browser, row);
            }
        }

        public void JobListFormatRow(object row)
        {
            foreach (var plugin in _pluginJobLists)
            {
                plugin.SetRow(row);
            }
        }

        public void FileBrowserSelectObject(IFileSystemInfoExt file)
        {
            foreach (var pluginFileBrowser in _pluginsFileBrowser)
            {
                pluginFileBrowser.FileBrowserSelectObject(file);
            }
        }

        public IPluginBase[] GetPluginsBase()
        {
            var list = new List<IPluginBase>();

            foreach (var plug in _plugins)
            {
                list.Add(plug.Value);
            }
            list.AddRange(_pluginsFileBrowser);
            list.AddRange(Mail.GetPluginBase());
            list.AddRange(MqController.GetPluginBase());
            list.AddRange(_pluginNewOrders);

            foreach (var addWork in _pluginFormAddWorks)
            {
                if (addWork is IPluginBase baseAddWork)
                {
                    list.Add(baseAddWork);
                }
            }

            if (_pluginPlaySound != null)
                list.Add(_pluginPlaySound);

            list.AddRange(_pluginJobLists);

            return list.ToArray();
        }

        public IEnumerable<IPluginFormAddWork> GetPluginFormAddWorks()
        {
            return _pluginFormAddWorks;
        }

        #region Work with plugins settings

        public void SaveSettings<T>(IPluginBase plugin, T settings) where T : class
        {
            var str = JsonConvert.SerializeObject(settings, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            var pluginSettingsPath =
                Path.Combine(_pluginsSettingsPath, $"{plugin.PluginName.Transliteration()}.settings");

            File.WriteAllText(pluginSettingsPath, str, Encoding.Unicode);
        }

        public T LoadSettings<T>() where T : class, new()
        {
            var pluginSettingsPath =
                Path.Combine(_pluginsSettingsPath, $"{typeof(T).Name}.json");

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

        public void SaveSettings<T>(T settings) where T : class
        {
            var str = JsonConvert.SerializeObject(settings, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            var pluginSettingsPath =
                Path.Combine(_pluginsSettingsPath, $"{typeof(T).Name}.json");

            File.WriteAllText(pluginSettingsPath, str, Encoding.Unicode);
        }
        public void PlaySound(AvailableSound soundType, object param)
        {
            _pluginPlaySound?.PlaySound(soundType, param);
        }
        /// <summary>
        /// Завантажити налаштування
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plugin"></param>
        /// <returns>вертає завантажені налаштування або новий клас</returns>
        public T LoadSettings<T>(IPluginBase plugin) where T : class, new()
        {
            //var setting = _pluginSettings.FirstOrDefault(x => x is T);
            //if (setting != null) return (T)setting;


            var pluginSettingsPath =
                Path.Combine(_pluginsSettingsPath, $"{plugin.PluginName.Transliteration()}.settings");


            if (File.Exists(pluginSettingsPath))
            {
                try
                {
                    var str = File.ReadAllText(pluginSettingsPath, Encoding.Unicode);
                    var s = JsonConvert.DeserializeObject<T>(str);

                    // _pluginSettings.Add(s);

                    return s;
                }
                catch
                {
                    return new T();
                }

            }
            return new T();
        }

        public void RemoveProcessesByJobId(object id)
        {
            // почистити плагіни
            foreach (var pluginFormAddWork in GetPluginFormAddWorks())
            {
                pluginFormAddWork.RemoveProcessByJobId(id);
            }
        }




        public string ReplaceStr(IJob job, string str)
        {
            // знайти в рядку шаблон $plugin:"ІмяПлагіна":[Параметр] через регулярки
            string template = @"\$plugin:""(?<pluginName>[^""]+)""\:\[(?<param>[^\]]+)\]";
            var matches = System.Text.RegularExpressions.Regex.Matches(str, template);
            if (matches.Count > 0)
            {

                string replaced = str;

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    var pluginName = match.Groups["pluginName"].Value;
                    var param = match.Groups["param"].Value;

                    IPluginFormAddWork plugin = GetPluginAddWorkByName(pluginName);
                    if (plugin != null)
                    {
                        string res = plugin.GetValue(job, param);
                        // замінити в рядку
                        replaced = replaced.Replace(match.Value, res);
                    }
                }

                return replaced;
            }

            return str;
        }

        private IPluginFormAddWork GetPluginAddWorkByName(string pluginName)
        {
            var plugin = _pluginFormAddWorks.FirstOrDefault(x => x.Name.Equals(pluginName, StringComparison.InvariantCulture));
            return plugin;
        }
        #endregion
    }
}
