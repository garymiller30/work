using System;
using System.Windows.Forms;
using Interfaces;
using Interfaces.Plugins;
using PluginWorkProcessPlates.Forms;

namespace PluginWorkProcessPlates
{
    public class WorkProcessPlatePlugin : AbstractPluginAddWork<PlateProcess>, IPluginBase
    {
        private PlateSettings _settings;


        public override string Name { get; set; } = "Форми";

        protected override void CreateContextMenu()
        {
            var tsmi = new ToolStripMenuItem("додати", null, OnAdd) { Tag = this };
            _menuPlugin.Items.Add(tsmi);
        }

        private void OnAdd(object sender, EventArgs e)
        {
            var process = AddProcess();

            SetDefaults((PlateProcess)process);

            if (!process.EditProcess(UserProfile))
            {
                RemoveProcess(process);
            }
            else
            {
                Update(process);
            }
        }

        private void SetDefaults(PlateProcess process)
        {
            LoadSettings();

            process.PriceForPlate = _settings.PriceForPlate;
        }

        public string PluginName { get; } = "Форми";
        public string PluginDescription { get; } = "вартість форм";
        public void ShowSettingsDlg()
        {
            LoadSettings();

            using (var formSettings = new FormSettings(_settings))
            {
                if (formSettings.ShowDialog() == DialogResult.OK)
                {
                    UserProfile.Plugins.SaveSettings(_settings);
                }
            }
        }

        void LoadSettings()
        {
            if (_settings == null)
                _settings = UserProfile.Plugins.LoadSettings<PlateSettings>();
        }
    }
}
