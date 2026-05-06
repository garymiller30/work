using ActiveWorks.Licensing;
using ActiveWorks.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace ActiveWorks.PluginHub
{
    internal sealed class PluginCatalogClient
    {
        private readonly LicenseClientService _licenseClientService;

        public PluginCatalogClient(LicenseClientService licenseClientService)
        {
            _licenseClientService = licenseClientService;
        }

        public bool IsConfigured
        {
            get
            {
                return _licenseClientService != null &&
                       _licenseClientService.IsConfigured &&
                       !string.IsNullOrWhiteSpace(Settings.Default.LicenseServerUrl);
            }
        }

        public async Task<List<PluginCatalogItem>> GetCatalogAsync()
        {
            var token = await GetUpdateTokenAsync().ConfigureAwait(false);

            using (var client = CreateClient(token))
            {
                var uri = BuildUri("api/plugins");
                var json = await client.GetStringAsync(uri).ConfigureAwait(false);
                return DeserializeList(json);
            }
        }

        public async Task DownloadPackageAsync(string pluginId, string destinationFilePath)
        {
            if (string.IsNullOrWhiteSpace(pluginId))
            {
                throw new ArgumentException("Plugin id is empty.", "pluginId");
            }

            var token = await GetUpdateTokenAsync().ConfigureAwait(false);

            using (var client = CreateClient(token))
            {
                var uri = BuildUri("api/plugins/" + Uri.EscapeDataString(pluginId) + "/download");
                var bytes = await client.GetByteArrayAsync(uri).ConfigureAwait(false);
                var directory = Path.GetDirectoryName(destinationFilePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllBytes(destinationFilePath, bytes);
            }
        }

        private async Task<string> GetUpdateTokenAsync()
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException("License server is not configured.");
            }

            var license = await _licenseClientService.GetUsableTokenAsync().ConfigureAwait(false);
            if (!license.AllowsUpdates)
            {
                throw new InvalidOperationException("Оновлення недоступні для цієї ліцензії. " + license.Message);
            }

            return license.Token;
        }

        private static HttpClient CreateClient(string token)
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        private static Uri BuildUri(string relativePath)
        {
            var baseUri = Settings.Default.LicenseServerUrl;
            if (!baseUri.EndsWith("/", StringComparison.Ordinal))
            {
                baseUri += "/";
            }

            return new Uri(new Uri(baseUri), relativePath);
        }

        private static List<PluginCatalogItem> DeserializeList(string json)
        {
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(List<PluginCatalogItem>));
                return (List<PluginCatalogItem>)serializer.ReadObject(stream);
            }
        }
    }
}
