using System;
using System.Windows.Forms;
using Interfaces;
using Interfaces.Plugins;

namespace PluginWorkPrepress
{
    public class WorkPrepressPlugin : AbstractPluginAddWork<PrepressProcess>, IPluginBase
    {
   
        protected override void CreateContextMenu()
        {
            var tsmi = new ToolStripMenuItem("додати", null, OnAdd) {Tag = this};
            _menuPlugin.Items.Add(tsmi);
        }
        private void OnAdd(object sender, EventArgs e)
        {
            var process = AddProcess();
            if (!process.EditProcess(UserProfile))
            {
                RemoveProcess(process);
            }
            else
            {
                Update(process);
            }

        }

        public override string Name { get; set; } = "Препрес";


        public string PluginName { get; } = "Препрес";
        public string PluginDescription { get; } = "вартість препресу";
        public void ShowSettingsDlg()
        {
            MessageBox.Show("Наразі налаштувань немає");
        }
    }
}
