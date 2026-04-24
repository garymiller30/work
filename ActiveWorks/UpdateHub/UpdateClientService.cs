using ActiveWorks.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using UpdateHub;

namespace ActiveWorks.UpdateHub
{
    internal sealed class UpdateClientService
    {
        private readonly string _manifestUrl;
        private readonly string _applicationDirectory;

        public UpdateClientService(string manifestUrl, string applicationDirectory)
        {
            _manifestUrl = manifestUrl;
            _applicationDirectory = applicationDirectory;
        }

        public bool IsConfigured =>
            Settings.Default.UpdateHubEnabled &&
            !string.IsNullOrWhiteSpace(_manifestUrl);

        public async Task<UpdateCheckResult> CheckForUpdatesAsync()
        {
            if (!IsConfigured)
            {
                return UpdateCheckResult.Disabled();
            }

            UpdateManifest manifest;
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                var json = await client.DownloadStringTaskAsync(new Uri(_manifestUrl)).ConfigureAwait(false);
                manifest = UpdateManifestSerializer.Deserialize(json);
            }

            var localVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0);
            if (!Version.TryParse(manifest.Version, out var serverVersion) || serverVersion <= localVersion)
            {
                return UpdateCheckResult.NoUpdate();
            }

            var filesToDownload = new List<UpdateManifestFile>();
            foreach (var file in manifest.Files ?? Enumerable.Empty<UpdateManifestFile>())
            {
                var localPath = Path.Combine(_applicationDirectory, file.Path.Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(localPath))
                {
                    filesToDownload.Add(file);
                    continue;
                }

                if (!string.Equals(FileHashService.ComputeSha256(localPath), file.Hash, StringComparison.OrdinalIgnoreCase))
                {
                    filesToDownload.Add(file);
                }
            }

            if (filesToDownload.Count == 0)
            {
                return UpdateCheckResult.NoUpdate();
            }

            var tempRoot = Path.Combine(_applicationDirectory, "temp_update", manifest.Version);
            if (Directory.Exists(tempRoot))
            {
                Directory.Delete(tempRoot, true);
            }

            Directory.CreateDirectory(tempRoot);
            var manifestBaseUri = new Uri(_manifestUrl);
            var packagePath = PathUtility.EnsureTrailingSlash(manifest.PackagePath);

            using (var client = new WebClient())
            {
                foreach (var file in filesToDownload)
                {
                    var relativeUrl = PathUtility.CombineUrl(packagePath, file.Path);
                    var fileUri = new Uri(manifestBaseUri, relativeUrl);
                    var targetPath = Path.Combine(tempRoot, file.Path.Replace('/', Path.DirectorySeparatorChar));
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                    await client.DownloadFileTaskAsync(fileUri, targetPath).ConfigureAwait(false);
                }
            }

            UpdateManifestSerializer.SaveToFile(manifest, Path.Combine(tempRoot, "_update_manifest.json"));

            return UpdateCheckResult.Available(manifest, tempRoot, filesToDownload.Count);
        }

        public static void StartUpdaterAndExit(string updateFolder)
        {
            var updaterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater.exe");
            if (!File.Exists(updaterPath))
            {
                throw new FileNotFoundException("Updater.exe was not found next to the application.", updaterPath);
            }

            var currentProcess = Process.GetCurrentProcess();
            Process.Start(new ProcessStartInfo
            {
                FileName = updaterPath,
                Arguments = currentProcess.Id + " \"" + updateFolder + "\"",
                UseShellExecute = false,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
            });
        }
    }

    internal sealed class UpdateCheckResult
    {
        public bool IsUpdateAvailable { get; private set; }
        public bool IsDisabled { get; private set; }
        public UpdateManifest Manifest { get; private set; }
        public string DownloadFolder { get; private set; }
        public int ChangedFilesCount { get; private set; }

        public static UpdateCheckResult Disabled()
        {
            return new UpdateCheckResult { IsDisabled = true };
        }

        public static UpdateCheckResult NoUpdate()
        {
            return new UpdateCheckResult();
        }

        public static UpdateCheckResult Available(UpdateManifest manifest, string downloadFolder, int changedFilesCount)
        {
            return new UpdateCheckResult
            {
                IsUpdateAvailable = true,
                Manifest = manifest,
                DownloadFolder = downloadFolder,
                ChangedFilesCount = changedFilesCount
            };
        }
    }
}
