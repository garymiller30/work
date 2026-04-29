using ActiveWorks.Properties;
using Logger;
using System;
using System.Windows.Forms;

namespace ActiveWorks
{
    public sealed partial class Form2
    {
        private void Form2_Load(object sender, EventArgs e)
        {
            SuspendLayout();

            CreateProfilesTab();

            SetActiveDefaultProfile();
            kryptonRibbon1.SelectedTabChanged += KryptonRibbon1_SelectedTabChanged;

            var virtualSize = SystemInformation.VirtualScreen;

            if (Settings.Default.VirtualScreenX == virtualSize.X &&
                Settings.Default.VirtualScreenY == virtualSize.Y &&
                Settings.Default.VirtualScreenW == virtualSize.Width &&
                Settings.Default.VirtualScreenH == virtualSize.Height)
            {
                if (Settings.Default.WindowLocation.X >= virtualSize.X && Settings.Default.WindowLocation.Y >= virtualSize.Y)
                    Location = Settings.Default.WindowLocation;
                // Set window size
                if (Settings.Default.WindowSize.Width >= RestoreBounds.Size.Width && Settings.Default.WindowSize.Height >= RestoreBounds.Height)
                    Size = Settings.Default.WindowSize;
                else
                {
                    Size = RestoreBounds.Size;
                }

            }
            else
            {
                Size = RestoreBounds.Size;
            }

            ResumeLayout();
            SplashScreen.Splash.CloseForm();
            Activate();

            _sw.Stop();
            Log.Info("App", "App", $"started by {_sw.ElapsedMilliseconds} ms");

            BeginInvoke(new Action(async () => await CheckForUpdatesAsync()));
            ConfigureUpdateTimer();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Copy window location to app settings
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            Settings.Default.WindowLocation = Location;
            Settings.Default.WindowSize = Size;
            var vs = SystemInformation.VirtualScreen;
            Settings.Default.VirtualScreenX = vs.X;
            Settings.Default.VirtualScreenY = vs.Y;
            Settings.Default.VirtualScreenW = vs.Width;
            Settings.Default.VirtualScreenH = vs.Height;

            // Save settings
            Settings.Default.Save();
        }
    }
}
