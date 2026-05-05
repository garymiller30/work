using ActiveWorks.Forms;
using ActiveWorks.Properties;
using QRCoder;
using System;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
    }
}
