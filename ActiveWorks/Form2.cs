using ActiveWorks.Forms;
using ActiveWorks.Properties;
using ActiveWorks.UserControls;
using Amazon;
using ExtensionMethods;
using Interfaces;
using Interfaces.Plugins;
using JobSpace.Fasades;
using JobSpace.Profiles;
using JobSpace.UserForms;
using Krypton.Ribbon;
using Krypton.Toolkit;
using Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Profile = JobSpace.Profiles.Profile;

namespace ActiveWorks
{
    public sealed partial class Form2 : KryptonForm
    {
        List<FormProfile> _profileTabs {get;set; } = new List<FormProfile>();
        FormProfile _activeProfileTab { get; set; }

        FormBackgroundTasks _formBackgroundTask;
        readonly Stopwatch _sw = new Stopwatch();


        public Form2()
        {
            InitializeComponent();
            kryptonRibbon1.AllowFormIntegrate = false;

            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Text = $"{Localize.FormTitle} {assemblyVersion}";

            buttonSpecAnyWhatNew.Click += ButtonSpecAnyWhatNew_Click;
            buttonSpecAnyIssue.Click += ButtonSpecAnyIssue_Click;
            buttonSpecBackgroundTasks.Click += ButtonSpecBackgroundTasks_Click;

            BackgroundTaskServiceLib.BackgroundTaskService.OnAdd += BackgroundTaskService_OnAdd;
            BackgroundTaskServiceLib.BackgroundTaskService.OnAllFinish += BackgroundTaskService_OnAllFinish;

            _sw.Start();

            SplashScreen.Splash.ShowSplashScreen();
            SplashScreen.Splash.SetImage(Resources.SplashScreen8);
            SplashScreen.Splash.SetVersion(assemblyVersion, Color.Yellow, 12, 12);
            SplashScreen.Splash.SetHeader(string.Empty);
            SplashScreen.Splash.SetStatus(string.Empty);
        }

        private void ButtonSpecAnyIssue_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/garymiller30/work/issues");
        }

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

