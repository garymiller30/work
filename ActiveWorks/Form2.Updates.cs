using ActiveWorks.Properties;
using ActiveWorks.UpdateHub;
using Logger;
using Ookii.Dialogs.WinForms;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UpdateHubShared = global::UpdateHub;

namespace ActiveWorks
{
    public sealed partial class Form2
    {
        private async Task CheckForUpdatesAsync()
        {
            try
            {
                var result = await _updateClientService.CheckForUpdatesAsync();
                if (result.IsDisabled || !result.IsUpdateAvailable)
                {
                    return;
                }

                if (!Enum.TryParse(result.Manifest.UpdateType, true, out UpdateHubShared.UpdateType updateType))
                {
                    updateType = UpdateHubShared.UpdateType.Recommended;
                }

                _pendingUpdateFolder = result.DownloadFolder;
                _pendingUpdateType = updateType;

                switch (updateType)
                {
                    case UpdateHubShared.UpdateType.Critical:
                        ShowCriticalUpdate(result);
                        break;
                    case UpdateHubShared.UpdateType.Recommended:
                        ShowRecommendedUpdate(result);
                        break;
                    default:
                        ShowOptionalUpdate(result);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateHub", "CheckForUpdatesAsync", ex.ToString());
            }
        }

        private void ShowCriticalUpdate(UpdateCheckResult result)
        {
            var message =
                $"Доступне критичне оновлення {result.Manifest.Version}.{Environment.NewLine}{Environment.NewLine}" +
                $"{BuildUpdateSummary(result)}{Environment.NewLine}{Environment.NewLine}" +
                "Програму буде закрито для встановлення оновлення.";

            MessageBox.Show(this, message, "Критичне оновлення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            StartUpdateAndClose();
        }

        private void ShowRecommendedUpdate(UpdateCheckResult result)
        {
            if (string.Equals(Settings.Default.LastSkippedUpdateVersion, result.Manifest.Version, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var dialog = new TaskDialog
            {
                WindowTitle = "Оновлення доступне",
                MainInstruction = $"Доступне рекомендоване оновлення {result.Manifest.Version}",
                Content = "Доступна нова версія програми.",
                ExpandedInformation = BuildUpdateSummary(result),
                ExpandedControlText = "Приховати деталі",
                CollapsedControlText = "Показати деталі",
                MainIcon = TaskDialogIcon.Information,
                AllowDialogCancellation = true
            };

            var updateButton = new TaskDialogButton("Оновити зараз")
            {
                Default = true
            };

            var laterButton = new TaskDialogButton("Пізніше");

            dialog.Buttons.Add(updateButton);
            dialog.Buttons.Add(laterButton);

            var resultButton = dialog.ShowDialog(this);

            if (resultButton == updateButton)
            {
                StartUpdateAndClose();
                return;
            }

            Settings.Default.LastSkippedUpdateVersion = result.Manifest.Version;
            Settings.Default.Save();
        }

        private void ShowOptionalUpdate(UpdateCheckResult result)
        {
            toolStripStatusLabelUpdate.Text = $"Доступне необов'язкове оновлення {result.Manifest.Version}. Натисніть для встановлення.";
            toolStripStatusLabelUpdate.ToolTipText = BuildUpdateSummary(result);
        }

        private string BuildUpdateSummary(UpdateCheckResult result)
        {
            var changelog = string.IsNullOrWhiteSpace(result.Manifest.Changelog)
                ? "Опис змін не вказаний."
                : result.Manifest.Changelog.Trim();

            return $"Файлів до завантаження: {result.ChangedFilesCount}.{Environment.NewLine}{Environment.NewLine}{changelog}";
        }

        private void ConfigureUpdateTimer()
        {
            if (!_updateClientService.IsConfigured)
            {
                return;
            }

            var minutes = Settings.Default.UpdateHubCheckIntervalMinutes;
            if (minutes <= 0)
            {
                return;
            }

            _updateCheckTimer = new System.Windows.Forms.Timer();
            _updateCheckTimer.Interval = (int)Math.Min(TimeSpan.FromMinutes(minutes).TotalMilliseconds, int.MaxValue);
            _updateCheckTimer.Tick += async (s, e) =>
            {
                _updateCheckTimer.Stop();
                try
                {
                    await CheckForUpdatesAsync();
                }
                finally
                {
                    _updateCheckTimer.Start();
                }
            };
            _updateCheckTimer.Start();
        }

        private void ToolStripStatusLabelUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(toolStripStatusLabelUpdate.Text) && _pendingUpdateType == UpdateHubShared.UpdateType.Optional)
            {
                var answer = MessageBox.Show(this, "Встановити необов'язкове оновлення зараз?", "Необов'язкове оновлення",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (answer == DialogResult.Yes)
                {
                    StartUpdateAndClose();
                }
            }
        }

        private void StartUpdateAndClose()
        {
            if (string.IsNullOrWhiteSpace(_pendingUpdateFolder))
            {
                return;
            }

            try
            {
                UpdateClientService.StartUpdaterAndExit(_pendingUpdateFolder);
                Close();
            }
            catch (Exception ex)
            {
                Log.Error("UpdateHub", "StartUpdateAndClose", ex.ToString());
                MessageBox.Show(this, ex.Message, "Не вдалося запустити оновлення", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
