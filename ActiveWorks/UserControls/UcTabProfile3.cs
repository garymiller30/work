﻿using ComponentFactory.Krypton.Docking;
using ComponentFactory.Krypton.Navigator;
using ComponentFactory.Krypton.Workspace;
using Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ActiveWorks.UserControls
{
    public partial class UcTabProfile3 : KryptonDockableWorkspace, IProfileTab
    {
        private const string LayoutFile = "layout.xml";
        private IUserProfile _profile;
        private readonly KryptonDockingManager _manager;

        public UcTabProfile3()
        {
            InitializeComponent();

        }

        public UcTabProfile3(KryptonDockingManager manager) : this()
        {
            _manager = manager;
        }


        public void ResetLayout()
        {
            try
            {
                File.Delete(Path.Combine(_profile.ProfilePath, LayoutFile));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        public void CloseProgram()
        {
            _profile.Exit();

        }

        public IUserProfile GetUserProfile()
        {
            return _profile;
        }

        public bool IsInitializedControl { get; set; }
        public void Init()
        {
            _profile = (IUserProfile)Tag;
            Init(_profile);
        }

        public void SaveLayout()
        {
            SaveLayoutToFile(Path.Combine(_profile.ProfilePath, LayoutFile));
        }

        private void Init(IUserProfile profile)
        {

            profile.InitProfile();

            CreateJobListTab();
            CreateBrowserTab();
            CreateFtpTab();
            CreatePluginsTab();
            CreateEvents();
            IsInitializedControl = true;

            LoadLayout();

            //profile.Jobs.LoadJobs();
           
                profile.Jobs?.ApplyStatusViewFilter();
        }

        private void CreateJobListTab()
        {
            if  (_profile.Jobs == null) return;

            var page = new KryptonPage(@"Список робіт") { TextTitle = @"JobList", UniqueName = "Список робіт" };

            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);

            var jobListControl = _profile.Jobs.JobListControl;
            if (jobListControl != null)
            {
                jobListControl.OnChangeCountJobs += (sender, i) =>
                {
                    page.Text = $"Список робіт ({i})";
                };

                page.Controls.Add((Control)jobListControl);

                _manager.AddToWorkspace("Workspace", new[] { page });
            }
          
        }

        private void CreateBrowserTab()
        {
            KryptonWorkspaceCell parent = new KryptonWorkspaceCell();
            parent.CloseAction += (sender, args) => args.Action = CloseButtonAction.None;

            var filebrowsers = _profile.FileBrowser;

            if (filebrowsers == null) return;

            // основний провідник
            CreatePluginPage(parent, "MainBrowser", "Провідник", (Control)filebrowsers.Browsers[0]);

            for (int i = 1; i <= _profile.Settings.CountExplorers; i++)
            {
                var settings = filebrowsers.Browsers[i].GetSettings();
                var title = settings.Title;
                if (string.IsNullOrEmpty(title)) title = $"Провідник {i + 1}";

                var page = CreatePluginPage(parent, i.ToString(), title, (Control)filebrowsers.Browsers[i]);

                //var idx = i;
                settings.PropertyChanged += (sender, args) =>
                    {

                        if (string.Compare(args.PropertyName, "Title", StringComparison.Ordinal) == 0)
                        {
                            page.TextTitle = settings.Title;
                            page.Text = settings.Title;
                        }
                    };


            }

            Root.Children.Add(parent);
        }
        KryptonPage CreatePluginPage(KryptonWorkspaceCell parent, string uniqueName, string name, Control control)
        {
            var page = new KryptonPage(name) { TextTitle = name, UniqueName = uniqueName };
            page.Controls.Add(control);
            parent.Pages.Add(page);
            return page;
        }

        private void CreateFtpTab()
        {
            if (_profile.Customers == null) return;

            var ftpCustomers = _profile.Customers.GetCustomersWithFtp();
            if (ftpCustomers.Any())
            {
                var page = new KryptonPage("Ftp") { TextTitle = @"Ftp" };
                page.UniqueName = "Ftp_page";
                page.Controls.Add((Control)_profile.Ftp.FtpExplorer);
                KryptonWorkspaceCell cell = new KryptonWorkspaceCell();
                cell.CloseAction += (sender, args) => args.Action = CloseButtonAction.None;
                cell.Pages.Add(page);

                Root.Children.Add(cell);
            }
        }

        private void CreatePluginsTab()
        {
            if (_profile.Plugins == null) return;

            KryptonWorkspaceCell parent = new KryptonWorkspaceCell();
            parent.CloseAction += (sender, args) => args.Action = CloseButtonAction.None;

            foreach (var pluginName in _profile.Plugins.GetPluginNames())
            {
                //_profile.Plugins.InvokeMethod(pluginName, "SetCurJobPathCallBack", _profile.Jobs.JobListControl.GetSelectedJobPath());
                //_profile.Plugins.InvokeMethod(pluginName, "SetCurJobCallBack", _profile.Jobs.JobListControl.GetSelectedJob());

                var c = (Control)_profile.Plugins.InvokeMethod(pluginName, "GetUserControl");


                if (c != null)
                {
                    c.Dock = DockStyle.Fill;

                    CreatePluginPage(parent, pluginName, pluginName, c);

                    _profile.Plugins.InvokeMethod(pluginName, "SetUserProfile", _profile);
                    _profile.Plugins.InvokeMethod(pluginName, "Start");
                }

            }

            if (parent.Pages.Count > 0)
            {
                Root.Children.Add(parent);
            }
        }
        private void CreateEvents()
        {
            if (_profile.Jobs == null) return;  

            _profile.Jobs.JobListControl.CreateEvents();
            _profile.FileBrowser.CreateEvents();
            _profile.Events.Ftp.ChangeStatus += ChangeStatus;
            _profile.Ftp.CreateEvents();

        }
        private void ChangeStatus(object sender, bool e)
        {
            //throw new NotImplementedException();
        }
        private void LoadLayout()
        {
            try
            {
                LoadLayoutFromFile(Path.Combine(_profile.ProfilePath, LayoutFile));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

            }
        }
    }
}
