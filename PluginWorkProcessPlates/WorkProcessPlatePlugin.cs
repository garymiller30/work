using System;
using System.Windows.Forms;
using Interfaces;
using Interfaces.Plugins;
using PluginWorkProcessPlates.Forms;

namespace PluginWorkProcessPlates
{
    public sealed class WorkProcessPlatePlugin : AbstractPluginAddWork<PlateProcess>, IPluginBase
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

        public override string ToString()
        {
            return Name;
        }

        public override string GetValue(IJob job, string param)
        {
            if (param == "TotalForms")
            {
                int count = 0;
                foreach (var process in GetProcesses())
                {
                    if (process is PlateProcess p && p.ParentId.Equals(job.Id))
                    {
                        count += p.CountPlates;
                    }
                }
                return count.ToString();
            }
            else if (param == "FormFormat")
            {
                string formats = string.Empty;
                foreach (var process in GetProcesses())
                {
                    if (process is PlateProcess p && p.ParentId.Equals(job.Id))
                    {
                        formats = $"{p.PlateFormat.Width:0.#}x{p.PlateFormat.Height:0.#}";
                    }
                    break;
                }
                return formats;
            }
            return string.Empty;
        }
    }
}
