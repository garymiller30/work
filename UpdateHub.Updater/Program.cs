using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Updater
{
    internal static class Program
    {
        private static readonly HashSet<string> ProtectedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "_update_manifest.json",
            "user.config",
            "local_settings.json"
        };

        [STAThread]
        private static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                return 1;
            }

            if (!int.TryParse(args[0], out var processId))
            {
                return 2;
            }

            var updateDirectory = args[1];
            if (!Directory.Exists(updateDirectory))
            {
                return 3;
            }

            Process process;
            string mainProcessPath;

            try
            {
                process = Process.GetProcessById(processId);
                mainProcessPath = process.MainModule.FileName;
            }
            catch
            {
                return 4;
            }

            WaitForProcessExit(process);

            var applicationDirectory = Path.GetDirectoryName(mainProcessPath);
            if (string.IsNullOrWhiteSpace(applicationDirectory))
            {
                return 5;
            }

            CopyUpdateFiles(updateDirectory, applicationDirectory);

            try
            {
                Process.Start(mainProcessPath);
            }
            catch
            {
                return 6;
            }

            return 0;
        }

        private static void WaitForProcessExit(Process process)
        {
            while (!process.HasExited)
            {
                Thread.Sleep(500);
                process.Refresh();
            }
        }

        private static void CopyUpdateFiles(string sourceDirectory, string targetDirectory)
        {
            foreach (var sourceFile in Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories))
            {
                var relativePath = sourceFile.Substring(sourceDirectory.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var fileName = Path.GetFileName(relativePath);

                if (ProtectedFiles.Contains(fileName))
                {
                    continue;
                }

                var targetFile = Path.Combine(targetDirectory, relativePath);
                var targetFolder = Path.GetDirectoryName(targetFile);
                if (!string.IsNullOrWhiteSpace(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                var success = false;
                var retries = 5;
                while (!success && retries-- > 0)
                {
                    try
                    {
                        File.Copy(sourceFile, targetFile, true);
                        success = true;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(500);
                    }
                }

                if (!success)
                {
                    throw new IOException("Failed to copy file: " + relativePath);
                }
            }
        }
    }
}
