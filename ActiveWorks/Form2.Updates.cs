using ActiveWorks.Forms;
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
        private const string EnterLicenseStatusText = "Ввести ключ ліцензії";
        private const string LicenseInactiveStatusText = "Оновлення недоступні: підписка неактивна.";

        private void InitializeLicenseStatus()
        {
            if (_licenseClientService == null || !_licenseClientService.IsConfigured)
            {
                return;
            }

            var state = _licenseClientService.CurrentToken;
            if (state.IsValid)
            {
                SetLicenseActiveStatus(state);
            }
            else
            {
                toolStripStatusLabelUpdate.Text = EnterLicenseStatusText;
                toolStripStatusLabelUpdate.ToolTipText = "Натисніть, щоб активувати підписку.";
            }
        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                ClearPendingUpdate();
                var result = await _updateClientService.CheckForUpdatesAsync();
                if (result.IsAccessDenied)
                {
                    toolStripStatusLabelUpdate.Text = LicenseInactiveStatusText;
                    toolStripStatusLabelUpdate.ToolTipText = result.AccessDeniedReason;
                    return;
                }

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

        private async void ToolStripStatusLabelUpdate_Click(object sender, EventArgs e)
        {
            if (_pendingUpdateType == UpdateHubShared.UpdateType.Optional && !string.IsNullOrWhiteSpace(_pendingUpdateFolder))
            {
                var answer = MessageBox.Show(this, "Встановити необов'язкове оновлення зараз?", "Необов'язкове оновлення",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (answer == DialogResult.Yes)
                {
                    StartUpdateAndClose();
                }

                return;
            }

            if (_licenseClientService == null || !_licenseClientService.IsConfigured)
            {
                return;
            }

            using (var form = new FormLicenseKey(_licenseClientService.StoredLicenseKey))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(form.LicenseKey))
                {
                    MessageBox.Show(this, "Введіть ключ ліцензії.", "Ключ ліцензії", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                toolStripStatusLabelUpdate.Text = "Активація ліцензії...";
                toolStripStatusLabelUpdate.ToolTipText = string.Empty;

                try
                {
                    var state = await _licenseClientService.ActivateAsync(form.LicenseKey);
                    if (!state.IsValid)
                    {
                        MessageBox.Show(this, state.Message, "Ліцензію не активовано", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        toolStripStatusLabelUpdate.Text = LicenseInactiveStatusText;
                        toolStripStatusLabelUpdate.ToolTipText = state.Message;
                        return;
                    }

                    SetLicenseActiveStatus(state);
                    var licenseStatusText = toolStripStatusLabelUpdate.Text;
                    toolStripStatusLabelUpdate.Text = licenseStatusText + " Перевіряю оновлення...";
                    await CheckForUpdatesAsync();
                    if (string.Equals(toolStripStatusLabelUpdate.Text, licenseStatusText + " Перевіряю оновлення...", StringComparison.Ordinal))
                    {
                        SetLicenseActiveStatus(state);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Licensing", "ToolStripStatusLabelUpdate_Click", ex.ToString());
                    MessageBox.Show(this, ex.Message, "Ліцензію не активовано", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabelUpdate.Text = LicenseInactiveStatusText;
                    toolStripStatusLabelUpdate.ToolTipText = ex.Message;
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

        private void ClearPendingUpdate()
        {
            _pendingUpdateFolder = null;
            _pendingUpdateType = null;
        }

        private void SetLicenseActiveStatus(ActiveWorks.Licensing.LicenseTokenState state)
        {
            DateTime paidUntilUtc;
            if (state?.Payload == null ||
                string.IsNullOrWhiteSpace(state.Payload.PaidUntilUtc) ||
                !DateTime.TryParse(state.Payload.PaidUntilUtc, null, System.Globalization.DateTimeStyles.RoundtripKind, out paidUntilUtc))
            {
                toolStripStatusLabelUpdate.Text = "Ліцензія активна.";
                toolStripStatusLabelUpdate.ToolTipText = string.Empty;
                return;
            }

            if (paidUntilUtc.Kind != DateTimeKind.Utc)
            {
                paidUntilUtc = paidUntilUtc.ToUniversalTime();
            }

            var daysLeft = Math.Max(0, (int)Math.Ceiling((paidUntilUtc - DateTime.UtcNow).TotalDays));
            toolStripStatusLabelUpdate.Text = $"Ліцензія активна: залишилось {daysLeft} {FormatDays(daysLeft)}.";
            toolStripStatusLabelUpdate.ToolTipText = "Підписка оплачена до " + paidUntilUtc.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        }

        private static string FormatDays(int days)
        {
            var lastTwoDigits = days % 100;
            if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            {
                return "днів";
            }

            switch (days % 10)
            {
                case 1:
                    return "день";
                case 2:
                case 3:
                case 4:
                    return "дні";
                default:
                    return "днів";
            }
        }
    }
}
