using ActiveWorks.Forms;
using ActiveWorks.Properties;
using ActiveWorks.UpdateHub;
using ActiveWorks.UserControls;
using ExtensionMethods;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using UpdateHubShared = global::UpdateHub;

namespace ActiveWorks
{
    public sealed partial class Form2 : KryptonForm
    {
        List<FormProfile> _profileTabs { get; set; } = new List<FormProfile>();
        FormProfile _activeProfileTab { get; set; }

        FormBackgroundTasks _formBackgroundTask;
        readonly Stopwatch _sw = new Stopwatch();
        readonly UpdateClientService _updateClientService;
        System.Windows.Forms.Timer _updateCheckTimer;
        string _pendingUpdateFolder;
        UpdateHubShared.UpdateType? _pendingUpdateType;


        public Form2()
        {
            InitializeComponent();
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

            string manifestUrl = Settings.Default.UpdateHubManifestUrl;
            Logger.Log.Info("App", "Form2", $"UpdateHub manifest URL: {manifestUrl}");
            _updateClientService = new UpdateClientService(Settings.Default.UpdateHubManifestUrl, AppDomain.CurrentDomain.BaseDirectory);
            toolStripStatusLabelUpdate.Click += ToolStripStatusLabelUpdate_Click;
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
            this.InvokeIfNeeded(new Action(() => buttonSpecBackgroundTasks.ExtraText = $"(...працює {e.Name}...)"));
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

     
    }
}
