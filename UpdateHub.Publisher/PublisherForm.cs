using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using UpdateHub;

namespace UpdateHubPublisher
{
    internal sealed class PublisherForm : Form
    {
        private const string BlacklistFileName = "blacklist.txt";
        private const string StateFileName = "publisher-state.txt";
        private const string PluginStateFileName = "plugin-publisher-state.txt";
        private const string ManifestFileName = "version.json";
        private const string PluginManifestFileName = "plugin.json";

        private readonly TextBox _sourceFolderTextBox;
        private readonly TextBox _publishFolderTextBox;
        private readonly TextBox _versionTextBox;
        private readonly ComboBox _updateTypeComboBox;
        private readonly TextBox _blacklistTextBox;
        private readonly TextBox _changelogTextBox;
        private readonly TextBox _previewTextBox;

        private readonly TextBox _pluginSourceFolderTextBox;
        private readonly TextBox _pluginPublishFolderTextBox;
        private readonly TextBox _pluginIdTextBox;
        private readonly TextBox _pluginNameTextBox;
        private readonly TextBox _pluginVersionTextBox;
        private readonly TextBox _pluginDescriptionTextBox;
        private readonly TextBox _pluginChangelogTextBox;
        private readonly TextBox _pluginApplicationMasksTextBox;
        private readonly TextBox _pluginIgnoredMasksTextBox;
        private readonly TextBox _pluginPreviewTextBox;

        private UpdateManifest _currentManifest;
        private PluginPackageManifest _currentPluginManifest;

        public PublisherForm()
        {
            Text = "Update Hub Publisher";
            Width = 980;
            Height = 820;
            StartPosition = FormStartPosition.CenterScreen;

            var tabs = new TabControl { Dock = DockStyle.Fill };
            var releaseTab = new TabPage("Application updates");
            var pluginTab = new TabPage("Plugins");
            tabs.TabPages.Add(releaseTab);
            tabs.TabPages.Add(pluginTab);
            Controls.Add(tabs);

            var root = CreateRootTable(8, 130);
            releaseTab.Controls.Add(root);

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

            _previewTextBox = CreatePreviewTextBox();
            AddRow(root, 7, "version.json:", _previewTextBox, 220);

            _blacklistTextBox.Text = string.Join(Environment.NewLine, new[]
            {
                "*.config",
                "*.log",
                "local_settings.json",
                "user.config",
                "temp_update/**"
            });

            var pluginRoot = CreateRootTable(11, 150);
            pluginTab.Controls.Add(pluginRoot);

            _pluginSourceFolderTextBox = AddPathRow(pluginRoot, 0, "Plugin source folder:", BrowsePluginSourceFolder);
            _pluginPublishFolderTextBox = AddPathRow(pluginRoot, 1, "Web publish path:", BrowsePluginPublishFolder);

            _pluginIdTextBox = new TextBox { Dock = DockStyle.Fill };
            AddRow(pluginRoot, 2, "Plugin id:", _pluginIdTextBox);

            _pluginNameTextBox = new TextBox { Dock = DockStyle.Fill };
            AddRow(pluginRoot, 3, "Plugin name:", _pluginNameTextBox);

            _pluginVersionTextBox = new TextBox { Dock = DockStyle.Fill };
            AddRow(pluginRoot, 4, "Version:", _pluginVersionTextBox);

            _pluginDescriptionTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(pluginRoot, 5, "Description:", _pluginDescriptionTextBox, 70);

            _pluginChangelogTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(pluginRoot, 6, "Changelog:", _pluginChangelogTextBox, 90);

            _pluginApplicationMasksTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(pluginRoot, 7, "Application masks:", _pluginApplicationMasksTextBox, 90);

            _pluginIgnoredMasksTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(pluginRoot, 8, "Ignored masks:", _pluginIgnoredMasksTextBox, 70);

            var pluginButtonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var generatePluginButton = new Button { Text = "Generate Plugin Manifest", AutoSize = true };
            var publishPluginButton = new Button { Text = "Publish Plugin", AutoSize = true };
            generatePluginButton.Click += (s, e) => GeneratePluginManifestPreview();
            publishPluginButton.Click += (s, e) => PublishPlugin();
            pluginButtonPanel.Controls.Add(generatePluginButton);
            pluginButtonPanel.Controls.Add(publishPluginButton);
            AddRow(pluginRoot, 9, "Actions:", pluginButtonPanel);

            _pluginPreviewTextBox = CreatePreviewTextBox();
            AddRow(pluginRoot, 10, "plugin.json:", _pluginPreviewTextBox, 210);

            _pluginApplicationMasksTextBox.Text = string.Join(Environment.NewLine, new[]
            {
                "*.deps.json",
                "*.runtimeconfig.json",
                "*.config",
                "runtimes/**"
            });

            _pluginIgnoredMasksTextBox.Text = string.Join(Environment.NewLine, new[]
            {
                "*.pdb",
                "*.xml",
                "*.log",
                "plugin.json"
            });

            LoadBlacklist();
            LoadState();
            LoadPluginState();
            FormClosing += PublisherForm_FormClosing;
        }

