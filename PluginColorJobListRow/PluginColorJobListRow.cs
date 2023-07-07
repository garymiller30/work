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

            var backColor = Settings.Get(UserProfile).GetColor(job.StatusCode);

            backColor = backColor == Color.Transparent ? ThemeController.Back : backColor;

            item.BackColor = backColor;
            //item.ForeColor = Color.FromArgb(backColor.ToArgb()^0xffffff);

            Settings.SetJob($"{job.Number}_{job.Customer.Transliteration()}", job.StatusCode);

        }
    }
}
