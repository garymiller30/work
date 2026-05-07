using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.IO.Compression;
using System.Xml.Linq;
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
        private const string PluginCatalogFileName = "plugins.json";

        private readonly TextBox _sourceFolderTextBox;
        private readonly TextBox _publishFolderTextBox;
        private readonly TextBox _versionTextBox;
        private readonly ComboBox _updateTypeComboBox;
        private readonly TextBox _blacklistTextBox;
        private readonly TextBox _changelogTextBox;
        private readonly TextBox _previewTextBox;

        private readonly TextBox _pluginFileTextBox;
        private readonly TextBox _pluginPublishFolderTextBox;
        private readonly TextBox _pluginApplicationFolderTextBox;
        private readonly TextBox _pluginIdTextBox;
        private readonly TextBox _pluginNameTextBox;
        private readonly TextBox _pluginVersionTextBox;
        private readonly TextBox _pluginDescriptionTextBox;
        private readonly TextBox _pluginChangelogTextBox;
        private readonly TextBox _pluginDependencyFilesTextBox;
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

            _pluginFileTextBox = AddPathRow(pluginRoot, 0, "Plugin DLL:", BrowsePluginFile);
            _pluginPublishFolderTextBox = AddPathRow(pluginRoot, 1, "Web publish path:", BrowsePluginPublishFolder);
            _pluginApplicationFolderTextBox = AddPathRow(pluginRoot, 2, "ActiveWorks bin path:", BrowsePluginApplicationFolder);

            _pluginIdTextBox = new TextBox { Dock = DockStyle.Fill };
            AddRow(pluginRoot, 3, "Plugin id:", _pluginIdTextBox);

            _pluginNameTextBox = new TextBox { Dock = DockStyle.Fill };
            AddRow(pluginRoot, 4, "Plugin name:", _pluginNameTextBox);

            _pluginVersionTextBox = new TextBox { Dock = DockStyle.Fill };
            AddRow(pluginRoot, 5, "Version:", _pluginVersionTextBox);

            _pluginDescriptionTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(pluginRoot, 6, "Description:", _pluginDescriptionTextBox, 70);

            _pluginChangelogTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            AddRow(pluginRoot, 7, "Changelog:", _pluginChangelogTextBox, 90);

            _pluginDependencyFilesTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            var dependencyPanel = new Panel { Dock = DockStyle.Fill };
            var dependencyButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 30,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            var addDependenciesButton = new Button { Text = "Add dependency files", AutoSize = true };
            var findDependenciesButton = new Button { Text = "Find dependencies", AutoSize = true };
            var clearDependenciesButton = new Button { Text = "Clear", AutoSize = true };
            addDependenciesButton.Click += (s, e) => AddPluginDependencyFiles();
            findDependenciesButton.Click += (s, e) => FindPluginDependencyFiles();
            clearDependenciesButton.Click += (s, e) => _pluginDependencyFilesTextBox.Clear();
            dependencyButtons.Controls.Add(addDependenciesButton);
            dependencyButtons.Controls.Add(findDependenciesButton);
            dependencyButtons.Controls.Add(clearDependenciesButton);
            _pluginDependencyFilesTextBox.Dock = DockStyle.Fill;
            dependencyPanel.Controls.Add(_pluginDependencyFilesTextBox);
            dependencyPanel.Controls.Add(dependencyButtons);
            AddRow(pluginRoot, 8, "Dependency files:", dependencyPanel, 120);

            var pluginButtonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var readPluginMetadataButton = new Button { Text = "Read Metadata", AutoSize = true };
            var generatePluginButton = new Button { Text = "Generate Plugin Manifest", AutoSize = true };
            var publishPluginButton = new Button { Text = "Publish Plugin", AutoSize = true };
            readPluginMetadataButton.Click += (s, e) => ReadSelectedPluginMetadata(showMessages: true);
            generatePluginButton.Click += (s, e) => GeneratePluginManifestPreview();
            publishPluginButton.Click += (s, e) => PublishPlugin();
            pluginButtonPanel.Controls.Add(readPluginMetadataButton);
            pluginButtonPanel.Controls.Add(generatePluginButton);
            pluginButtonPanel.Controls.Add(publishPluginButton);
            AddRow(pluginRoot, 9, "Actions:", pluginButtonPanel);

            _pluginPreviewTextBox = CreatePreviewTextBox();
            AddRow(pluginRoot, 10, "plugin.json:", _pluginPreviewTextBox, 210);

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

        private void BrowsePluginApplicationFolder(object sender, EventArgs e) => BrowseFolder(_pluginApplicationFolderTextBox);

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

        private void BrowsePluginFile(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Plugin DLL (*.dll)|*.dll|All files (*.*)|*.*";
                dialog.FileName = _pluginFileTextBox.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _pluginFileTextBox.Text = dialog.FileName;
                    ReadSelectedPluginMetadata(showMessages: false);
                }
            }
        }

        private void AddPluginDependencyFiles()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Dependency files (*.dll;*.config;*.json)|*.dll;*.config;*.json|All files (*.*)|*.*";
                dialog.Multiselect = true;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var existing = GetLines(_pluginDependencyFilesTextBox).ToList();
                foreach (var fileName in dialog.FileNames)
                {
                    if (!existing.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    {
                        existing.Add(fileName);
                    }
                }

                _pluginDependencyFilesTextBox.Text = string.Join(Environment.NewLine, existing);
            }
        }

        private void FindPluginDependencyFiles()
        {
            try
            {
                var pluginFile = RequireFile(_pluginFileTextBox.Text, "Plugin DLL");
                var projectFile = FindPluginProjectFile(pluginFile);
                if (string.IsNullOrWhiteSpace(projectFile))
                {
                    MessageBox.Show(this, "Could not find a .csproj for the selected plugin DLL.", "Plugin Publisher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var publishRoot = PublishPluginForDependencyDiscovery(projectFile, pluginFile);
                var applicationFiles = GetApplicationFilesForDependencyFiltering(pluginFile);
                var dependencies = DiscoverPublishedDependencyFiles(projectFile, pluginFile, publishRoot, applicationFiles).ToList();
                var existing = GetLines(_pluginDependencyFilesTextBox).ToList();

                foreach (var dependency in dependencies)
                {
                    var line = FormatDependencyLine(dependency.SourceFilePath, dependency.TargetPath);
                    if (!existing.Contains(line, StringComparer.OrdinalIgnoreCase))
                    {
                        existing.Add(line);
                    }
                }

                _pluginDependencyFilesTextBox.Text = string.Join(Environment.NewLine, existing);
                MessageBox.Show(
                    this,
                    "Found " + dependencies.Count + " dependency file(s)." + Environment.NewLine + publishRoot,
                    "Plugin Publisher",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Plugin Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadSelectedPluginMetadata(bool showMessages)
        {
            var pluginFile = (_pluginFileTextBox.Text ?? string.Empty).Trim();
            if (!File.Exists(pluginFile))
            {
                if (showMessages)
                {
                    MessageBox.Show(this, "Plugin DLL does not exist.", "Plugin Publisher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return;
            }

            try
            {
                var metadata = PluginMetadataReader.Read(pluginFile);
                if (string.IsNullOrWhiteSpace(_pluginIdTextBox.Text))
                {
                    _pluginIdTextBox.Text = Path.GetFileNameWithoutExtension(pluginFile);
                }

                if (!string.IsNullOrWhiteSpace(metadata.Name))
                {
                    _pluginNameTextBox.Text = metadata.Name;
                }
                else if (string.IsNullOrWhiteSpace(_pluginNameTextBox.Text))
                {
                    _pluginNameTextBox.Text = Path.GetFileNameWithoutExtension(pluginFile);
                }

                if (!string.IsNullOrWhiteSpace(metadata.Version))
                {
                    _pluginVersionTextBox.Text = metadata.Version;
                }

                if (!string.IsNullOrWhiteSpace(metadata.Description))
                {
                    _pluginDescriptionTextBox.Text = metadata.Description;
                }
            }
            catch (Exception ex)
            {
                if (showMessages)
                {
                    MessageBox.Show(this, ex.Message, "Plugin Publisher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private static string FindPluginProjectFile(string pluginFile)
        {
            var pluginName = Path.GetFileNameWithoutExtension(pluginFile);
            var directory = new DirectoryInfo(Path.GetDirectoryName(pluginFile));
            while (directory != null)
            {
                foreach (var projectFile in directory.GetFiles("*.csproj"))
                {
                    if (ProjectBuildsAssembly(projectFile.FullName, pluginName))
                    {
                        return projectFile.FullName;
                    }
                }

                directory = directory.Parent;
            }

            return null;
        }

        private static bool ProjectBuildsAssembly(string projectFile, string assemblyName)
        {
            var projectAssemblyName = GetProjectAssemblyName(projectFile);
            return string.Equals(projectAssemblyName, assemblyName, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetProjectAssemblyName(string projectFile)
        {
            try
            {
                var document = XDocument.Load(projectFile);
                var assemblyName = document
                    .Descendants()
                    .FirstOrDefault(x => string.Equals(x.Name.LocalName, "AssemblyName", StringComparison.OrdinalIgnoreCase))
                    ?.Value
                    ?.Trim();

                return string.IsNullOrWhiteSpace(assemblyName)
                    ? Path.GetFileNameWithoutExtension(projectFile)
                    : assemblyName;
            }
            catch
            {
                return Path.GetFileNameWithoutExtension(projectFile);
            }
        }

        private static string PublishPluginForDependencyDiscovery(string projectFile, string pluginFile)
        {
            var configuration = GetBuildConfiguration(pluginFile);
            var outputRoot = Path.Combine(
                Path.GetTempPath(),
                "UpdateHubPublisher",
                "plugin-dependencies",
                Path.GetFileNameWithoutExtension(pluginFile));

            if (Directory.Exists(outputRoot))
            {
                Directory.Delete(outputRoot, true);
            }

            Directory.CreateDirectory(outputRoot);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            startInfo.ArgumentList.Add("publish");
            startInfo.ArgumentList.Add(projectFile);
            startInfo.ArgumentList.Add("-c");
            startInfo.ArgumentList.Add(configuration);
            startInfo.ArgumentList.Add("-o");
            startInfo.ArgumentList.Add(outputRoot);

            using (var process = Process.Start(startInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException("dotnet publish failed." + Environment.NewLine + output + Environment.NewLine + error);
                }
            }

            return outputRoot;
        }

        private static string GetBuildConfiguration(string pluginFile)
        {
            var parts = pluginFile.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return parts.Any(x => string.Equals(x, "Release", StringComparison.OrdinalIgnoreCase))
                ? "Release"
                : "Debug";
        }

        private HashSet<string> GetApplicationFilesForDependencyFiltering(string pluginFile)
        {
            var applicationRoot = ResolveApplicationRoot(pluginFile);
            var files = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(applicationRoot) || !Directory.Exists(applicationRoot))
            {
                return files;
            }

            foreach (var file in Directory.GetFiles(applicationRoot, "*", SearchOption.AllDirectories))
            {
                var relativePath = PathUtility.NormalizeRelativePath(file.Substring(applicationRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                files.Add(relativePath);

                if (relativePath.IndexOf('/') < 0)
                {
                    files.Add(Path.GetFileName(file));
                }
            }

            return files;
        }

        private string ResolveApplicationRoot(string pluginFile)
        {
            var applicationFolder = (_pluginApplicationFolderTextBox.Text ?? string.Empty).Trim();
            if (Directory.Exists(applicationFolder))
            {
                return applicationFolder;
            }

            return null;
        }

        private static IEnumerable<PluginDependencyFile> DiscoverPublishedDependencyFiles(
            string projectFile,
            string pluginFile,
            string publishRoot,
            HashSet<string> applicationFiles)
        {
            var assetPaths = GetPackageRuntimeAssetPaths(projectFile);
            var pluginFileName = Path.GetFileName(pluginFile);
            foreach (var file in Directory.GetFiles(publishRoot, "*", SearchOption.AllDirectories))
            {
                var relativePath = PathUtility.NormalizeRelativePath(file.Substring(publishRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                var fileName = Path.GetFileName(file);
                if (string.Equals(fileName, pluginFileName, StringComparison.OrdinalIgnoreCase) ||
                    IsPluginBuildFile(fileName, pluginFileName))
                {
                    continue;
                }

                if (!assetPaths.Contains(relativePath) && !assetPaths.Contains(fileName))
                {
                    continue;
                }

                if (ApplicationAlreadyContainsDependency(applicationFiles, relativePath))
                {
                    continue;
                }

                yield return new PluginDependencyFile
                {
                    SourceFilePath = file,
                    TargetPath = relativePath
                };
            }
        }

        private static bool ApplicationAlreadyContainsDependency(HashSet<string> applicationFiles, string relativePath)
        {
            if (applicationFiles == null || applicationFiles.Count == 0)
            {
                return false;
            }

            relativePath = PathUtility.NormalizeRelativePath(relativePath);
            if (applicationFiles.Contains(relativePath))
            {
                return true;
            }

            return relativePath.IndexOf('/') < 0 &&
                   applicationFiles.Contains(Path.GetFileName(relativePath));
        }

        private static bool IsPluginBuildFile(string fileName, string pluginFileName)
        {
            var pluginBaseName = Path.GetFileNameWithoutExtension(pluginFileName);
            return string.Equals(fileName, pluginBaseName + ".pdb", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(fileName, pluginBaseName + ".deps.json", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(fileName, pluginBaseName + ".runtimeconfig.json", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(fileName, pluginFileName + ".config", StringComparison.OrdinalIgnoreCase);
        }

        private static HashSet<string> GetPackageRuntimeAssetPaths(string projectFile)
        {
            var packageNames = GetPackageClosure(projectFile);
            var assetPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var assetsFile = Path.Combine(Path.GetDirectoryName(projectFile), "obj", "project.assets.json");
            if (!File.Exists(assetsFile))
            {
                return assetPaths;
            }

            using (var document = JsonDocument.Parse(File.ReadAllText(assetsFile)))
            {
                if (!document.RootElement.TryGetProperty("targets", out var targets))
                {
                    return assetPaths;
                }

                foreach (var target in targets.EnumerateObject())
                {
                    foreach (var package in target.Value.EnumerateObject())
                    {
                        var packageName = package.Name.Split('/')[0];
                        if (!packageNames.Contains(packageName))
                        {
                            continue;
                        }

                        AddRuntimeAssets(package.Value, "runtime", assetPaths);
                        AddRuntimeAssets(package.Value, "runtimeTargets", assetPaths);
                    }

                    break;
                }
            }

            return assetPaths;
        }

        private static void AddRuntimeAssets(JsonElement package, string propertyName, HashSet<string> assetPaths)
        {
            if (!package.TryGetProperty(propertyName, out var assets))
            {
                return;
            }

            foreach (var asset in assets.EnumerateObject())
            {
                if (ShouldIncludeRuntimeAsset(asset.Name, asset.Value))
                {
                    assetPaths.Add(Path.GetFileName(asset.Name));
                    assetPaths.Add(PathUtility.NormalizeRelativePath(asset.Name));
                }
            }
        }

        private static bool ShouldIncludeRuntimeAsset(string assetPath, JsonElement asset)
        {
            var extension = Path.GetExtension(assetPath);
            if (!string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(extension, ".config", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(extension, ".json", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (asset.TryGetProperty("rid", out var rid))
            {
                var ridValue = rid.GetString();
                return !string.IsNullOrWhiteSpace(ridValue) &&
                       ridValue.StartsWith("win", StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }

        private static HashSet<string> GetPackageClosure(string projectFile)
        {
            var packageNames = GetDirectPackageReferences(projectFile);
            var assetsFile = Path.Combine(Path.GetDirectoryName(projectFile), "obj", "project.assets.json");
            if (!File.Exists(assetsFile))
            {
                return packageNames;
            }

            using (var document = JsonDocument.Parse(File.ReadAllText(assetsFile)))
            {
                if (!document.RootElement.TryGetProperty("targets", out var targets))
                {
                    return packageNames;
                }

                JsonElement targetPackages = default;
                foreach (var target in targets.EnumerateObject())
                {
                    targetPackages = target.Value;
                    break;
                }

                if (targetPackages.ValueKind == JsonValueKind.Undefined)
                {
                    return packageNames;
                }

                var queue = new Queue<string>(packageNames);
                while (queue.Count > 0)
                {
                    var packageName = queue.Dequeue();
                    var package = FindPackageTarget(targetPackages, packageName);
                    if (package.ValueKind == JsonValueKind.Undefined ||
                        !package.TryGetProperty("dependencies", out var dependencies))
                    {
                        continue;
                    }

                    foreach (var dependency in dependencies.EnumerateObject())
                    {
                        if (packageNames.Add(dependency.Name))
                        {
                            queue.Enqueue(dependency.Name);
                        }
                    }
                }
            }

            return packageNames;
        }

        private static JsonElement FindPackageTarget(JsonElement targetPackages, string packageName)
        {
            foreach (var package in targetPackages.EnumerateObject())
            {
                if (package.Name.StartsWith(packageName + "/", StringComparison.OrdinalIgnoreCase))
                {
                    return package.Value;
                }
            }

            return default;
        }

        private static HashSet<string> GetDirectPackageReferences(string projectFile)
        {
            var packageNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var document = XDocument.Load(projectFile);
                foreach (var packageReference in document.Descendants().Where(x => string.Equals(x.Name.LocalName, "PackageReference", StringComparison.OrdinalIgnoreCase)))
                {
                    var include = packageReference.Attribute("Include")?.Value ?? packageReference.Attribute("Update")?.Value;
                    if (!string.IsNullOrWhiteSpace(include))
                    {
                        packageNames.Add(include.Trim());
                    }
                }
            }
            catch
            {
                // The caller can still use manually selected dependencies.
            }

            return packageNames;
        }

        private static string FormatDependencyLine(string sourceFile, string targetPath)
        {
            targetPath = PathUtility.NormalizeRelativePath(targetPath);
            return string.Equals(Path.GetFileName(sourceFile), targetPath, StringComparison.OrdinalIgnoreCase)
                ? sourceFile
                : sourceFile + "|" + targetPath;
        }

        private static string GetDependencyTargetPath(string dependencyLine, string fullPath)
        {
            var separatorIndex = (dependencyLine ?? string.Empty).IndexOf('|');
            if (separatorIndex < 0)
            {
                return Path.GetFileName(fullPath);
            }

            var targetPath = dependencyLine.Substring(separatorIndex + 1).Trim();
            return string.IsNullOrWhiteSpace(targetPath)
                ? Path.GetFileName(fullPath)
                : targetPath;
        }

        private static string GetDependencySourcePath(string dependencyLine)
        {
            var separatorIndex = (dependencyLine ?? string.Empty).IndexOf('|');
            return separatorIndex < 0
                ? dependencyLine
                : dependencyLine.Substring(0, separatorIndex).Trim();
        }

        private sealed class PluginDependencyFile
        {
            public string SourceFilePath { get; set; }

            public string TargetPath { get; set; }
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
                var publishRoot = RequireDirectory(_pluginPublishFolderTextBox.Text, "Web publish path");
                var manifest = BuildPluginManifest();
                var pluginRoot = Path.Combine(publishRoot, "Data", "plugins", manifest.Id);
                Directory.CreateDirectory(pluginRoot);
                var packagePath = Path.Combine(pluginRoot, manifest.PackagePath.Replace('/', Path.DirectorySeparatorChar));
                CreatePluginPackageZip(manifest, packagePath);

                ClearSourcePaths(manifest);
                SavePluginManifest(manifest, Path.Combine(pluginRoot, PluginManifestFileName));
                UpdatePluginCatalog(publishRoot, manifest);
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
            var pluginFile = RequireFile(_pluginFileTextBox.Text, "Plugin DLL");
            ReadSelectedPluginMetadata(showMessages: false);

            var pluginId = RequireText(_pluginIdTextBox.Text, "Plugin id");
            var pluginName = RequireText(_pluginNameTextBox.Text, "Plugin name");
            var version = RequireText(_pluginVersionTextBox.Text, "Version");
            var files = new List<PluginPackageFile>();

            files.Add(CreatePluginPackageFile(
                pluginFile,
                Path.GetFileName(pluginFile),
                PluginInstallTarget.ProfilePlugins));

            foreach (var dependencyFile in GetLines(_pluginDependencyFilesTextBox))
            {
                var fullPath = RequireFile(GetDependencySourcePath(dependencyFile), "Dependency file");
                if (string.Equals(fullPath, pluginFile, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                files.Add(CreatePluginPackageFile(
                    fullPath,
                    GetDependencyTargetPath(dependencyFile, fullPath),
                    PluginInstallTarget.Application));
            }

            return new PluginPackageManifest
            {
                Id = pluginId,
                Name = pluginName,
                Version = version,
                Description = (_pluginDescriptionTextBox.Text ?? string.Empty).Trim(),
                Changelog = (_pluginChangelogTextBox.Text ?? string.Empty).Trim(),
                PublishedAtUtc = DateTime.UtcNow.ToString("O"),
                PackagePath = BuildPluginPackageFileName(pluginId, version),
                Files = files
                    .GroupBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
                    .Select(x => x.First())
                    .OrderBy(x => x.TargetRoot, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
                    .ToList()
            };
        }

        private static void CreatePluginPackageZip(PluginPackageManifest manifest, string packagePath)
        {
            if (File.Exists(packagePath))
            {
                File.Delete(packagePath);
            }

            using (var archive = ZipFile.Open(packagePath, ZipArchiveMode.Create, Encoding.UTF8))
            {
                AddTextEntry(archive, "plugin.json", SerializePluginManifest(manifest));
                AddTextEntry(archive, "install-readme.txt", BuildPluginInstallReadme(manifest));

                foreach (var file in manifest.Files)
                {
                    if (string.IsNullOrWhiteSpace(file.SourceFilePath) || !File.Exists(file.SourceFilePath))
                    {
                        throw new FileNotFoundException("Plugin package source file does not exist.", file.SourceFilePath);
                    }

                    archive.CreateEntryFromFile(file.SourceFilePath, GetZipEntryPath(file), CompressionLevel.Optimal);
                }
            }
        }

        private static void AddTextEntry(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
            using (var writer = new StreamWriter(entry.Open(), Encoding.UTF8))
            {
                writer.Write(content);
            }
        }

        private static string GetZipEntryPath(PluginPackageFile file)
        {
            var targetPath = PathUtility.NormalizeRelativePath(string.IsNullOrWhiteSpace(file.TargetPath) ? file.Path : file.TargetPath);
            if (string.Equals(file.TargetRoot, PluginInstallTarget.Application, StringComparison.OrdinalIgnoreCase))
            {
                return PathUtility.NormalizeRelativePath(Path.Combine("Application", targetPath));
            }

            return PathUtility.NormalizeRelativePath(Path.Combine("Profiles", "_PROFILE_", "Plugins", targetPath));
        }

        private static string BuildPluginInstallReadme(PluginPackageManifest manifest)
        {
            var builder = new StringBuilder();
            builder.AppendLine("ActiveWorks plugin package");
            builder.AppendLine();
            builder.AppendLine("Plugin: " + manifest.Name);
            builder.AppendLine("Version: " + manifest.Version);
            builder.AppendLine();
            builder.AppendLine("Application/* goes next to ActiveWorks.exe.");
            builder.AppendLine("Profiles/_PROFILE_/Plugins/* goes to Profiles/<user profile>/Plugins.");
            return builder.ToString();
        }

        private static string BuildPluginPackageFileName(string pluginId, string version)
        {
            return BuildSafeFileName(pluginId + "-" + version) + ".zip";
        }

        private static string BuildSafeFileName(string value)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return new string((value ?? string.Empty)
                .Select(ch => invalid.Contains(ch) ? '-' : ch)
                .ToArray());
        }

        private static PluginPackageFile CreatePluginPackageFile(string sourceFile, string packagePath, string targetRoot)
        {
            var fileInfo = new FileInfo(sourceFile);
            return new PluginPackageFile
            {
                Path = PathUtility.NormalizeRelativePath(packagePath),
                TargetPath = PathUtility.NormalizeRelativePath(packagePath),
                TargetRoot = targetRoot,
                Hash = FileHashService.ComputeSha256(sourceFile),
                Size = fileInfo.Length,
                SourceFilePath = sourceFile
            };
        }

        private static void ClearSourcePaths(PluginPackageManifest manifest)
        {
            foreach (var file in manifest.Files)
            {
                file.SourceFilePath = null;
            }
        }

        private static void UpdatePluginCatalog(string publishRoot, PluginPackageManifest manifest)
        {
            var catalogRoot = Path.Combine(publishRoot, "Data", "plugins");
            Directory.CreateDirectory(catalogRoot);
            var catalogPath = Path.Combine(catalogRoot, PluginCatalogFileName);
            var catalog = LoadPluginCatalog(catalogPath);
            var manifestPath = PathUtility.NormalizeRelativePath(Path.Combine(manifest.Id, PluginManifestFileName));

            foreach (var entry in DiscoverPluginCatalogEntries(catalogRoot))
            {
                catalog.Plugins.RemoveAll(x => string.Equals(x.Id, entry.Id, StringComparison.OrdinalIgnoreCase));
                catalog.Plugins.Add(entry);
            }

            catalog.Plugins.RemoveAll(x => string.Equals(x.Id, manifest.Id, StringComparison.OrdinalIgnoreCase));
            catalog.Plugins.Add(new PluginCatalogEntry
            {
                Id = manifest.Id,
                ManifestPath = manifestPath
            });

            catalog.Plugins = catalog.Plugins
                .OrderBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
                .ToList();

            SavePluginCatalog(catalog, catalogPath);
        }

        private static IEnumerable<PluginCatalogEntry> DiscoverPluginCatalogEntries(string catalogRoot)
        {
            if (!Directory.Exists(catalogRoot))
            {
                yield break;
            }

            foreach (var pluginManifestPath in Directory.GetFiles(catalogRoot, PluginManifestFileName, SearchOption.AllDirectories))
            {
                PluginPackageManifest pluginManifest = null;
                try
                {
                    using (var stream = File.OpenRead(pluginManifestPath))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(PluginPackageManifest));
                        pluginManifest = (PluginPackageManifest)serializer.ReadObject(stream);
                    }
                }
                catch
                {
                    // Keep the catalog usable even if one plugin manifest is temporarily broken.
                }

                var pluginId = pluginManifest != null && !string.IsNullOrWhiteSpace(pluginManifest.Id)
                    ? pluginManifest.Id
                    : Path.GetFileName(Path.GetDirectoryName(pluginManifestPath));

                if (string.IsNullOrWhiteSpace(pluginId))
                {
                    continue;
                }

                var relativePath = pluginManifestPath.Substring(catalogRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                yield return new PluginCatalogEntry
                {
                    Id = pluginId,
                    ManifestPath = PathUtility.NormalizeRelativePath(relativePath)
                };
            }
        }

        private static PluginCatalog LoadPluginCatalog(string catalogPath)
        {
            if (!File.Exists(catalogPath))
            {
                return new PluginCatalog { Plugins = new List<PluginCatalogEntry>() };
            }

            try
            {
                using (var stream = File.OpenRead(catalogPath))
                {
                    var serializer = new DataContractJsonSerializer(typeof(PluginCatalog));
                    var catalog = (PluginCatalog)serializer.ReadObject(stream);
                    if (catalog.Plugins == null)
                    {
                        catalog.Plugins = new List<PluginCatalogEntry>();
                    }

                    return catalog;
                }
            }
            catch
            {
                return new PluginCatalog { Plugins = new List<PluginCatalogEntry>() };
            }
        }

        private static void SavePluginCatalog(PluginCatalog catalog, string catalogPath)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(PluginCatalog));
                serializer.WriteObject(stream, catalog);
                File.WriteAllText(catalogPath, Encoding.UTF8.GetString(stream.ToArray()), Encoding.UTF8);
            }
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
                if (item.Key.Equals("PluginFile", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginFileTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("PluginPublishFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginPublishFolderTextBox.Text = item.Value;
                }
                else if (item.Key.Equals("PluginApplicationFolder", StringComparison.OrdinalIgnoreCase))
                {
                    _pluginApplicationFolderTextBox.Text = item.Value;
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
                "PluginFile=" + ((_pluginFileTextBox.Text ?? string.Empty).Trim()),
                "PluginPublishFolder=" + ((_pluginPublishFolderTextBox.Text ?? string.Empty).Trim()),
                "PluginApplicationFolder=" + ((_pluginApplicationFolderTextBox.Text ?? string.Empty).Trim()),
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

        private static string RequireFile(string path, string caption)
        {
            path = (path ?? string.Empty).Trim();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(caption + " does not exist.", path);
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

    internal sealed class PluginMetadata
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }
    }

    internal static class PluginMetadataReader
    {
        public static PluginMetadata Read(string pluginFile)
        {
            var assembly = Assembly.LoadFrom(pluginFile);
            var metadata = new PluginMetadata
            {
                Name = GetAttribute<AssemblyTitleAttribute>(assembly)?.Title,
                Description = GetAttribute<AssemblyDescriptionAttribute>(assembly)?.Description,
                Version = assembly.GetName().Version?.ToString()
            };

            foreach (var type in assembly.GetTypes())
            {
                var pluginNameProperty = type.GetProperty("PluginName", BindingFlags.Public | BindingFlags.Instance);
                var pluginDescriptionProperty = type.GetProperty("PluginDescription", BindingFlags.Public | BindingFlags.Instance);
                if (pluginNameProperty == null && pluginDescriptionProperty == null)
                {
                    continue;
                }

                if (type.GetConstructor(Type.EmptyTypes) == null || type.IsAbstract)
                {
                    continue;
                }

                try
                {
                    var instance = Activator.CreateInstance(type);
                    var pluginName = pluginNameProperty?.GetValue(instance, null) as string;
                    var pluginDescription = pluginDescriptionProperty?.GetValue(instance, null) as string;

                    if (!string.IsNullOrWhiteSpace(pluginName))
                    {
                        metadata.Name = pluginName;
                    }

                    if (!string.IsNullOrWhiteSpace(pluginDescription))
                    {
                        metadata.Description = pluginDescription;
                    }

                    break;
                }
                catch
                {
                    // Fall back to assembly attributes when plugin construction needs runtime services.
                }
            }

            return metadata;
        }

        private static T GetAttribute<T>(Assembly assembly) where T : Attribute
        {
            return assembly.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();
        }
    }

    [DataContract]
    internal sealed class PluginCatalog
    {
        [DataMember(Name = "plugins", Order = 1)]
        public List<PluginCatalogEntry> Plugins { get; set; }
    }

    [DataContract]
    internal sealed class PluginCatalogEntry
    {
        [DataMember(Name = "id", Order = 1)]
        public string Id { get; set; }

        [DataMember(Name = "manifestPath", Order = 2)]
        public string ManifestPath { get; set; }
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

        [IgnoreDataMember]
        public string SourceFilePath { get; set; }
    }

    internal static class PluginInstallTarget
    {
        public const string Application = "Application";

        public const string ProfilePlugins = "ProfilePlugins";
    }
}
