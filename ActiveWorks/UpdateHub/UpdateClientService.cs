using ActiveWorks.Properties;
using ActiveWorks.Licensing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using UpdateHub;

namespace ActiveWorks.UpdateHub
{
    internal sealed class UpdateClientService
    {
        private readonly string _manifestUrl;
        private readonly string _applicationDirectory;
        private readonly LicenseClientService _licenseClientService;

        public UpdateClientService(string manifestUrl, string applicationDirectory, LicenseClientService licenseClientService)
        {
            _manifestUrl = manifestUrl;
            _applicationDirectory = applicationDirectory;
            _licenseClientService = licenseClientService;
        }

        public bool IsConfigured =>
            Settings.Default.UpdateHubEnabled &&
            (!string.IsNullOrWhiteSpace(_manifestUrl) ||
             Settings.Default.LicenseServerEnabled && !string.IsNullOrWhiteSpace(Settings.Default.LicenseServerUrl));

        public async Task<UpdateCheckResult> CheckForUpdatesAsync()
        {
            if (!IsConfigured)
            {
                return UpdateCheckResult.Disabled();
            }

            var license = await _licenseClientService.GetUsableTokenAsync().ConfigureAwait(false);
            if (!license.AllowsUpdates)
            {
                return UpdateCheckResult.AccessDenied(license.Message);
            }

            var manifest = await DownloadManifestAsync(license.Token).ConfigureAwait(false);

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
            var manifestBaseUri = GetManifestUri();
            var packagePath = PathUtility.EnsureTrailingSlash(manifest.PackagePath);

            using (var client = CreateWebClient(license.Token))
            {
                foreach (var file in filesToDownload)
                {
                    var relativeUrl = PathUtility.CombineUrl(packagePath, file.Path);
                    var fileUri = BuildDownloadUri(manifestBaseUri, relativeUrl);
                    var targetPath = Path.Combine(tempRoot, file.Path.Replace('/', Path.DirectorySeparatorChar));
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                    await client.DownloadFileTaskAsync(fileUri, targetPath).ConfigureAwait(false);
                }
            }

            UpdateManifestSerializer.SaveToFile(manifest, Path.Combine(tempRoot, "_update_manifest.json"));

            return UpdateCheckResult.Available(manifest, tempRoot, filesToDownload.Count);
        }

        public async Task<UpdateManifest> DownloadManifestAsync()
        {
            var license = await _licenseClientService.GetUsableTokenAsync().ConfigureAwait(false);
            if (!license.AllowsUpdates)
            {
                throw new InvalidOperationException("License does not allow updates. " + license.Message);
            }

            return await DownloadManifestAsync(license.Token).ConfigureAwait(false);
        }

        private async Task<UpdateManifest> DownloadManifestAsync(string licenseToken)
        {
            using (var client = CreateWebClient(licenseToken))
            {
                client.Encoding = System.Text.Encoding.UTF8;
                var json = await client.DownloadStringTaskAsync(GetManifestUri()).ConfigureAwait(false);
                return UpdateManifestSerializer.Deserialize(json);
            }
        }

        private Uri GetManifestUri()
        {
            if (Settings.Default.LicenseServerEnabled && !string.IsNullOrWhiteSpace(Settings.Default.LicenseServerUrl))
            {
                return new Uri(new Uri(PathUtility.EnsureTrailingSlash(Settings.Default.LicenseServerUrl)), "api/updates/manifest");
            }

            return new Uri(_manifestUrl);
        }

        private static Uri BuildDownloadUri(Uri manifestBaseUri, string relativeUrl)
        {
            if (Settings.Default.LicenseServerEnabled && !string.IsNullOrWhiteSpace(Settings.Default.LicenseServerUrl))
            {
                return new Uri(
                    new Uri(PathUtility.EnsureTrailingSlash(Settings.Default.LicenseServerUrl)),
                    "api/updates/download/" + relativeUrl.TrimStart('/'));
            }

            return new Uri(manifestBaseUri, relativeUrl);
        }

        private static WebClient CreateWebClient(string licenseToken)
        {
            var client = new WebClient();
            if (!string.IsNullOrWhiteSpace(licenseToken))
            {
                client.Headers[HttpRequestHeader.Authorization] = "Bearer " + licenseToken;
            }

            return client;
        }

        public static void StartUpdaterAndExit(string updateFolder)
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var updaterPath = Path.Combine(applicationDirectory, "Updater.exe");
            if (!File.Exists(updaterPath))
            {
                throw new FileNotFoundException("Updater.exe was not found next to the application.", updaterPath);
            }

            DeleteOldUpdaterRunnerDirectories(applicationDirectory);

            var updaterRunnerDirectory = Path.Combine(
                applicationDirectory,
                "temp_update",
                "updater_runner_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(updaterRunnerDirectory);

            var updaterRunnerPath = Path.Combine(updaterRunnerDirectory, "Updater.exe");
            File.Copy(updaterPath, updaterRunnerPath, true);

            var currentProcess = Process.GetCurrentProcess();
            Process.Start(new ProcessStartInfo
            {
                FileName = updaterRunnerPath,
                Arguments = currentProcess.Id + " \"" + updateFolder + "\"",
                UseShellExecute = false,
                WorkingDirectory = applicationDirectory
            });
        }

        private static void DeleteOldUpdaterRunnerDirectories(string applicationDirectory)
        {
            var tempUpdateDirectory = Path.Combine(applicationDirectory, "temp_update");
            if (!Directory.Exists(tempUpdateDirectory))
            {
                return;
            }

            foreach (var directory in Directory.GetDirectories(tempUpdateDirectory, "updater_runner_*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch
                {
                    // A runner can still be locked if an update is in progress; it will be cleaned up next time.
                }
            }
        }
    }

    internal sealed class UpdateCheckResult
    {
        public bool IsUpdateAvailable { get; private set; }
        public bool IsDisabled { get; private set; }
        public bool IsAccessDenied { get; private set; }
        public string AccessDeniedReason { get; private set; }
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

        public static UpdateCheckResult AccessDenied(string reason)
        {
            return new UpdateCheckResult
            {
                IsAccessDenied = true,
                AccessDeniedReason = reason
            };
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
