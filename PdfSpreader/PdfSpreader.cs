// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using BrightIdeasSoftware;
using Interfaces;
using Job.Models;
using Logger;
using RulesService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Timer = System.Timers.Timer;


namespace PdfSpreader
{
    public class PdfSpreader : IPluginFileBrowser
    {
        private readonly RulesService.RulesService _rulesService;
        List<AbstractRule> _formatRules = new List<AbstractRule>();
        private readonly string _rulesFile;
        private readonly object _lock = new object();
        private readonly FileSystemWatcher _watcher = new FileSystemWatcher();

        public PdfSpreader()
        {
            var map = new ExeConfigurationFileMap
            {
                ExeConfigFilename = $"{Assembly.GetExecutingAssembly().Location}.config"
            };
            var libConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            var section = libConfig.GetSection("appSettings") as AppSettingsSection;
            if (section.Settings.Count > 0)
            {
                _rulesFile = section.Settings["RulesDirectory"].Value;

                Log.Info(this, "PdfSpreader", $"Rules path: {_rulesFile}");
                _rulesService = new RulesService.RulesService();

                GetRules();
                InitFileWatcher(_rulesFile);

            }
            else
            {
                Log.Error(this, "PdfSpreader", $"Can't load settings");
            }

        }

        ~PdfSpreader()
        {
            _watcher?.Dispose();
        }

        private void GetRules()
        {
            lock (_lock)
            {
                try
                {
                    _rulesService.LoadRules(_rulesFile);
                    _formatRules = _rulesService.GetRules().Where(x => x is RuleFileSize).ToList();
                }
                catch (Exception e)
                {
                    Log.Error(this, "PdfSpreader", e.Message);
                }

            }

        }

        private void InitFileWatcher(string path)
        {
            var dir = Path.GetDirectoryName(path);
            var name = Path.GetFileName(path);


            _watcher.Path = dir;
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.Filter = name;
            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = true;

        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {

            var timer = new Timer(2000);
            timer.Elapsed += (o, args) => GetRules();
            timer.Start();
        }

        public IUserProfile UserProfile { get; set; }

        public void FileBrowserFormatRow(IFileBrowser browser, object row)
        {
            if (UserProfile.FileBrowser.Browsers.IndexOf(browser) == 0)
            {
                if (row is FormatRowEventArgs r)
                {
                    if (r.Model is FileSystemInfoExt file)
                    {
                        if (Path.GetExtension(file.FileInfo.FullName)
                            .Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (CheckFormat(file))
                                r.Item.BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                }
            }
        }

        public void FileBrowserSelectObject(IFileSystemInfoExt file)
        {
            //MessageBox.Show($"{file?.FileInfo?.Name}");
            // skip. Not used
        }

        private bool CheckFormat(IFileSystemInfoExt file)
        {
            foreach (var abstractRule in _formatRules)
            {
                var rule = (RuleFileSize)abstractRule;
                if (rule.Check((double)Math.Round(file.Format.Width, 1), (double)Math.Round(file.Format.Height, 1),
                    file.Format.cntPages)) return true;
            }
            return false;
        }

        public string PluginName => "PdfSpreader";
        public string PluginDescription => "виділяє кольором файли, які можна пропустити через PrepressSpreader";

        public void ShowSettingsDlg()
        {
            MessageBox.Show("Наразі налаштування зберігаються у файлі PdfSpreader.exe.config");
        }
    }
}
