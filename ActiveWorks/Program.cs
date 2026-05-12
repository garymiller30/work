using ActiveWorks.Forms;
using ActiveWorks.PluginHub;
using ActiveWorks.Properties;
using BackgroundTaskServiceLib;
using System;
using System.IO;
using System.Windows.Forms;

namespace ActiveWorks
{
    static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            PluginPackageInstaller.ApplyPendingFiles(
                AppDomain.CurrentDomain.BaseDirectory,
                ResolveApplicationPath(Settings.Default.ProfilesPath));
            DeferredDeleteService.Initialize();

            var profilesPath = ResolveApplicationPath(Settings.Default.ProfilesPath);

            if (!FormInitialProfileWizard.HasProfiles(profilesPath))
            {
                using (var wizard = new FormInitialProfileWizard(profilesPath))
                {
                    if (wizard.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            Application.Run(new Form2());
        }

        private static string ResolveApplicationPath(string path)
        {
            return Path.IsPathRooted(path)
                ? path
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
    }
}
