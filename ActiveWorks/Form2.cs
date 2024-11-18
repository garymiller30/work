using ActiveWorks.Forms;
using ActiveWorks.Properties;
using ActiveWorks.UserControls;
using Krypton.Ribbon;
using Krypton.Toolkit;
using ExtensionMethods;
using Interfaces;
using Interfaces.Plugins;
using JobSpace.Profiles;
using JobSpace.UserForms;
using Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ActiveWorks
{
    public sealed partial class Form2 : KryptonForm
    {
        private readonly string _version = $"{Localize.FormTitle} 8.19.18";
        readonly List<FormProfile> _profileTabs = new List<FormProfile>();

        FormBackgroundTasks _formBackgroundTask;
        readonly Stopwatch _sw = new Stopwatch();


        public Form2()
        {
            InitializeComponent();
            kryptonRibbon1.AllowFormIntegrate = false;
            
            Text = _version;

            buttonSpecAnyWhatNew.Click += ButtonSpecAnyWhatNew_Click;
            buttonSpecAnyIssue.Click += ButtonSpecAnyIssue_Click;
            buttonSpecBackgroundTasks.Click += ButtonSpecBackgroundTasks_Click;

            BackgroundTaskServiceLib.BackgroundTaskService.OnAdd += BackgroundTaskService_OnAdd;
            BackgroundTaskServiceLib.BackgroundTaskService.OnAllFinish += BackgroundTaskService_OnAllFinish;

            _sw.Start();

            SplashScreen.Splash.ShowSplashScreen();
            SplashScreen.Splash.SetImage(Resources.SplashScreen8);
            SplashScreen.Splash.SetVersion(_version, Color.Yellow, 12, 12);
            SplashScreen.Splash.SetHeader(string.Empty);
            SplashScreen.Splash.SetStatus(string.Empty);
        }

        private void ButtonSpecAnyIssue_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/garymiller30/work/issues");
        }

        //private void ThemeController_ThemeChanged(object sender, EventArgs e)
        //{
        //    kryptonManager1.GlobalPaletteMode = ThemeController.Theme == ThemeEnum.Light ? PaletteMode.Office2010Silver : PaletteMode.SparkleBlueDarkMode;
        //}

        private void BackgroundTaskService_OnAllFinish(object sender, EventArgs e)
        {
            this.InvokeIfNeeded(new Action(() => buttonSpecBackgroundTasks.ExtraText = ""));
        }


        private void BackgroundTaskService_OnAdd(object sender, BackgroundTaskServiceLib.BackgroundTaskItem e)
        {
            this.InvokeIfNeeded(new Action(() => buttonSpecBackgroundTasks.ExtraText = "(...працюємо...)"));
        }

        private void ButtonSpecBackgroundTasks_Click(object sender, EventArgs e)
        {
            if (_formBackgroundTask == null)
            {
                _formBackgroundTask = new FormBackgroundTasks();
                _formBackgroundTask.FormClosed += (s, ev) => { _formBackgroundTask = null; };
                _formBackgroundTask.Show();
            }
            else
            {
                _formBackgroundTask.Activate();
            }
        }


        private void ButtonSpecAnyWhatNew_Click(object sender, EventArgs e)
        {
            using (var form = new FormWhatNew(Settings.Default.WhatNew))
            {
                form.ShowDialog();
            }
        }

        private void SetActiveDefaultProfile()
        {
            var defProfileName = Settings.Default.DefaultProfile;
            var profile =
                _profileTabs.FirstOrDefault(x => ((Profile)x.Tag).Settings.ProfileName.Equals(defProfileName));

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
            formProfile.Activate();
        }
        /// <summary>
        /// Create Profile Ribbon  Tab
        /// </summary>
        private void CreateProfilesTab()
        {
            SplashScreen.Splash.SetHeader("профілі");
            SplashScreen.Splash.SetStatus("завантажуємо...");

            var profiles = ProfilesController.GetProfiles(Settings.Default.ProfilesPath);

            if (profiles.Length == 0)
            {
                //потрібно створити типчасовий профіль
                profiles = new[] {ProfilesController.AddProfile()}; 
            }

            SplashScreen.Splash.SetStatus("ок!");
            foreach (var profile in profiles)
            {
                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();

                CreateProfileTab(profile);
                
                sw.Stop();
                Log.Info("App", "App", $"profile '{profile.Settings.ProfileName}' loaded by {sw.ElapsedMilliseconds} ms");
            }
        }

        private void CreateProfileTab(Profile profile)
        {
            var tab = new KryptonRibbonTab { Text = profile.Settings.ProfileName };
            kryptonRibbon1.RibbonTabs.Add(tab);

            var formProfile = new FormProfile
            {
                Tag = profile,
                Text = profile.Settings.ProfileName,
                MdiParent = this
            };

            SplashScreen.Splash.SetHeader(profile.Settings.ProfileName,ContentAlignment.TopRight);
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
        }

        private void FillRibbonTab(IFormProfile formProfile, KryptonRibbonTab tab, Profile profile)
        {
            CreateOrderGroup(tab, profile);
            CreateStatusesGroup(tab, profile);
            CreateViewFilterGroup(tab, profile);
            CreateSearchGroup(tab, profile);
            CreateSettingsGroup(formProfile, tab, profile);
        }

        private void CreateSettingsGroup(IFormProfile formProfile, KryptonRibbonTab tab, Profile profile)
        {
            // -----------------------------------------------
            var group = new KryptonRibbonGroup
            {
                TextLine1 = @"Налаштування",
                Image = Resources.Apps_preferences_icon,
                MinimumWidth = 100
            };
            tab.Groups.Add(group);

            var groupTriple = new KryptonRibbonGroupTriple();
            var button = new KryptonRibbonGroupButton
            {
                TextLine1 = @"Основні",
                ImageLarge = Resources.Apps_preferences_icon,
                ImageSmall = Resources.Apps_preferences_icon

            };
            button.Click += (sender, args) =>
            {
                using (var s = new FormSettings())
                {
                    s.ShowDialog();
                    ApplySettings();
                }
            };
            groupTriple.Items.Add(button);

            // -----------------------------------------------
            button = new KryptonRibbonGroupButton
            {
                TextLine1 = @"Замовники",
                ImageLarge = Resources.user_settings_icon_big,
                ImageSmall = Resources.user_settings_icon_small
            };
            button.Click += (sender, args) =>
            {
                using (var c = new FormCustomers(profile))
                {
                    c.ShowDialog();
                }
            };
            groupTriple.Items.Add(button);

            // -----------------------------------------------
            button = new KryptonRibbonGroupButton
            {
                TextLine1 = @"Логи",
                ImageLarge = Resources.file_extension_log_icon,
                ImageSmall = Resources.file_extension_log_icon
            };
            button.Click += (sender, args) =>
            {
                Log.ShowWindow();
            };
            groupTriple.Items.Add(button);


            group.Items.Add(groupTriple);
            // -----------------------------------------------
            groupTriple = new KryptonRibbonGroupTriple();

            button = new KryptonRibbonGroupButton
            {
                TextLine1 = @"Save Layout",
                ImageLarge = Resources.window_layout_icon,
                ImageSmall = Resources.window_layout_icon_small
            };
            button.Click += (sender, args) =>
            {
                formProfile.SaveLayout();
            };
            groupTriple.Items.Add(button);
            button = new KryptonRibbonGroupButton
            {
                TextLine1 = @"Reset Layout",
                ImageLarge = Resources.layout_reset_window_icon,
                ImageSmall = Resources.layout_reset_window_icon
            };
            button.Click += (sender, args) =>
            {
                formProfile.ResetLayout();
                _profileTabs.Remove((FormProfile)formProfile);
                kryptonRibbon1.RibbonTabs.Remove(tab);
                CreateProfileTab((Profile)((FormProfile)formProfile).Tag);
            };
            groupTriple.Items.Add(button);
            //// --- Theme switcher button ---
            //button = new KryptonRibbonGroupButton { TextLine1 = @"Змінити тему"};
            //button.Click += (sender, args) => ThemeController.SwitchTheme();
            //groupTriple.Items.Add(button);
            group.Items.Add(groupTriple);

        }

        private void ApplySettings()
        {
            foreach (var tab in _profileTabs)
            {
                ((Profile)tab.Tag).FileBrowser.InitBrowserToolStripUtils();
            }
        }

        private void CreateSearchGroup(KryptonRibbonTab tab, Profile profile)
        {
            var group = new KryptonRibbonGroup
            {
                TextLine1 = @"Пошук",
                MinimumWidth = 100,
                AllowCollapsed = false


            };

            tab.Groups.Add(group);

            var triple = new KryptonRibbonGroupTriple { ItemSizeMaximum = GroupItemSize.Small };

            group.Image = Resources.Binoculars_icon;
            group.Items.Add(triple);

            var combobox = new KryptonRibbonGroupComboBox();
            combobox.ComboBox.ToolTipValues.Description = @"Введіть слово і натисніть <Enter> для пошуку";
            combobox.ComboBox.ToolTipValues.Image = Resources.Sql_runner_icon;
            combobox.ComboBox.ToolTipValues.Heading = @"Пошук по базі";
            combobox.ComboBox.ToolTipValues.EnableToolTips = true;


            AutoCompleteStringCollection data = new AutoCompleteStringCollection();
            if (profile.Customers != null)
                data.AddRange(profile.Customers.Select(x => x.Name).ToArray());
            if (profile.Categories != null)
            {
                data.AddRange(profile.Categories.GetAll().Select(x => x.Name).ToArray());
            }
            combobox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            combobox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            combobox.AutoCompleteCustomSource = data;

            if (profile.SearchHistory != null)
                combobox.Items.AddRange(profile.SearchHistory.GetHistory());

            combobox.KeyDown += (sender, args) =>
            {

                if (args.KeyCode == Keys.Enter)
                {
                    profile.Jobs.JobListControl.Search(combobox.Text);
                }
            };
            combobox.TextUpdate += (sender, args) =>
            {
                if (string.IsNullOrEmpty(combobox.Text))
                    profile.Jobs.JobListControl.ApplyViewFilter();
            };
            combobox.SelectedIndexChanged += (sender, args) =>
            {
                profile.Jobs.JobListControl.Search(combobox.Text);
            };

            var clearButton = new ButtonSpecAny
            {
                Style = PaletteButtonStyle.Standalone,
                Type = PaletteButtonSpecStyle.Close
            };
            clearButton.Click += (sender, args) =>
            {
                profile.SearchHistory.Add(combobox.Text);
                combobox.Text = string.Empty;
                combobox.Items.Clear();
                combobox.Items.AddRange(profile.SearchHistory.GetHistory());
                profile.Jobs.JobListControl.ApplyViewFilter();
            };

            combobox.ButtonSpecs.Add(clearButton);
            triple.Items.Add(combobox);

            var textBox = new KryptonRibbonGroupTextBox();
            textBox.TextBox.ToolTipValues.EnableToolTips = true;
            textBox.TextBox.ToolTipValues.Heading = @"Фільтр у списку робіт";
            textBox.TextBox.ToolTipValues.Description = @"фільтрує у поточному списку робіт";
            textBox.TextBox.ToolTipValues.Image = Properties.Resources.text_list_bullets_icon;

            textBox.TextChanged += (sender, args) =>
            {
                profile.Jobs.JobListControl.ApplyJobListFilter(textBox.Text);
            };
            var clearTextBoxBtn = new ButtonSpecAny
            {
                Style = PaletteButtonStyle.Standalone,
                Type = PaletteButtonSpecStyle.Close
            };
            clearTextBoxBtn.Click += (sender, args) =>
            {
                textBox.Text = string.Empty;
                profile.Jobs.JobListControl.ApplyJobListFilter(textBox.Text);
            };
            textBox.ButtonSpecs.Add(clearTextBoxBtn);
            triple.Items.Add(textBox);

            var dateSelect = new KryptonRibbonGroupDateTimePicker
            {
                MinimumSize = new Size(120, 0),
                MaximumSize = new Size(120, 0)
            };

            dateSelect.CloseUp += (sender, args) => profile.Jobs.JobListControl.ApplyViewFilter(dateSelect.DateTimePicker.Value);

            triple.Items.Add(dateSelect);

        }

        private void CreateViewFilterGroup(KryptonRibbonTab tab, Profile profile)
        {
            if (profile.StatusManager == null) return;

            var group = new KryptonRibbonGroup
            {
                TextLine1 = @"Перегляд",
                MinimumWidth = 100,
                Image = Resources.Glasses_icon,


            };
            tab.Groups.Add(group);
            var groupLines = new KryptonRibbonGroupLines { ItemSizeMaximum = GroupItemSize.Small };
            group.Items.Add(groupLines);

            // завантажуємо фільтри статусів
            //Dictionary<int, bool> dic = profile.StatusManager.GetViewStatuses();


            foreach (IJobStatus status in profile.StatusManager.GetJobStatuses())
            {


                var button = new KryptonRibbonGroupButton()
                {
                    
                    //ToolTipStyle = LabelStyle.ToolTip,
                    //ToolTipBody = status.Name,
                    TextLine1 = status.Name,
                    ImageSmall = status.Img,
                    ButtonType = GroupButtonType.Check,
                    Tag = status
                };



                button.Click += (sender, args) =>
                {
                    IJobStatus assignedStatus = (IJobStatus)((KryptonRibbonGroupButton)sender).Tag;

                    profile.StatusManager.ViewStatusChecked(assignedStatus, ((KryptonRibbonGroupButton)sender).Checked);
                    profile.Jobs.JobListControl.ApplyViewFilter();
                };
                button.Checked = profile.StatusManager.IsViewStatusChecked(status.Code);

                groupLines.Items.Add(button);
            }

        }

        private void CreateStatusesGroup(KryptonRibbonTab tab, Profile profile)
        {

            if (profile.StatusManager == null) return;

            var group = new KryptonRibbonGroup
            {
                TextLine1 = @"Змінити статус",
                MinimumWidth = 100,
                Image = Resources.move_icon
            };
            var groupTriple = new KryptonRibbonGroupLines();
            

            foreach (IJobStatus status in profile.StatusManager.GetJobStatuses())
            {
                var button = new KryptonRibbonGroupButton
                {
                    TextLine1 = status.Name,
                    ImageSmall = status.Img,
                    Tag = status
                };
                button.Click += (sender, args) =>
                {
                    profile.Jobs.JobListControl.ChangeSelectedJobsStatus((IJobStatus)((KryptonRibbonGroupButton)sender).Tag);
                };
                groupTriple.Items.Add(button);
            }
            group.Items.Add(groupTriple);
            tab.Groups.Add(group);
        }


        private void CreateOrderGroup(KryptonRibbonTab tab, Profile profile)
        {
            var group = new KryptonRibbonGroup
            {
                TextLine1 = @"Замовлення",
                Image = Resources.addnewjob,
                MinimumWidth = 100
            };

            var groupTriple = new KryptonRibbonGroupTriple
            {
                MinimumSize = GroupItemSize.Large
            };
            var button = new KryptonRibbonGroupButton { TextLine1 = @"нове", ImageLarge = Resources.File_new_icon };

            
            void OnJobAdd(object sender,IJob job)
            {
                profile.Jobs.JobListControl.SelectJob(job);
            }

            button.Click += (sender, args) => {
                
                profile.Events.Jobs.OnJobAdd += OnJobAdd;
                profile.Jobs.CreateJob();
                profile.Events.Jobs.OnJobAdd -= OnJobAdd;
                };
            groupTriple.Items.Add(button);

            IPluginNewOrder[] plugins = profile.Plugins?.GetPluginsNewOrder() ?? new IPluginNewOrder[0];
            if (plugins.Any())
            {
                button = new KryptonRibbonGroupButton
                {
                    TextLine1 = plugins[0].PluginName,
                    ImageLarge = plugins[0].PluginImage,

                    //ToolTipBody = plugins[0].PluginDescription 
                };
                button.Click += (sender, args) => profile.Jobs.CreateJob(plugins[0]);
                groupTriple.Items.Add(button);
            }

            //button = new KryptonRibbonGroupButton { TextLine1 = @"повторити", ImageLarge = Resources.Document_copy_icon };
            //button.Click += (sender, args) => profile.Jobs.RepeatSelectedJob();
            //groupTriple.Items.Add(button);
            group.Items.Add(groupTriple);
            tab.Groups.Add(group);
        }

        

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
            // Copy window size to app settings
            //Settings.Default.WindowSize =
            //    WindowState == FormWindowState.Normal ?
            //    Size : RestoreBounds.Size;

            // Save settings
            Settings.Default.Save();
        }
    }
}
