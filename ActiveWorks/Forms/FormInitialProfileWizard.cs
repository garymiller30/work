using ActiveWorks.Properties;
using JobSpace.Profiles;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActiveWorks.Forms
{
    public sealed class FormInitialProfileWizard : Form
    {
        private readonly string _profilesPath;
        private readonly TextBox _textBoxProfileName;
        private readonly TextBox _textBoxMongoConnection;
        private readonly TextBox _textBoxDatabaseName;
        private readonly NumericUpDown _numericUpDownTimeout;
        private readonly Button _buttonCreate;
        private readonly Button _buttonImport;
        private readonly Button _buttonCancel;
        private readonly Label _labelStatus;

        public FormInitialProfileWizard(string profilesPath)
        {
            _profilesPath = profilesPath;

            Text = "Setting Wizard";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(560, 306);

            var labelTitle = new Label
            {
                AutoSize = false,
                Text = "Перший запуск ActiveWorks",
                Font = new Font(Font, FontStyle.Bold),
                Location = new Point(16, 16),
                Size = new Size(520, 22)
            };

            var labelDescription = new Label
            {
                AutoSize = false,
                Text = "Створіть перший профіль і вкажіть параметри підключення до MongoDB.",
                Location = new Point(16, 42),
                Size = new Size(520, 32)
            };

            _textBoxProfileName = CreateTextBox(170, 86, 350);
            _textBoxMongoConnection = CreateTextBox(170, 122, 350);
            _textBoxDatabaseName = CreateTextBox(170, 158, 220);
            _numericUpDownTimeout = new NumericUpDown
            {
                Location = new Point(170, 194),
                Minimum = 1,
                Maximum = 120,
                Value = 3,
                Size = new Size(80, 20)
            };

            _buttonImport = new Button
            {
                Text = "Імпортувати архів...",
                Location = new Point(170, 230),
                Size = new Size(140, 26)
            };
            _buttonImport.Click += ButtonImport_Click;

            _buttonCreate = new Button
            {
                Text = "Створити",
                Location = new Point(344, 266),
                Size = new Size(88, 26)
            };
            _buttonCreate.Click += ButtonCreate_Click;

            _buttonCancel = new Button
            {
                Text = "Скасувати",
                DialogResult = DialogResult.Cancel,
                Location = new Point(438, 266),
                Size = new Size(88, 26)
            };

            _labelStatus = new Label
            {
                AutoSize = false,
                Location = new Point(16, 268),
                Size = new Size(315, 22),
                ForeColor = Color.Firebrick
            };

            Controls.Add(labelTitle);
            Controls.Add(labelDescription);
            Controls.Add(CreateLabel("Назва профілю:", 16, 89));
            Controls.Add(CreateLabel("MongoDB connection:", 16, 125));
            Controls.Add(CreateLabel("Назва бази:", 16, 161));
            Controls.Add(CreateLabel("Timeout, сек:", 16, 197));
            Controls.Add(CreateLabel("Архів профілів:", 16, 234));
            Controls.Add(_textBoxProfileName);
            Controls.Add(_textBoxMongoConnection);
            Controls.Add(_textBoxDatabaseName);
            Controls.Add(_numericUpDownTimeout);
            Controls.Add(_labelStatus);
            Controls.Add(_buttonImport);
            Controls.Add(_buttonCreate);
            Controls.Add(_buttonCancel);

            AcceptButton = _buttonCreate;
            CancelButton = _buttonCancel;
        }

        public static bool HasProfiles(string profilesPath)
        {
            if (!Directory.Exists(profilesPath))
            {
                return false;
            }

            return Directory.GetDirectories(profilesPath)
                .Any(d => !Path.GetFileName(d).StartsWith("-") &&
                          File.Exists(Path.Combine(d, "ProfileSettings.xml")));
        }

        private static Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(140, 20)
            };
        }

        private static TextBox CreateTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 20)
            };
        }

        private async void ButtonCreate_Click(object sender, EventArgs e)
        {
            var profileName = _textBoxProfileName.Text.Trim();
            var connectionString = _textBoxMongoConnection.Text.Trim();
            var databaseName = _textBoxDatabaseName.Text.Trim();
            var timeout = (int)_numericUpDownTimeout.Value;

            if (!ValidateInput(profileName, connectionString, databaseName))
            {
                return;
            }

            SetBusy(true, "Перевіряємо підключення...");

            try
            {
                var canConnect = await TestMongoConnectionAsync(connectionString, databaseName, timeout);
                if (!canConnect)
                {
                    SetBusy(false, "Не вдалося підключитися до бази даних.");
                    return;
                }

                var settings = new ProfileSettings
                {
                    ProfileName = profileName
                };
                settings.BaseSettings.MongoDbServer = connectionString;
                settings.BaseSettings.MongoDbBaseName = databaseName;
                settings.BaseSettings.BaseTimeOut = timeout;
                settings.Mail.SettingsFolder = Path.Combine(Settings.Default.ProfilesPath, profileName);

                ProfilesController.CreateProfile(_profilesPath, settings);

                Settings.Default.DefaultProfile = profileName;
                Settings.Default.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                SetBusy(false, ex.Message);
            }
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "ZIP archives (*.zip)|*.zip|All files (*.*)|*.*";
                dialog.Title = "Оберіть архів профілю";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                SetBusy(true, "Імпортуємо профілі...");

                try
                {
                    var importedProfileNames = ImportProfilesArchive(dialog.FileName);
                    if (importedProfileNames.Length == 0)
                    {
                        SetBusy(false, "В архіві не знайдено профілів.");
                        return;
                    }

                    ProfilesController.LoadProfiles(_profilesPath);
                    var defaultProfile = ProfilesController.GetProfiles().FirstOrDefault();
                    Settings.Default.DefaultProfile = defaultProfile?.Settings.ProfileName ?? importedProfileNames[0];
                    Settings.Default.Save();

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    SetBusy(false, ex.Message);
                }
            }
        }

        private string[] ImportProfilesArchive(string archivePath)
        {
            Directory.CreateDirectory(_profilesPath);

            using (var archive = ZipFile.OpenRead(archivePath))
            {
                var profileRoots = GetProfileRoots(archive).ToArray();
                if (profileRoots.Length == 0)
                {
                    return Array.Empty<string>();
                }

                foreach (var profileRoot in profileRoots)
                {
                    var profileName = GetProfileName(profileRoot);
                    var targetRoot = ResolveChildPath(_profilesPath, profileName);
                    if (Directory.Exists(targetRoot) && Directory.EnumerateFileSystemEntries(targetRoot).Any())
                    {
                        throw new InvalidOperationException($"Профіль '{profileName}' вже існує.");
                    }

                    foreach (var entry in archive.Entries)
                    {
                        if (string.IsNullOrWhiteSpace(entry.Name))
                        {
                            continue;
                        }

                        var entryPath = NormalizeArchivePath(entry.FullName);
                        var rootPrefix = profileRoot + "/";
                        if (!entryPath.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var relativePath = entryPath.Substring(rootPrefix.Length);
                        var targetPath = ResolveChildPath(targetRoot, relativePath);
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                        entry.ExtractToFile(targetPath, true);
                    }
                }

                return profileRoots.Select(GetProfileName).ToArray();
            }
        }

        private static IEnumerable<string> GetProfileRoots(ZipArchive archive)
        {
            return archive.Entries
                .Where(entry => string.Equals(entry.Name, "ProfileSettings.xml", StringComparison.OrdinalIgnoreCase))
                .Select(entry => NormalizeArchivePath(entry.FullName))
                .Select(path =>
                {
                    var separatorIndex = path.LastIndexOf('/');
                    return separatorIndex > 0 ? path.Substring(0, separatorIndex) : string.Empty;
                })
                .Where(root => !string.IsNullOrWhiteSpace(root))
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private static string GetProfileName(string profileRoot)
        {
            var profileName = profileRoot.Split('/').Last();
            if (string.IsNullOrWhiteSpace(profileName) || profileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new InvalidOperationException("Архів містить профіль з недопустимою назвою.");
            }

            return profileName;
        }

        private static string NormalizeArchivePath(string entryPath)
        {
            return (entryPath ?? string.Empty).Replace('\\', '/').Trim('/');
        }

        private static string ResolveChildPath(string root, string relativePath)
        {
            var rootFullPath = Path.GetFullPath(root);
            var fullPath = Path.GetFullPath(Path.Combine(rootFullPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));
            var prefix = rootFullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!fullPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fullPath, rootFullPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Архів містить шлях за межі каталогу профілів.");
            }

            return fullPath;
        }

        private bool ValidateInput(string profileName, string connectionString, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                _labelStatus.Text = "Вкажіть назву профілю.";
                return false;
            }

            if (profileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                _labelStatus.Text = "Назва профілю містить недопустимі символи.";
                return false;
            }

            if (Directory.Exists(Path.Combine(_profilesPath, profileName)))
            {
                _labelStatus.Text = "Профіль з такою назвою вже існує.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _labelStatus.Text = "Вкажіть рядок підключення MongoDB.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                _labelStatus.Text = "Вкажіть назву бази даних.";
                return false;
            }

            return true;
        }

        private void SetBusy(bool busy, string status)
        {
            _buttonCreate.Enabled = !busy;
            _buttonImport.Enabled = !busy;
            _buttonCancel.Enabled = !busy;
            _textBoxProfileName.Enabled = !busy;
            _textBoxMongoConnection.Enabled = !busy;
            _textBoxDatabaseName.Enabled = !busy;
            _numericUpDownTimeout.Enabled = !busy;
            _labelStatus.ForeColor = busy ? Color.DarkSlateGray : Color.Firebrick;
            _labelStatus.Text = status;
        }

        private static Task<bool> TestMongoConnectionAsync(string connectionString, string databaseName, int timeout)
        {
            return Task.Run(() =>
            {
                try
                {
                    var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
                    clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(timeout);
                    clientSettings.ConnectTimeout = TimeSpan.FromSeconds(timeout);
                    var client = new MongoClient(clientSettings);
                    var database = client.GetDatabase(databaseName);
                    database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}