        private static TableLayoutPanel CreateRootTable(int rows, int firstColumnWidth)
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = rows,
                Padding = new Padding(12)
            };

            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, firstColumnWidth));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            return root;
        }

        private static TextBox CreatePreviewTextBox()
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Font = new System.Drawing.Font("Consolas", 9F)
            };
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

        private void BrowseSourceFolder(object sender, EventArgs e) => BrowseFolder(_sourceFolderTextBox);

        private void BrowsePublishFolder(object sender, EventArgs e) => BrowseFolder(_publishFolderTextBox);

        private void BrowsePluginPublishFolder(object sender, EventArgs e) => BrowseFolder(_pluginPublishFolderTextBox);

        private void BrowsePluginSourceFolder(object sender, EventArgs e)
        {
            var previous = _pluginSourceFolderTextBox.Text;
            BrowseFolder(_pluginSourceFolderTextBox);
            if (!string.Equals(previous, _pluginSourceFolderTextBox.Text, StringComparison.OrdinalIgnoreCase))
            {
                var folderName = Path.GetFileName(_pluginSourceFolderTextBox.Text.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                if (string.IsNullOrWhiteSpace(_pluginIdTextBox.Text))
                {
                    _pluginIdTextBox.Text = folderName;
                }

                if (string.IsNullOrWhiteSpace(_pluginNameTextBox.Text))
                {
                    _pluginNameTextBox.Text = folderName;
                }
            }
        }

        private void BrowseFolder(TextBox textBox)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = textBox.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBox.Text = dialog.SelectedPath;
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

        private void GeneratePluginManifestPreview()
        {
            try
            {
                SavePluginState();
                _currentPluginManifest = BuildPluginManifest();
                _pluginPreviewTextBox.Text = SerializePluginManifest(_currentPluginManifest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Plugin Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PublishPlugin()
        {
            try
            {
                SavePluginState();
                var sourceFolder = RequireDirectory(_pluginSourceFolderTextBox.Text, "Plugin source folder");
                var publishRoot = RequireDirectory(_pluginPublishFolderTextBox.Text, "Web publish path");
                var manifest = _currentPluginManifest ?? BuildPluginManifest();
                var pluginRoot = Path.Combine(publishRoot, "Data", "plugins", manifest.Id);
                var filesRoot = Path.Combine(pluginRoot, "files");

                if (Directory.Exists(filesRoot))
                {
                    Directory.Delete(filesRoot, true);
                }

                Directory.CreateDirectory(filesRoot);
                foreach (var file in manifest.Files)
                {
                    var sourceFile = Path.Combine(sourceFolder, file.Path.Replace('/', Path.DirectorySeparatorChar));
                    var targetFile = Path.Combine(filesRoot, file.Path.Replace('/', Path.DirectorySeparatorChar));
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                    File.Copy(sourceFile, targetFile, true);
                }

                SavePluginManifest(manifest, Path.Combine(pluginRoot, PluginManifestFileName));
                _currentPluginManifest = manifest;
                _pluginPreviewTextBox.Text = SerializePluginManifest(manifest);
                MessageBox.Show(this, "Plugin successfully published.", "Plugin Publisher", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Plugin Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private UpdateManifest BuildManifest()
        {
            var sourceFolder = RequireDirectory(_sourceFolderTextBox.Text, "Source bin/Release");
            var version = RequireText(_versionTextBox.Text, "Version");
            var selectedType = (UpdateType)_updateTypeComboBox.SelectedItem;
            var blacklist = GetLines(_blacklistTextBox).ToList();

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

        private PluginPackageManifest BuildPluginManifest()
        {
            var sourceFolder = RequireDirectory(_pluginSourceFolderTextBox.Text, "Plugin source folder");
            var pluginId = RequireText(_pluginIdTextBox.Text, "Plugin id");
            var pluginName = RequireText(_pluginNameTextBox.Text, "Plugin name");
            var version = RequireText(_pluginVersionTextBox.Text, "Version");
            var applicationMasks = GetLines(_pluginApplicationMasksTextBox).ToList();
            var ignoredMasks = GetLines(_pluginIgnoredMasksTextBox).ToList();

            var files = new List<PluginPackageFile>();
            foreach (var filePath in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
            {
                var relativePath = PathUtility.NormalizeRelativePath(filePath.Substring(sourceFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                if (ignoredMasks.Any(mask => FileMaskMatcher.IsMatch(mask, relativePath)))
                {
                    continue;
                }

                var fileInfo = new FileInfo(filePath);
                files.Add(new PluginPackageFile
                {
                    Path = relativePath,
                    TargetPath = relativePath,
                    TargetRoot = applicationMasks.Any(mask => FileMaskMatcher.IsMatch(mask, relativePath))
                        ? PluginInstallTarget.Application
                        : PluginInstallTarget.ProfilePlugins,
                    Hash = FileHashService.ComputeSha256(filePath),
                    Size = fileInfo.Length
                });
            }

            if (files.Count == 0)
            {
                throw new InvalidOperationException("Plugin source folder does not contain publishable files.");
            }

            return new PluginPackageManifest
            {
                Id = pluginId,
                Name = pluginName,
                Version = version,
                Description = (_pluginDescriptionTextBox.Text ?? string.Empty).Trim(),
                Changelog = (_pluginChangelogTextBox.Text ?? string.Empty).Trim(),
                PublishedAtUtc = DateTime.UtcNow.ToString("O"),
                PackagePath = "files",
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

                return history.Where(x => !string.IsNullOrWhiteSpace(x.Version)).ToList();
            }
            catch
            {
                return new List<UpdateChangelogEntry>();
            }
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
            File.WriteAllLines(filePath, GetLines(_blacklistTextBox).ToArray());
        }

        private void LoadState()
        {
            var filePath = GetStateFilePath();
            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (var item in LoadKeyValues(filePath))
            {
                if (item.Key.Equals("SourceFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _sourceFolderTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("PublishFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _publishFolderTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("Version", StringComparison.OrdinalIgnoreCase))
                {
                    _versionTextBox.Text = item.Value;
                }
            }
        }

        private void LoadPluginState()
        {
            var filePath = GetPluginStateFilePath();
            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (var item in LoadKeyValues(filePath))
            {
                if (item.Key.Equals("PluginSourceFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginSourceFolderTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("PluginPublishFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginPublishFolderTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("PluginId", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginIdTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("PluginName", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginNameTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("PluginVersion", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginVersionTextBox.Text = item.Value;
                }
            }
        }

        private void SaveState()
        {
            SaveKeyValues(GetStateFilePath(), new[]
            {
                "SourceFolder=" + ((_sourceFolderTextBox.Text ?? string.Empty).Trim()),
                "PublishFolder=" + ((_publishFolderTextBox.Text ?? string.Empty).Trim()),
                "Version=" + ((_versionTextBox.Text ?? string.Empty).Trim())
            });
        }

        private void SavePluginState()
        {
            SaveKeyValues(GetPluginStateFilePath(), new[]
            {
                "PluginSourceFolder=" + ((_pluginSourceFolderTextBox.Text ?? string.Empty).Trim()),
                "PluginPublishFolder=" + ((_pluginPublishFolderTextBox.Text ?? string.Empty).Trim()),
                "PluginId=" + ((_pluginIdTextBox.Text ?? string.Empty).Trim()),
                "PluginName=" + ((_pluginNameTextBox.Text ?? string.Empty).Trim()),
                "PluginVersion=" + ((_pluginVersionTextBox.Text ?? string.Empty).Trim())
            });
        }

        private void PublisherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SaveBlacklist();
                SaveState();
                SavePluginState();
            }
            catch
            {
                // Do not block closing if the local preferences file cannot be written.
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> LoadKeyValues(string filePath)
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0)
                {
                    continue;
                }

                yield return new KeyValuePair<string, string>(
                    line.Substring(0, separatorIndex).Trim(),
                    line.Substring(separatorIndex + 1).Trim());
            }
        }

        private static void SaveKeyValues(string filePath, IEnumerable<string> lines)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllLines(filePath, lines.ToArray());
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

        private static string RequireText(string value, string caption)
        {
            value = (value ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(caption + " is required.");
            }

            return value;
        }

        private static IEnumerable<string> GetLines(TextBox textBox)
        {
            return textBox.Lines
                .Select(x => (x ?? string.Empty).Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase);
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

        private static string GetBlacklistFilePath() => Path.Combine(Application.UserAppDataPath, BlacklistFileName);

        private static string GetStateFilePath() => Path.Combine(Application.UserAppDataPath, StateFileName);

        private static string GetPluginStateFilePath() => Path.Combine(Application.UserAppDataPath, PluginStateFileName);

        private static string SerializePluginManifest(PluginPackageManifest manifest)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(PluginPackageManifest));
                serializer.WriteObject(stream, manifest);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static void SavePluginManifest(PluginPackageManifest manifest, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, SerializePluginManifest(manifest), Encoding.UTF8);
        }
    }

    [DataContract]
    internal sealed class PluginPackageManifest
    {
        [DataMember(Name = "id", Order = 1)]
        public string Id { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "version", Order = 3)]
        public string Version { get; set; }

        [DataMember(Name = "description", Order = 4, EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "changelog", Order = 5, EmitDefaultValue = false)]
        public string Changelog { get; set; }

        [DataMember(Name = "publishedAtUtc", Order = 6, EmitDefaultValue = false)]
        public string PublishedAtUtc { get; set; }

        [DataMember(Name = "packagePath", Order = 7)]
        public string PackagePath { get; set; }

        [DataMember(Name = "files", Order = 8)]
        public List<PluginPackageFile> Files { get; set; }
    }

    [DataContract]
    internal sealed class PluginPackageFile
    {
        [DataMember(Name = "path", Order = 1)]
        public string Path { get; set; }

        [DataMember(Name = "targetRoot", Order = 2)]
        public string TargetRoot { get; set; }

        [DataMember(Name = "targetPath", Order = 3)]
        public string TargetPath { get; set; }

        [DataMember(Name = "hash", Order = 4)]
        public string Hash { get; set; }

        [DataMember(Name = "size", Order = 5)]
        public long Size { get; set; }
    }

    internal static class PluginInstallTarget
    {
        public const string Application = "Application";

        public const string ProfilePlugins = "ProfilePlugins";
    }
}
