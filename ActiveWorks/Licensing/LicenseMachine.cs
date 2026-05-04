using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace ActiveWorks.Licensing
{
    internal static class LicenseMachine
    {
        public static string GetMachineId()
        {
            var parts = new List<string>
            {
                ReadMachineGuid(),
                ReadWmiValue("Win32_ComputerSystemProduct", "UUID"),
                ReadWmiValue("Win32_BaseBoard", "SerialNumber"),
                Environment.MachineName,
                Environment.UserDomainName,
                Environment.UserName
            };

            var raw = string.Join("|", parts.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));
            using (var sha = SHA256.Create())
            {
                return Base64Url.Encode(sha.ComputeHash(Encoding.UTF8.GetBytes(raw)));
            }
        }

        private static string ReadMachineGuid()
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
                {
                    return key?.GetValue("MachineGuid") as string;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string ReadWmiValue(string className, string propertyName)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT " + propertyName + " FROM " + className))
                using (var results = searcher.Get())
                {
                    foreach (ManagementObject item in results)
                    {
                        using (item)
                        {
                            var value = item[propertyName]?.ToString();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                return value;
                            }
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
