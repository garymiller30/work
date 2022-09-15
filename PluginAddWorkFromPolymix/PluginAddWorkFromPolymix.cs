using Interfaces;
using Interfaces.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PluginAddWorkFromPolymix
{
    sealed class PluginAddWorkFromPolymix : IPluginNewOrder
    {
        public string PluginName => "з Polymix";
        public string PluginDescription => "Створити замовлення з Polymix";

        private PluginAddWorkFromPolymixSettings _addWorkFromPolymixSettings;
        public PluginAddWorkFromPolymixSettings AddWorkFromPolymixSettings
        {
            get => _addWorkFromPolymixSettings ?? (_addWorkFromPolymixSettings = LoadSettings());
            set => _addWorkFromPolymixSettings = value;
        }
        /*
                private string _fileSettingsPath;
        */

        public PluginAddWorkFromPolymix()
        {
            //LoadSettings();
        }

        private PluginAddWorkFromPolymixSettings LoadSettings()
        {
            var settings = UserProfile.Plugins.LoadSettings<PluginAddWorkFromPolymixSettings>();
            if (settings.CusomerNames == null) settings.CusomerNames = new Dictionary<string, string>();
            return settings;

            //_fileSettingsPath = Assembly.GetExecutingAssembly().Location + ".settings";

            //if (File.Exists(_fileSettingsPath))
            //{
            //    try
            //    {
            //        var str = File.ReadAllText(_fileSettingsPath,Encoding.Unicode);
            //        _settings=JsonConvert.DeserializeObject<PluginSettings>(str);

            //        if (_settings.CusomerNames == null) _settings.CusomerNames = new Dictionary<string, string>();
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.WriteLine(e);
            //    }
            //}
        }

        public void ShowSettingsDlg()
        {
            using (var form = new FormSettings(AddWorkFromPolymixSettings, UserProfile))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SaveSettings();
                }
            }
        }

        public IUserProfile UserProfile { get; set; }

        public Image PluginImage => Properties.Resources.New_Database_icon;
        public DialogResult ShowDialogNewOrder(IUserProfile profile, IJob job)
        {
            DialogResult result;

            using (var form = new FormAddOrder(AddWorkFromPolymixSettings, profile, job))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    SaveSettings();
                }
            }

            return result;

        }

        private void SaveSettings()
        {

            UserProfile.Plugins.SaveSettings(AddWorkFromPolymixSettings);
            //var str = JsonConvert.SerializeObject(_settings);
            //File.WriteAllText(_fileSettingsPath, str,Encoding.Unicode);
        }
    }
}
