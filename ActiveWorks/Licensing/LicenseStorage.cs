using System;
using System.IO;

namespace ActiveWorks.Licensing
{
    internal sealed class LicenseStorage
    {
        private readonly string _filePath;

        public LicenseStorage()
        {
            _filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ActiveWorks",
                "license.json");
        }

        public LicenseStorageState Load()
        {
            return LoadFromFile() ?? new LicenseStorageState();
        }

        public void Save(LicenseStorageState state)
        {
            state.UpdatedAtUtc = DateTime.UtcNow.ToString("O");
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_filePath, LicenseJsonSerializer.Serialize(state), System.Text.Encoding.UTF8);
        }

        private LicenseStorageState LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            try
            {
                return LicenseJsonSerializer.Deserialize<LicenseStorageState>(File.ReadAllText(_filePath, System.Text.Encoding.UTF8));
            }
            catch
            {
                return null;
            }
        }
    }
}
