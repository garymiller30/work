// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Interfaces;
using JobSpace.UC;

namespace JobSpace.Profiles
{
    public sealed class FileBrowsers : IFileBrowsers
    {

        private readonly IUserProfile _profile;

        public List<IFileBrowser> Browsers { get; set; } = new List<IFileBrowser>();
        
        
        public void CreateEvents()
        {
            _profile.Events.Browsers.OnChangeJobDescription += BrowserEvents_OnChangeJobDescription;
            _profile.Events.Browsers.OnChangeStatus += BrowserEventsOnChangeStatus;
            _profile.Events.Browsers.OnCreateOrderFromFile += BrowserEvents_OnCreateOrderFromFile;

            _profile.Events.Browsers.Init(_profile);
        }
        

        private void BrowserEvents_OnCreateOrderFromFile(object sender, string e)
        {
            _profile.Jobs.JobListControl.CreateJobFromFile(e);
        }

        private void BrowserEventsOnChangeStatus(object sender, IJob e)
        {
            throw new NotImplementedException();
        }

        private void BrowserEvents_OnChangeJobDescription(object sender, string e)
        {
            var job = _profile.Jobs.JobListControl.ChangeSelectedJobDescription(e);

            //_profile.Jobs.SetCurrentJob(job);

            //if (job != null)
            //{
            //    ((FileBrowser)sender).SetRootFolder(_profile.Jobs.GetFullPathToWorkFolder(job));
            //    ((FileBrowser)sender).CurrentJob = job;
            //}
            
        }


        public FileBrowsers(IUserProfile userProfile)
        {
            _profile = userProfile;
            _profile.Events.Jobs.OnSetCurrentJob += Jobs_OnSetCurrentJob;
            CreateFileBrowsers();
            LoadSettings();
            SetCustomButtonPath();
        }

        private void Jobs_OnSetCurrentJob(object sender, IJob e)
        {
            if (e == null)
            {
                _profile.FileBrowser.Browsers[0].LockUI(false);
            }
            else
            {
                _profile.FileBrowser.Browsers[0].LockUI(true);
                _profile.FileBrowser.Browsers[0].SetRootFolder(_profile.Jobs.GetFullPathToWorkFolder(e));
            }
            
        }

        private void CreateFileBrowsers()
        {
            var mainBrowser = AddBrowser();
            mainBrowser.InitToolStripUtils(0);

            Browsers.Add(mainBrowser);

            for (int i = 1; i <= _profile.Settings.CountExplorers; i++)
            {
                var otherBrowser = AddBrowser();
                Browsers.Add(otherBrowser);
            }
            InitBrowserToolStripUtils();
        }

        public void InitBrowserToolStripUtils()
        {
            for (int i = 0; i < _profile.Settings.CountExplorers; i++)
            {
                Browsers[i].InitToolStripUtils(i);
            }
        }

        IFileBrowser AddBrowser()
        {
            var browser = Factory.CreateFileBrowser(_profile);
            ((Control) browser).Dock = DockStyle.Fill;
            return browser;
        }

        public void SaveSettings()
        {
            Browsers.ForEach(x=>x.SaveSettings());
        }

        private void LoadSettings()
        {
            for (int i = 0; i < Browsers.Count; i++)
            {
                Browsers[i].DefaultSettingsFolder = Path.Combine(_profile.ProfilePath, "Browsers", $"{i}");

                Directory.CreateDirectory(Browsers[i].DefaultSettingsFolder);
                Browsers[i].LoadSettings();
            }
        }

        public void SetCustomButtonPath()
        {
            Browsers.ForEach(x=>x.SetCustomButtonPath(_profile.Settings.GetFileBrowser().CustomButtonPath.ToArray()));
        }
    }
}
