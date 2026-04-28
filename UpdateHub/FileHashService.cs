using System;
using System.IO;
using System.Security.Cryptography;

namespace UpdateHub
{
    public static class FileHashService
    {
        public static string ComputeSha256(string filePath)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
