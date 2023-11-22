using ExtensionMethods;
using Interfaces;
using Interfaces.Plugins;
using Job.Static;
using System.Drawing;
using System.Windows.Forms;

namespace PluginColorJobListRow
{
    sealed class PluginColorJobListRow : IPluginJobList
    {


        public string PluginName => "Color JobList row";
        public string PluginDescription => "розфарбовує рядки у списку робіт";
        public void ShowSettingsDlg()
        {


            using (var form = new FormSettings(Settings.Get(UserProfile), UserProfile))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Settings.Save(UserProfile);
                }
            }
        }

        public IUserProfile UserProfile { get; set; }
        public void SetRow(object row)
        {
            var item = (dynamic)row;
            IJob job = (IJob)item.RowObject;

            var color = Settings.Get(UserProfile).GetColor(job.StatusCode);
            var backColor = color.Back;
            var foreColor = color.Fore;
            backColor = backColor == Color.Transparent ? ThemeController.Back : backColor;
            foreColor = foreColor == Color.Transparent ? ThemeController.Fore : foreColor;
            item.BackColor = backColor;
            item.ForeColor = foreColor;

            Settings.SetJob($"{job.Number}_{job.Customer.Transliteration()}", job.StatusCode);

        }
    }
}
