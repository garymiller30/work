using ActiveWorks.Forms;
using ActiveWorks.PluginHub;
using ActiveWorks.Properties;
using QRCoder;
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

            if (!FormInitialProfileWizard.HasProfiles(Settings.Default.ProfilesPath))
            {
                using (var wizard = new FormInitialProfileWizard(Settings.Default.ProfilesPath))
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
