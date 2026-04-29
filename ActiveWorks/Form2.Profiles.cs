using ActiveWorks.Properties;
using ActiveWorks.UserControls;
using ExtensionMethods;
using JobSpace.Profiles;
using Krypton.Ribbon;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Profile = JobSpace.Profiles.Profile;

namespace ActiveWorks
{
    public sealed partial class Form2
    {
        private void SetActiveDefaultProfile()
        {
            var defProfileName = Settings.Default.DefaultProfile;
            var profile =
                _profileTabs.FirstOrDefault(x => ((JobSpace.Profiles.Profile)x.Tag).Settings.ProfileName.Equals(defProfileName));

            if (profile == null && _profileTabs.Count > 0)
            {
                profile = _profileTabs[0];
            }

            if (profile != null)
            {
                ChangeUserProfile(profile);
                SetRibbonTab(profile);
            }
        }

        private void SetRibbonTab(FormProfile formProfile)
        {
            var ribbonTab = kryptonRibbon1.RibbonTabs.FirstOrDefault(x => (FormProfile)x.Tag == formProfile);
            if (ribbonTab != null)
            {

                kryptonRibbon1.SelectedTab = ribbonTab;

            }
        }

        private void ChangeUserProfile(FormProfile formProfile)
        {
            _activeProfileTab = formProfile;
            formProfile.Activate();
        }
        /// <summary>
        /// Create Profile Ribbon  Tab
        /// </summary>
        private void CreateProfilesTab()
        {
            SplashScreen.Splash.SetHeader("профілі");
            SplashScreen.Splash.SetStatus("завантажуємо...");

            Profile[] profiles = ProfilesController.GetProfiles(Settings.Default.ProfilesPath);

            SplashScreen.Splash.SetStatus("ок!");

            var defProfileName = Settings.Default.DefaultProfile;

            this.InvokeIfNeeded(() =>
            {
                foreach (var profile in profiles)
                {

                    CreateProfileTab(profile);

                }
            });
        }
        /// <summary>
        /// Creates a new profile tab in the ribbon and initializes the associated profile form.
        /// </summary>
        /// <remarks>This method adds a new tab to the ribbon control, initializes a corresponding profile
        /// form,  and associates the form with the tab. The profile form is displayed as an MDI child of the current
        /// window.</remarks>
        /// <param name="profile">The profile object containing the settings and data to be displayed in the tab and form.</param>
        private void CreateProfileTab(JobSpace.Profiles.Profile profile)
        {
            var tab = new KryptonRibbonTab { Text = profile.Settings.ProfileName };
            kryptonRibbon1.RibbonTabs.Add(tab);

            var formProfile = new FormProfile
            {
                Tag = profile,
                Text = profile.Settings.ProfileName,
                MdiParent = this
            };

            SplashScreen.Splash.SetHeader(profile.Settings.ProfileName, ContentAlignment.TopRight);
            SplashScreen.Splash.SetStatus("завантажуємо налаштування...");

            formProfile.InitProfile();

            formProfile.Dock = DockStyle.Fill;
            formProfile.Show();

            tab.Tag = formProfile;
            _profileTabs.Add(formProfile);

            FillRibbonTab(formProfile, tab, profile);
        }

        private void KryptonRibbon1_SelectedTabChanged(object sender, EventArgs e)
        {
            var formProfile = (FormProfile)((KryptonRibbon)sender)?.SelectedTab?.Tag;
            formProfile?.Activate();
            _activeProfileTab = formProfile;
        }
    }
}
