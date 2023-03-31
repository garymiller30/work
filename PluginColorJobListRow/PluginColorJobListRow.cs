using Interfaces;
using Interfaces.Plugins;
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

            item.BackColor = Settings.Get(UserProfile).GetColor(job.StatusCode);

            Settings.SetJob($"{job.Number}_{job.Customer}", job.StatusCode);

        }
    }
}
