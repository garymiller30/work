using ActiveWorks.Properties;
using JobSpace.Profiles;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Drawing;
using System.IO;
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
            ClientSize = new Size(560, 270);

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

            _buttonCreate = new Button
            {
                Text = "Створити",
                Location = new Point(344, 230),
                Size = new Size(88, 26)
            };
            _buttonCreate.Click += ButtonCreate_Click;

            _buttonCancel = new Button
            {
                Text = "Скасувати",
                DialogResult = DialogResult.Cancel,
                Location = new Point(438, 230),
                Size = new Size(88, 26)
            };

            _labelStatus = new Label
            {
                AutoSize = false,
                Location = new Point(16, 232),
                Size = new Size(315, 22),
                ForeColor = Color.Firebrick
            };

            Controls.Add(labelTitle);
            Controls.Add(labelDescription);
            Controls.Add(CreateLabel("Назва профілю:", 16, 89));
            Controls.Add(CreateLabel("MongoDB connection:", 16, 125));
            Controls.Add(CreateLabel("Назва бази:", 16, 161));
            Controls.Add(CreateLabel("Timeout, сек:", 16, 197));
            Controls.Add(_textBoxProfileName);
            Controls.Add(_textBoxMongoConnection);
            Controls.Add(_textBoxDatabaseName);
            Controls.Add(_numericUpDownTimeout);
            Controls.Add(_labelStatus);
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
