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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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


        private async void ButtonSpecAnyWhatNew_Click(object sender, EventArgs e)
        {
            using (var form = new FormWhatNew(await GetWhatNewTextAsync()))
            {
                form.ShowDialog();
            }
        }

        private async Task<string> GetWhatNewTextAsync()
        {
            var fallbackText = Settings.Default.WhatNew;
            if (_updateClientService == null || !_updateClientService.IsConfigured)
            {
                return fallbackText;
            }

            try
            {
                var manifest = await _updateClientService.DownloadManifestAsync();
                var changelogHistoryText = BuildChangelogHistoryText(manifest);
                if (!string.IsNullOrWhiteSpace(changelogHistoryText))
                {
                    if (!string.IsNullOrWhiteSpace(fallbackText))
                    {
                        return changelogHistoryText + Environment.NewLine + Environment.NewLine + fallbackText.Trim();
                    }

                    return changelogHistoryText;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("UpdateHub", "GetWhatNewTextAsync", ex.ToString());
            }

            return fallbackText;
        }

        private static string BuildChangelogHistoryText(UpdateHubShared.UpdateManifest manifest)
        {
            if (manifest == null)
            {
                return string.Empty;
            }

            var entries = manifest.ChangelogHistory ?? new List<UpdateHubShared.UpdateChangelogEntry>();
            if (entries.Count == 0 && (!string.IsNullOrWhiteSpace(manifest.Version) || !string.IsNullOrWhiteSpace(manifest.Changelog)))
            {
                entries = new List<UpdateHubShared.UpdateChangelogEntry>
                {
                    new UpdateHubShared.UpdateChangelogEntry
                    {
                        Version = manifest.Version,
                        UpdateType = manifest.UpdateType,
                        PublishedAtUtc = manifest.PublishedAtUtc,
                        Changelog = manifest.Changelog
                    }
                };
            }

            var builder = new StringBuilder();
            foreach (var entry in entries.Where(x => x != null && (!string.IsNullOrWhiteSpace(x.Version) || !string.IsNullOrWhiteSpace(x.Changelog))))
            {
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }

                builder.AppendLine(BuildChangelogHeader(entry));
                builder.AppendLine("-----------------");
                builder.AppendLine(string.IsNullOrWhiteSpace(entry.Changelog) ? "Опис змін не вказаний." : entry.Changelog.Trim());
            }

            return builder.ToString().Trim();
        }

        private static string BuildChangelogHeader(UpdateHubShared.UpdateChangelogEntry entry)
        {
            var version = string.IsNullOrWhiteSpace(entry.Version) ? "Версія не вказана" : entry.Version.Trim();
            DateTime publishedAtUtc;
            if (DateTime.TryParse(entry.PublishedAtUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out publishedAtUtc))
            {
                return version + " (" + publishedAtUtc.ToLocalTime().ToString("dd.MM.yyyy") + ")";
            }

            return version;
        }
    }
}