            this.InvokeIfNeeded(() => {
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
            _activeProfileTab = formProfile;
        }

        private void FillRibbonTab(IFormProfile formProfile, KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
        {
            CreateOrderGroup(tab, profile);
            CreateStatusesGroup(tab, profile);
            CreateViewFilterGroup(tab, profile);
            CreateSearchGroup(tab, profile);
            CreateSettingsGroup(formProfile, tab, profile);
            CreateServiceStateGroup(tab,profile);
        }

        private void CreateServiceStateGroup(KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
        {
            var group = new KryptonRibbonGroup
            {
                TextLine1 = @"Стан сервісів",
                MinimumWidth = 100,
            };
            // сюди будуть додаватися кнопки стану сервісів
            var groupTriple = new KryptonRibbonGroupLines();

            foreach (IServiceState service in profile.ServicesState.GetAll())
            {
                var button = new KryptonRibbonGroupButton
                {
                    TextLine1 = service.Name,
                    ImageSmall = profile.ServicesState.GetImage(service),
                    Tag = service
                };
                button.ToolTipValues.Heading = $"Статус {service.Name}";
                button.ToolTipValues.Description = service.Tooltip;
                button.ToolTipValues.EnableToolTips = true;
                button.Click += (sender, args) =>
                {
                    MessageBox.Show(service.Description, service.Name, MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                };
                groupTriple.Items.Add(button);
            }

            group.Items.Add(groupTriple);
            tab.Groups.Add(group);

            // підписка на події зміни стану сервісів
            profile.Events.ServiceStateEvents.AddServiceState += (s, e) =>
            {
                this.InvokeIfNeeded(new Action(() =>
                {
                    var button = new KryptonRibbonGroupButton
                    {
                        TextLine1 = e.Name,
                        ImageSmall = profile.ServicesState.GetImage(e),
                        Tag = e
                    };

                    button.ToolTipValues.Heading = e.Tooltip;

                    button.Click += (sender, args) =>
                    {
                        MessageBox.Show(e.Description, e.Name, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    };
                    groupTriple.Items.Add(button);
                }));
            };

            profile.Events.ServiceStateEvents.UpdateServiceState += (s, e) =>
            {
                this.InvokeIfNeeded(new Action(() =>
                {
                    var button = groupTriple.Items.OfType<KryptonRibbonGroupButton>()
                        .FirstOrDefault(x => ((IServiceState)x.Tag).Id == e.Id);
                    if (button != null)
                    {
                        button.ImageSmall = profile.ServicesState.GetImage(e);
                        button.TextLine1 = e.Name;
                        button.ToolTipValues.Description = e.Tooltip;
                    }
                }));
            };
        }



        private void CreateSettingsGroup(IFormProfile formProfile, KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
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
                using (var s = new FormSettings((JobSpace.Profiles.Profile)(_activeProfileTab)?.Tag))
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
                CreateProfileTab((JobSpace.Profiles.Profile)((FormProfile)formProfile).Tag);
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
                ((JobSpace.Profiles.Profile)tab.Tag).FileBrowser.InitBrowserToolStripUtils();
            }
        }

        private void CreateSearchGroup(KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
        {
            if (profile.Customers is null) return;

           
            

            var group = new KryptonRibbonGroup
            {
                TextLine1 = @"Пошук",
                MinimumWidth = 100,
                AllowCollapsed = false
            };

            tab.Groups.Add(group);

            var triple1 = new KryptonRibbonGroupTriple { ItemSizeMaximum = GroupItemSize.Small };

            group.Image = Resources.Binoculars_icon;
            group.Items.Add(triple1);

            var cb_customers = new KryptonRibbonGroupComboBox();
            // додати cb_searchStr для фільтру по замовникам
            
            cb_customers.ComboBox.ToolTipValues.Heading = @"Фільтр по замовнику";
            cb_customers.ComboBox.ToolTipValues.Description = @"Виберіть замовника";
            cb_customers.ComboBox.ToolTipValues.EnableToolTips = true;

            AutoCompleteStringCollection customer_data = new AutoCompleteStringCollection();

            string[] customers = CreateCustomersList(profile);

            if (customers.Length > 0) customer_data.AddRange(customers);

            cb_customers.ComboBox.Items.AddRange(customers);
            cb_customers.ComboBox.SelectedIndex = -1;
            cb_customers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cb_customers.AutoCompleteSource = AutoCompleteSource.CustomSource;
            cb_customers.AutoCompleteCustomSource = customer_data;

            var btn_clearCustomers = new ButtonSpecAny
            {
                Style = PaletteButtonStyle.Standalone,
                Type = PaletteButtonSpecStyle.Close
            };

            btn_clearCustomers.Click += (sender, args) =>
            {
                cb_customers.Text = string.Empty;
            };

            cb_customers.ButtonSpecs.Add(btn_clearCustomers);

            void OnCustomersChanged(JobSpace.Customer s)
            {
                string[] cadd = CreateCustomersList(profile);
                customer_data.Clear();
                if (cadd.Length > 0) customer_data.AddRange(cadd);
                cb_customers.ComboBox.Items.Clear();
                cb_customers.ComboBox.Items.AddRange(cadd);
                cb_customers.ComboBox.SelectedIndex = -1;
            }
            CustomerManager customersManager = (CustomerManager)profile.Customers;
            customersManager.OnCustomerAdd += OnCustomersChanged;
            customersManager.OnCustomerChange += OnCustomersChanged;
            customersManager.OnCustomerRemove += OnCustomersChanged;

            triple1.Items.Add(cb_customers);



            // додати cb_searchStr для пошуку по базі
            var cb_searchStr = new KryptonRibbonGroupComboBox();
            cb_searchStr.ComboBox.ToolTipValues.Description = @"Введіть слово і натисніть <Enter> для пошуку";
            cb_searchStr.ComboBox.ToolTipValues.Image = Resources.Sql_runner_icon;
            cb_searchStr.ComboBox.ToolTipValues.Heading = @"Пошук по базі";
            cb_searchStr.ComboBox.ToolTipValues.EnableToolTips = true;

            AutoCompleteStringCollection data = new AutoCompleteStringCollection();

            if (profile.Categories != null)
            {
                data.AddRange(profile.Categories.GetAll().Select(x => x.Name).ToArray());
            }
            cb_searchStr.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cb_searchStr.AutoCompleteSource = AutoCompleteSource.CustomSource;

            cb_searchStr.AutoCompleteCustomSource = data;

            if (profile.SearchHistory != null)
                cb_searchStr.Items.AddRange(profile.SearchHistory.GetHistory());

            var clearButton = new ButtonSpecAny
            {
                Style = PaletteButtonStyle.Standalone,
                Type = PaletteButtonSpecStyle.Close
            };
            clearButton.Click += (sender, args) =>
            {
                //profile.SearchHistory.Add(cb_searchStr.Text);
                cb_searchStr.Text = string.Empty;
                cb_searchStr.Sorted = false;

                SetHistoryList(profile, cb_searchStr);

            };

            cb_searchStr.ButtonSpecs.Add(clearButton);
            triple1.Items.Add(cb_searchStr);

            var textBox = new KryptonRibbonGroupTextBox();
            textBox.TextBox.ToolTipValues.EnableToolTips = true;
            textBox.TextBox.ToolTipValues.Heading = @"Фільтр у списку робіт";
            textBox.TextBox.ToolTipValues.Description = @"фільтрує у поточному списку робіт";
            textBox.TextBox.ToolTipValues.Image = Properties.Resources.text_list_bullets_icon;

            textBox.TextChanged += (sender, args) =>
            {
                profile.Jobs.JobListControl.ApplyViewListFilterText(textBox.Text);
            };
            var clearTextBoxBtn = new ButtonSpecAny
            {
                Style = PaletteButtonStyle.Standalone,
                Type = PaletteButtonSpecStyle.Close
            };
            clearTextBoxBtn.Click += (sender, args) =>
            {
                textBox.Text = string.Empty;
                profile.Jobs.JobListControl.ApplyViewListFilterText(textBox.Text);
            };
            textBox.ButtonSpecs.Add(clearTextBoxBtn);
            triple1.Items.Add(textBox);

            var groupLines = new KryptonRibbonGroupLines { ItemSizeMaximum = GroupItemSize.Small };

            var statuses = profile.SearchManager.GetStatuses();
            foreach (var status in statuses)
            {
                IJobStatus s = status.Key;
                var button = new KryptonRibbonGroupButton()
                {
                    TextLine1 = s.Name,
                    ImageSmall = s.Img,
                    ButtonType = GroupButtonType.Check,
                    Tag = status
                };
                button.Checked = status.Value;

                button.Click += (sender, args) =>
                {
                    profile.SearchManager.ChangeStatus(s, ((KryptonRibbonGroupButton)sender).Checked);

                };

                groupLines.Items.Add(button);
            }
            group.Items.Add(groupLines);

            var triple2 = new KryptonRibbonGroupTriple { ItemSizeMaximum = GroupItemSize.Large };
            // додати кнопку для пошуку по базі даних
            var btnSearch = new KryptonRibbonGroupButton
            {
                TextLine1 = @"Пошук",
                ImageSmall = Resources.Omercetin_Pixelophilia_Search_32,
                ImageLarge = Resources.Omercetin_Pixelophilia_Search_32
            };

            btnSearch.Click += (sender, args) =>
            {
                profile.SearchManager.Search(cb_customers.Text, cb_searchStr.Text);
                SetHistoryList(profile, cb_searchStr);
            };

            triple2.Items.Add(btnSearch);

            // додати кнопку для скидання фільтрів
            var btnReset = new KryptonRibbonGroupButton
            {
                TextLine1 = @"Скинути",
                ImageSmall = Resources.Fatcow_Farm_Fresh_Filter_clear_32,
                ImageLarge = Resources.Fatcow_Farm_Fresh_Filter_clear_32
            };

            btnReset.Click += (sender, args) =>
            {
                cb_customers.Text = string.Empty;
                cb_searchStr.Text = string.Empty;
                textBox.Text = string.Empty;
                profile.SearchManager.ClearFilters();

            };

            triple2.Items.Add(btnReset);

            group.Items.Add(triple2);
        }

        private static string[] CreateCustomersList(JobSpace.Profiles.Profile profile)
        {
            var customerList = new List<string>(profile.Customers.Count());

            foreach (var customer in profile.Customers)
            {
                if (customer.Show)
                    customerList.Add(customer.Name);
            }
            var customers = customerList.ToArray();
            return customers;
        }

        private static void SetHistoryList(JobSpace.Profiles.Profile profile, KryptonRibbonGroupComboBox cb_searchStr)
        {
            cb_searchStr.Items.Clear();
            cb_searchStr.Items.AddRange(profile.SearchHistory.GetHistory());
        }

        private void CreateViewFilterGroup(KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
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

            foreach (IJobStatus status in profile.StatusManager.GetJobStatuses())
            {
                var button = new KryptonRibbonGroupButton()
                {
                    TextLine1 = status.Name,
                    ImageSmall = status.Img,
                    ButtonType = GroupButtonType.Check,
                    Tag = status
                };
                button.Click += (sender, args) =>
                {
                    IJobStatus assignedStatus = (IJobStatus)((KryptonRibbonGroupButton)sender).Tag;

                    profile.StatusManager.ViewStatusChecked(assignedStatus, ((KryptonRibbonGroupButton)sender).Checked);

                    profile.Jobs.JobListControl.ApplyViewListFilterStatuses(profile.StatusManager.GetEnabledViewStatuses());
                };
                button.Checked = profile.StatusManager.IsViewStatusChecked(status.Code);
                groupLines.Items.Add(button);
            }

        }

        private void CreateStatusesGroup(KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
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


        private void CreateOrderGroup(KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
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
                   
                };
                button.Click += (sender, args) => profile.Jobs.CreateJob(plugins[0]);
                groupTriple.Items.Add(button);
            }

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

            // Save settings
            Settings.Default.Save();
        }

        private void kryptonRibbon1_SelectedTabChanged_1(object sender, EventArgs e)
        {

        }
    }
}
