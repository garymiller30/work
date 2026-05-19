using ActiveWorks.Forms;
using ActiveWorks.PluginHub;
using ActiveWorks.Properties;
using BackgroundTaskServiceLib;
using JobSpace.Static;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ActiveWorks
{
    static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);

            if (TryHandleLockingProcessesHelper(args))
            {
                return;
            }

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

        private static bool TryHandleLockingProcessesHelper(string[] args)
        {
            if (args.Length != 3 || args[0] != "--activeworks-list-locking-processes")
            {
                return false;
            }

            try
            {
                var result = FileUtil.GetNamesWhoBlock(args[1], useElevatedFallback: false);
                File.WriteAllText(args[2], result, Encoding.UTF8);
            }
            catch (Exception e)
            {
                File.WriteAllText(args[2], $"[не вдалося отримати список]\n{e.Message}", Encoding.UTF8);
            }

            return true;
        }
    }
}
