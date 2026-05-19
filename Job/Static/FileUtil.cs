using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JobSpace.Static
{
    static public class FileUtil
    {
        const int ERROR_ACCESS_DENIED = 5;
        const int ERROR_MORE_DATA = 234;
        const int ERROR_CANCELLED = 1223;
        const int MAX_RESTART_MANAGER_RESOURCES = 1000;

        [StructLayout(LayoutKind.Sequential)]
        struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        const int RmRebootReasonNone = 0;
        const int CCH_RM_MAX_APP_NAME = 255;
        const int CCH_RM_MAX_SVC_NAME = 63;

        enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;

            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        static extern int RmRegisterResources(uint pSessionHandle,
                                              UInt32 nFiles,
                                              string[] rgsFilenames,
                                              UInt32 nApplications,
                                              [In] RM_UNIQUE_PROCESS[] rgApplications,
                                              UInt32 nServices,
                                              string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        static extern int RmGetList(uint dwSessionHandle,
                                    out uint pnProcInfoNeeded,
                                    ref uint pnProcInfo,
                                    [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
                                    ref uint lpdwRebootReasons);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        static extern int WNetGetConnection(string localName, StringBuilder remoteName, ref int length);

        /// <summary>
        /// Find out what process(es) have a lock on the specified file.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <returns>Processes locking the file</returns>
        /// <remarks>See also:
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa373661(v=vs.85).aspx
        /// http://wyupdate.googlecode.com/svn-history/r401/trunk/frmFilesInUse.cs (no copyright in code at time of viewing)
        /// 
        /// </remarks>
        static public List<Process> WhoIsLocking(string path)
        {
            uint handle;
            string key = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();

            int res = RmStartSession(out handle, 0, key);

            if (res != 0)
                throw new Exception("Could not begin restart session.  Unable to determine file locker.");

            try
            {
                uint pnProcInfoNeeded = 0,
                     pnProcInfo = 0,
                     lpdwRebootReasons = RmRebootReasonNone;

                string[] resources = GetRestartManagerResources(path);

                if (resources.Length == 0)
                {
                    return processes;
                }

                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);

                if (res != 0)
                    throw new Exception("Could not register resource.");

                for (int attempt = 0; attempt < 3; attempt++)
                {
                    pnProcInfoNeeded = 0;
                    pnProcInfo = 0;

                    // The first call asks Restart Manager how much space is needed.
                    // It can legitimately return 0 when no locking processes are found.
                    res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                    if (res == 0)
                    {
                        return processes;
                    }

                    if (res != ERROR_MORE_DATA)
                    {
                        throw new RestartManagerException(res);
                    }

                    RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;

                    res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);

                    if (res == ERROR_MORE_DATA)
                    {
                        continue;
                    }

                    if (res != 0)
                    {
                        throw new RestartManagerException(res);
                    }

                    processes = new List<Process>((int)pnProcInfo);

                    for (int i = 0; i < pnProcInfo; i++)
                    {
                        try
                        {
                            processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                        }
                        // catch the error -- in case the process is no longer running
                        catch (ArgumentException) { }
                    }

                    return processes;
                }

                throw new Exception("Could not list processes locking resource. The process list changed while reading it.");
            }
            finally
            {
                RmEndSession(handle);
            }

        }
        public static string GetNamesWhoBlock(string path, bool useElevatedFallback = true)
        {
            try
            {
                var processes = WhoIsLocking(path);
                var processNames = processes.Select(GetProcessDisplayName).Distinct().ToList();

                if (processNames.Count == 0)
                {
                    return "процеси не знайдено (можливо, теку вже звільнили або Windows не показала власника блокування)";
                }

                return string.Join(Environment.NewLine, processNames);
            }
            catch (RestartManagerException e) when (e.ErrorCode == ERROR_ACCESS_DENIED && useElevatedFallback)
            {
                var elevatedResult = TryGetNamesWhoBlockElevated(path);

                if (!string.IsNullOrWhiteSpace(elevatedResult))
                {
                    return elevatedResult;
                }

                return "[не вдалося отримати список]\nWindows відмовила в доступі до списку процесів. Спроба отримати список з підвищеними правами також не вдалася або була скасована.";
            }
            catch (Exception e)
            {

                return $"[не вдалося отримати список]\n{e.Message}";
            }
            
        }

        private static string GetProcessDisplayName(Process process)
        {
            try
            {
                return $"{process.ProcessName} (PID {process.Id})";
            }
            catch
            {
                return $"PID {process.Id}";
            }
        }

        private static string TryGetNamesWhoBlockElevated(string path)
        {
            string outputPath = Path.Combine(Path.GetTempPath(), $"activeworks-locking-processes-{Guid.NewGuid():N}.txt");

            try
            {
                using (var process = Process.Start(new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    Arguments = $"--activeworks-list-locking-processes {QuoteArgument(ConvertMappedDriveToUnc(path))} {QuoteArgument(outputPath)}",
                    Verb = "runas",
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }))
                {
                    if (process == null || !process.WaitForExit(60000) || !File.Exists(outputPath))
                    {
                        return null;
                    }
                }

                return File.ReadAllText(outputPath, Encoding.UTF8);
            }
            catch (Win32Exception e) when (e.NativeErrorCode == ERROR_CANCELLED)
            {
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                try
                {
                    if (File.Exists(outputPath))
                    {
                        File.Delete(outputPath);
                    }
                }
                catch
                {
                }
            }
        }

        private static string[] GetRestartManagerResources(string path)
        {
            if (!Directory.Exists(path))
            {
                return new string[] { path };
            }

            var resources = new List<string>();
            AddFiles(path, resources);
            return resources.ToArray();
        }

        private static void AddFiles(string directory, List<string> resources)
        {
            if (resources.Count >= MAX_RESTART_MANAGER_RESOURCES)
            {
                return;
            }

            try
            {
                foreach (string file in Directory.EnumerateFiles(directory))
                {
                    resources.Add(file);

                    if (resources.Count >= MAX_RESTART_MANAGER_RESOURCES)
                    {
                        return;
                    }
                }
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is IOException)
            {
            }

            try
            {
                foreach (string childDirectory in Directory.EnumerateDirectories(directory))
                {
                    AddFiles(childDirectory, resources);

                    if (resources.Count >= MAX_RESTART_MANAGER_RESOURCES)
                    {
                        return;
                    }
                }
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is IOException)
            {
            }
        }

        private static string ConvertMappedDriveToUnc(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || path.Length < 3 || path[1] != ':' || path[2] != '\\')
            {
                return path;
            }

            string drive = path.Substring(0, 2);
            int length = 512;
            var remoteName = new StringBuilder(length);
            int result = WNetGetConnection(drive, remoteName, ref length);

            if (result == ERROR_MORE_DATA)
            {
                remoteName = new StringBuilder(length);
                result = WNetGetConnection(drive, remoteName, ref length);
            }

            if (result != 0)
            {
                return path;
            }

            return Path.Combine(remoteName.ToString(), path.Substring(3));
        }

        private static string QuoteArgument(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\"";
            }

            var quoted = new StringBuilder("\"");
            int backslashes = 0;

            foreach (char character in value)
            {
                if (character == '\\')
                {
                    backslashes++;
                    continue;
                }

                if (character == '"')
                {
                    quoted.Append('\\', backslashes * 2 + 1);
                    quoted.Append(character);
                    backslashes = 0;
                    continue;
                }

                quoted.Append('\\', backslashes);
                quoted.Append(character);
                backslashes = 0;
            }

            quoted.Append('\\', backslashes * 2);
            quoted.Append('"');

            return quoted.ToString();
        }

        class RestartManagerException : Exception
        {
            public RestartManagerException(int errorCode)
                : base($"Could not list processes locking resource. RmGetList failed with code {errorCode}.")
            {
                ErrorCode = errorCode;
            }

            public int ErrorCode { get; }
        }

    }
}
