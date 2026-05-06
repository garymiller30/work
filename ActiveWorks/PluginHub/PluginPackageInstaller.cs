using System;
using System.IO;
using System.IO.Compression;

namespace ActiveWorks.PluginHub
{
    internal sealed class PluginInstallResult
    {
        public int InstalledFilesCount { get; set; }

        public int PendingFilesCount { get; set; }

        public bool RestartRequired { get { return PendingFilesCount > 0 || InstalledFilesCount > 0; } }
    }

    internal sealed class PluginPackageInstaller
    {
        private const string PendingExtension = ".pending";
        private readonly string _applicationDirectory;
        private readonly string _profilePluginsDirectory;

        public PluginPackageInstaller(string applicationDirectory, string profilePluginsDirectory)
        {
            _applicationDirectory = Path.GetFullPath(applicationDirectory);
            _profilePluginsDirectory = Path.GetFullPath(profilePluginsDirectory);
        }

        public PluginInstallResult Install(string packageFilePath)
        {
            var result = new PluginInstallResult();
            Directory.CreateDirectory(_profilePluginsDirectory);

            using (var archive = ZipFile.OpenRead(packageFilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (string.IsNullOrWhiteSpace(entry.Name))
                    {
                        continue;
                    }

                    string targetPath;
                    if (!TryGetTargetPath(entry.FullName, out targetPath))
                    {
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                    try
                    {
                        entry.ExtractToFile(targetPath, true);
                        result.InstalledFilesCount++;
                    }
                    catch (IOException)
                    {
                        entry.ExtractToFile(targetPath + PendingExtension, true);
                        result.PendingFilesCount++;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        entry.ExtractToFile(targetPath + PendingExtension, true);
                        result.PendingFilesCount++;
                    }
                }
            }

            return result;
        }

        private bool TryGetTargetPath(string entryPath, out string targetPath)
        {
            targetPath = null;
            var normalized = (entryPath ?? string.Empty).Replace('\\', '/').TrimStart('/');

            const string applicationPrefix = "Application/";
            const string profilePrefix = "Profiles/_PROFILE_/Plugins/";

            if (normalized.StartsWith(applicationPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var relativePath = normalized.Substring(applicationPrefix.Length);
                targetPath = ResolveChildPath(_applicationDirectory, relativePath);
                return true;
            }

            if (normalized.StartsWith(profilePrefix, StringComparison.OrdinalIgnoreCase))
            {
                var relativePath = normalized.Substring(profilePrefix.Length);
                targetPath = ResolveChildPath(_profilePluginsDirectory, relativePath);
                return true;
            }

            return false;
        }

        private static string ResolveChildPath(string root, string relativePath)
        {
            var rootFullPath = Path.GetFullPath(root);
            var fullPath = Path.GetFullPath(Path.Combine(rootFullPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));
            var prefix = rootFullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!fullPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Package entry points outside of target directory.");
            }

            return fullPath;
        }

        public static int ApplyPendingFiles(string applicationDirectory, string profilesDirectory)
        {
            var count = 0;
            count += ApplyPendingFilesInDirectory(applicationDirectory);
            if (Directory.Exists(profilesDirectory))
            {
                count += ApplyPendingFilesInDirectory(profilesDirectory);
            }

            return count;
        }

        private static int ApplyPendingFilesInDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }

            var count = 0;
            foreach (var pendingFile in Directory.GetFiles(directory, "*" + PendingExtension, SearchOption.AllDirectories))
            {
                var targetFile = pendingFile.Substring(0, pendingFile.Length - PendingExtension.Length);
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                    File.Copy(pendingFile, targetFile, true);
                    File.Delete(pendingFile);
                    count++;
                }
                catch
                {
                    // The file may still be locked. Keep it pending for the next start.
                }
            }

            return count;
        }
    }
}
