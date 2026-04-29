using ActiveWorks.Forms;
using ActiveWorks.Properties;
using ActiveWorks.UserControls;
using ExtensionMethods;
using Interfaces;
using Interfaces.Plugins;
using JobSpace.Fasades;
using JobSpace.UserForms;
using Krypton.Ribbon;
using Krypton.Toolkit;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ActiveWorks
{
    public sealed partial class Form2
    {
        private void FillRibbonTab(IFormProfile formProfile, KryptonRibbonTab tab, JobSpace.Profiles.Profile profile)
        {
            CreateOrderGroup(tab, profile);
            CreateStatusesGroup(tab, profile);
            CreateViewFilterGroup(tab, profile);
            CreateSearchGroup(tab, profile);
            CreateSettingsGroup(formProfile, tab, profile);
            CreateServiceStateGroup(tab, profile);
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
                groupTriple.Items.Add(CreateServiceStateButton(profile, service));
            }

            group.Items.Add(groupTriple);
            tab.Groups.Add(group);

            // підписка на події зміни стану сервісів
            profile.Events.ServiceStateEvents.AddServiceState += (s, e) =>
            {
                this.InvokeIfNeeded(new Action(() =>
                {
                    groupTriple.Items.Add(CreateServiceStateButton(profile, e));
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
                ImageSmall = Resources.Apps_preferences_icon,
                ButtonType = GroupButtonType.DropDown
            };

            var settingsMenu = new KryptonContextMenu();

            settingsMenu.Items.Add(new KryptonContextMenuItems(new KryptonContextMenuItemBase[]
            {
                new KryptonContextMenuItem(@"Основні", Resources.Apps_preferences_icon, (sender, args) =>
                {
                    using (var s = new FormSettings((JobSpace.Profiles.Profile)(_activeProfileTab)?.Tag))
                    {
                        s.ShowDialog();
                        ApplySettings();
                    }
                }),
                new KryptonContextMenuItem(@"Замовники", Resources.user_settings_icon_big, (sender, args) =>
                {
                    using (var c = new FormCustomers(profile))
                    {
                        c.ShowDialog();
                    }
                }),
                new KryptonContextMenuItem(@"Логи", Resources.file_extension_log_icon, (sender, args) =>
                {
                    Log.ShowWindow();
                }),
                new KryptonContextMenuSeparator(),
                new KryptonContextMenuItem(@"Save Layout", Resources.window_layout_icon_small, (sender, args) =>
                {
                    formProfile.SaveLayout();
                }),
                new KryptonContextMenuItem(@"Reset Layout", Resources.layout_reset_window_icon, (sender, args) =>
                {
                    formProfile.ResetLayout();
                    _profileTabs.Remove((FormProfile)formProfile);
                    kryptonRibbon1.RibbonTabs.Remove(tab);
                    CreateProfileTab((JobSpace.Profiles.Profile)((FormProfile)formProfile).Tag);
                })
            }));

            button.KryptonContextMenu = settingsMenu;
            groupTriple.Items.Add(button);
            group.Items.Add(groupTriple);

        }

        private void ApplySettings()
        {
            foreach (var tab in _profileTabs)
            {
                // примінити налаштування шрифту для JobList
                ((JobSpace.Profiles.Profile)tab.Tag).Jobs.JobListControl.ApplyJobListFontSettings();
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

            cb_customers.ButtonSpecs.Add(CreateClearButton((sender, args) => cb_customers.Text = string.Empty));

            void OnCustomersChanged(JobSpace.Customer s)
            {
                RefreshCustomersList(profile, cb_customers, customer_data);
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

            cb_searchStr.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                {
                    profile.SearchManager.Search(cb_customers.Text, cb_searchStr.Text);
                    SetHistoryList(profile, cb_searchStr);
                }
            };

            cb_searchStr.ButtonSpecs.Add(CreateClearButton((sender, args) =>
            {
                cb_searchStr.Text = string.Empty;
                cb_searchStr.Sorted = false;

                SetHistoryList(profile, cb_searchStr);
            }));
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
            textBox.ButtonSpecs.Add(CreateClearButton((sender, args) =>
            {
                textBox.Text = string.Empty;
                profile.Jobs.JobListControl.ApplyViewListFilterText(textBox.Text);
            }));
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
                    Tag = status,
                    ToolTipValues = {
                        Heading = $"Показувати статус \"{s.Name}\" у пошуку",
                        Description = $"Увімкніть, щоб шукати роботи зі статусом \"{s.Name}\".",
                        EnableToolTips = true,
                        Image = s.Img
                    }
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
                ImageLarge = Resources.Omercetin_Pixelophilia_Search_32,
                ToolTipValues = {
                    Heading = @"Пошук по базі",
                    Description = @"Натисніть, щоб виконати пошук по базі з урахуванням вибраного замовника та введеного тексту.",
                    EnableToolTips = true,
                    Image = Resources.Omercetin_Pixelophilia_Search_32
                }

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
                ImageLarge = Resources.Fatcow_Farm_Fresh_Filter_clear_32,
                ToolTipValues = {
                    Heading = @"Скинути фільтри",
                    Description = @"Натисніть, щоб скинути всі фільтри.",
                    EnableToolTips = true,
                    Image = Resources.Fatcow_Farm_Fresh_Filter_clear_32
                }
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

        private static ButtonSpecAny CreateClearButton(EventHandler onClick)
        {
            var button = new ButtonSpecAny
            {
                Style = PaletteButtonStyle.Standalone,
                Type = PaletteButtonSpecStyle.Close
            };

            button.Click += onClick;
            return button;
        }

        private static KryptonRibbonGroupButton CreateServiceStateButton(JobSpace.Profiles.Profile profile, IServiceState service)
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
            button.Click += (sender, args) => ShowServiceStateInfo(service);

            return button;
        }

        private static void ShowServiceStateInfo(IServiceState service)
        {
            MessageBox.Show(service.Description, service.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void RefreshCustomersList(
            JobSpace.Profiles.Profile profile,
            KryptonRibbonGroupComboBox customersComboBox,
            AutoCompleteStringCollection customerData)
        {
            string[] customers = CreateCustomersList(profile);
            customerData.Clear();
            if (customers.Length > 0) customerData.AddRange(customers);
            customersComboBox.ComboBox.Items.Clear();
            customersComboBox.ComboBox.Items.AddRange(customers);
            customersComboBox.ComboBox.SelectedIndex = -1;
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
                    Tag = status,
                    ToolTipValues = {
                        Heading = $"Показувати статус \"{status.Name}\"",
                        Description = $"Увімкніть, щоб показувати роботи зі статусом \"{status.Name}\" у списку робіт.",
                        EnableToolTips = true,
                        Image = status.Img
                    }
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
                MinimumWidth = 100,
            };

            var groupTriple = new KryptonRibbonGroupTriple
            {
                MinimumSize = GroupItemSize.Large
            };
            var button = new KryptonRibbonGroupButton
            {
                TextLine1 = @"нове",
                ImageLarge = Resources.File_new_icon,
                ToolTipValues = {
                    Heading = @"Створити нове замовлення",
                    Description = @"Натисніть, щоб створити нове замовлення. Утримуйте Shift для створення кількох замовлень поспіль.",
                    EnableToolTips = true,
                    Image = Resources.File_new_icon
                }
            };


            void OnJobAdd(object sender, IJob job)
            {
                profile.Jobs.JobListControl.SelectJob(job);
            }

            button.Click += (sender, args) =>
            {
                if (Control.ModifierKeys.HasFlag(Keys.Shift))
                {
                    profile.Jobs.CreateManyJobsWithDialog();
                }
                else
                {
                    profile.Events.Jobs.OnJobAdd += OnJobAdd;
                    profile.Jobs.CreateJob();
                    profile.Events.Jobs.OnJobAdd -= OnJobAdd;

                }
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
    }
}
