using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UpdateHub;

namespace UpdateHubPublisher
{
    internal sealed class PublisherForm : Form
    {
        private const string BlacklistFileName = "blacklist.txt";
        private const string StateFileName = "publisher-state.txt";
        private const string ManifestFileName = "version.json";

        private readonly TextBox _sourceFolderTextBox;
        private readonly TextBox _publishFolderTextBox;
        private readonly TextBox _versionTextBox;
        private readonly ComboBox _updateTypeComboBox;
        private readonly TextBox _blacklistTextBox;
        private readonly TextBox _changelogTextBox;
        private readonly TextBox _previewTextBox;
        private UpdateManifest _currentManifest;

        public PublisherForm()
        {
            Text = "Update Hub Publisher";
            Width = 920;
            Height = 760;
            StartPosition = FormStartPosition.CenterScreen;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 8,
                Padding = new Padding(12)
            };

            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));

            Controls.Add(root);

            _sourceFolderTextBox = AddPathRow(root, 0, "Source bin/Release:", BrowseSourceFolder);
            _publishFolderTextBox = AddPathRow(root, 1, "IIS publish path:", BrowsePublishFolder);

            _versionTextBox = new TextBox { Dock = DockStyle.Fill };
            AddRow(root, 2, "Version:", _versionTextBox);

            _updateTypeComboBox = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _updateTypeComboBox.Items.AddRange(new object[] { UpdateType.Critical, UpdateType.Recommended, UpdateType.Optional });
            _updateTypeComboBox.SelectedIndex = 1;
            AddRow(root, 3, "Update type:", _updateTypeComboBox);

            _blacklistTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(root, 4, "Blacklist masks:", _blacklistTextBox, 120);

            _changelogTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(root, 5, "Changelog:", _changelogTextBox, 140);

            var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var generateButton = new Button { Text = "Generate Manifest", AutoSize = true };
            var publishButton = new Button { Text = "Publish Release", AutoSize = true };
            var saveBlacklistButton = new Button { Text = "Save Blacklist", AutoSize = true };
            generateButton.Click += (s, e) => GenerateManifestPreview();
            publishButton.Click += (s, e) => PublishRelease();
            saveBlacklistButton.Click += (s, e) => SaveBlacklist();
            buttonPanel.Controls.Add(generateButton);
            buttonPanel.Controls.Add(publishButton);
            buttonPanel.Controls.Add(saveBlacklistButton);
            AddRow(root, 6, "Actions:", buttonPanel);

            _previewTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Font = new System.Drawing.Font("Consolas", 9F)
            };
            AddRow(root, 7, "version.json:", _previewTextBox, 220);

            _blacklistTextBox.Text = string.Join(Environment.NewLine, new[]
            {
                "*.config",
                "*.log",
                "local_settings.json",
                "user.config",
                "temp_update/**"
            });

            LoadBlacklist();
            LoadState();
            FormClosing += PublisherForm_FormClosing;
        }

        private TextBox AddPathRow(TableLayoutPanel root, int rowIndex, string labelText, EventHandler browseHandler)
        {
            var textBox = new TextBox { Dock = DockStyle.Fill };
            var browseButton = new Button { Text = "Browse...", Dock = DockStyle.Fill };
            browseButton.Click += browseHandler;

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            root.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft }, 0, rowIndex);
            root.Controls.Add(textBox, 1, rowIndex);
            root.Controls.Add(browseButton, 2, rowIndex);

            return textBox;
        }

        private void AddRow(TableLayoutPanel root, int rowIndex, string labelText, Control control, int fixedHeight = 34)
        {
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, fixedHeight));
            root.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft }, 0, rowIndex);
            root.Controls.Add(control, 1, rowIndex);
            root.SetColumnSpan(control, 2);
        }

        private void BrowseSourceFolder(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = _sourceFolderTextBox.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _sourceFolderTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void BrowsePublishFolder(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = _publishFolderTextBox.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _publishFolderTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void GenerateManifestPreview()
        {
            try
            {
                SaveBlacklist();
                SaveState();
                _currentManifest = BuildManifest();
                _previewTextBox.Text = UpdateManifestSerializer.Serialize(_currentManifest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Update Hub Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PublishRelease()
        {
            try
            {
                SaveBlacklist();
                SaveState();
                var publishRoot = RequireDirectory(_publishFolderTextBox.Text, "IIS publish path");
                var manifest = _currentManifest ?? BuildManifest();
                ApplyChangelogHistory(manifest, publishRoot);
                var releaseRoot = Path.Combine(publishRoot, "releases", manifest.Version);

                if (Directory.Exists(releaseRoot))
                {
                    Directory.Delete(releaseRoot, true);
                }

                Directory.CreateDirectory(releaseRoot);

                foreach (var file in manifest.Files)
                {
                    var sourceFile = Path.Combine(_sourceFolderTextBox.Text, file.Path.Replace('/', Path.DirectorySeparatorChar));
                    var targetFile = Path.Combine(releaseRoot, file.Path.Replace('/', Path.DirectorySeparatorChar));
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                    File.Copy(sourceFile, targetFile, true);
                }

                UpdateManifestSerializer.SaveToFile(manifest, Path.Combine(releaseRoot, ManifestFileName));
                UpdateManifestSerializer.SaveToFile(manifest, Path.Combine(publishRoot, ManifestFileName));

                _currentManifest = manifest;
                _previewTextBox.Text = UpdateManifestSerializer.Serialize(manifest);

                MessageBox.Show(this, "Release successfully published.", "Update Hub Publisher", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Update Hub Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private UpdateManifest BuildManifest()
        {
            var sourceFolder = RequireDirectory(_sourceFolderTextBox.Text, "Source bin/Release");
            var version = (_versionTextBox.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new InvalidOperationException("Version is required.");
            }

            var selectedType = (UpdateType)_updateTypeComboBox.SelectedItem;
            var blacklist = _blacklistTextBox.Lines
                .Select(x => (x ?? string.Empty).Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var files = new List<UpdateManifestFile>();
            foreach (var filePath in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
            {
                var relativePath = PathUtility.NormalizeRelativePath(filePath.Substring(sourceFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                if (blacklist.Any(mask => FileMaskMatcher.IsMatch(mask, relativePath)))
                {
                    continue;
                }

                var fileInfo = new FileInfo(filePath);
                files.Add(new UpdateManifestFile
                {
                    Path = relativePath,
                    Hash = FileHashService.ComputeSha256(filePath),
                    Size = fileInfo.Length
                });
            }

            return new UpdateManifest
            {
                Version = version,
                UpdateType = selectedType.ToString(),
                Changelog = (_changelogTextBox.Text ?? string.Empty).Trim(),
                IgnoredFiles = blacklist,
                PackagePath = PathUtility.EnsureTrailingSlash(PathUtility.CombineUrl("releases", version)),
                PublishedAtUtc = DateTime.UtcNow.ToString("O"),
                Files = files.OrderBy(x => x.Path, StringComparer.OrdinalIgnoreCase).ToList()
            };
        }

        private static void ApplyChangelogHistory(UpdateManifest manifest, string publishRoot)
        {
            var history = LoadExistingChangelogHistory(publishRoot);
            history.RemoveAll(x => string.Equals(x.Version, manifest.Version, StringComparison.OrdinalIgnoreCase));
            history.Insert(0, new UpdateChangelogEntry
            {
                Version = manifest.Version,
                UpdateType = manifest.UpdateType,
                PublishedAtUtc = manifest.PublishedAtUtc,
                Changelog = manifest.Changelog
            });

            manifest.ChangelogHistory = history
                .OrderByDescending(x => TryParseVersion(x.Version))
                .ThenByDescending(x => TryParseDateTime(x.PublishedAtUtc))
                .ToList();
        }

        private static List<UpdateChangelogEntry> LoadExistingChangelogHistory(string publishRoot)
        {
            var manifestPath = Path.Combine(publishRoot, ManifestFileName);
            if (!File.Exists(manifestPath))
            {
                return new List<UpdateChangelogEntry>();
            }

            try
            {
                var previousManifest = UpdateManifestSerializer.Deserialize(File.ReadAllText(manifestPath));
                var history = previousManifest.ChangelogHistory ?? new List<UpdateChangelogEntry>();

                if (!string.IsNullOrWhiteSpace(previousManifest.Changelog) &&
                    !history.Any(x => string.Equals(x.Version, previousManifest.Version, StringComparison.OrdinalIgnoreCase)))
                {
                    history.Add(new UpdateChangelogEntry
                    {
                        Version = previousManifest.Version,
                        UpdateType = previousManifest.UpdateType,
                        PublishedAtUtc = previousManifest.PublishedAtUtc,
                        Changelog = previousManifest.Changelog
                    });
                }

                return history
                    .Where(x => !string.IsNullOrWhiteSpace(x.Version))
                    .ToList();
            }
            catch
            {
                return new List<UpdateChangelogEntry>();
            }
        }

        private static Version TryParseVersion(string version)
        {
            Version parsedVersion;
            return Version.TryParse(version, out parsedVersion) ? parsedVersion : new Version(0, 0);
        }

        private static DateTime TryParseDateTime(string value)
        {
            DateTime parsedDateTime;
            return DateTime.TryParse(value, out parsedDateTime) ? parsedDateTime : DateTime.MinValue;
        }

        private static string RequireDirectory(string path, string caption)
        {
            path = (path ?? string.Empty).Trim();
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(caption + " does not exist.");
            }

            return path;
        }

        private void LoadBlacklist()
        {
            var filePath = GetBlacklistFilePath();
            if (!File.Exists(filePath))
            {
                return;
            }

            var lines = File.ReadAllLines(filePath)
                .Select(x => (x ?? string.Empty).Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (lines.Length > 0)
            {
                _blacklistTextBox.Text = string.Join(Environment.NewLine, lines);
            }
        }

        private void SaveBlacklist()
        {
            var filePath = GetBlacklistFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var lines = _blacklistTextBox.Lines
                .Select(x => (x ?? string.Empty).Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            File.WriteAllLines(filePath, lines);
        }

        private void PublisherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SaveBlacklist();
                SaveState();
            }
            catch
            {
                // Do not block closing if the local preferences file cannot be written.
            }
        }

        private static string GetBlacklistFilePath()
        {
            return Path.Combine(Application.UserAppDataPath, BlacklistFileName);
        }

        private void LoadState()
        {
            var filePath = GetStateFilePath();
            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                var separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0)
                {
                    continue;
                }

                var key = line.Substring(0, separatorIndex).Trim();
                var value = line.Substring(separatorIndex + 1).Trim();

                if (key.Equals("SourceFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _sourceFolderTextBox.Text = value;
                }
                else if (key.Equals("PublishFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _publishFolderTextBox.Text = value;
                }
                else if (key.Equals("Version", StringComparison.OrdinalIgnoreCase))
                {
                    _versionTextBox.Text = value;
                }
            }
        }

        private void SaveState()
        {
            var filePath = GetStateFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var lines = new[]
            {
                "SourceFolder=" + ((_sourceFolderTextBox.Text ?? string.Empty).Trim()),
                "PublishFolder=" + ((_publishFolderTextBox.Text ?? string.Empty).Trim()),
                "Version=" + ((_versionTextBox.Text ?? string.Empty).Trim())
            };

            File.WriteAllLines(filePath, lines);
        }

        private static string GetStateFilePath()
        {
            return Path.Combine(Application.UserAppDataPath, StateFileName);
        }
    }
}
