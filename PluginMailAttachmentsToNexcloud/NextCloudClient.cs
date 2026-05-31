using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PluginMailAttachmentsToNextcloud
{
    public sealed class NextCloudClient
    {
        private readonly CloudSettings _settings;

        public Exception ErrorException { get; set; }

        public NextCloudClient(CloudSettings settings)
        {
            _settings = settings;
        }

        public string Upload(string localFile)
        {
            if (!_settings.Validate())
            {
                ErrorException = new Exception("Check Nextcloud settings");
                return string.Empty;
            }

            var filename = Path.GetFileName(localFile);
            Debug.Write("start upload....");
            var remoteFile = CombineRemotePath(_settings.RemoteFolder, filename);

            using (var stream = new FileStream(localFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (UploadStream(remoteFile, stream, GetMimeType(localFile)))
                {
                    Debug.WriteLine("ok");
                    return ShareWithLink(remoteFile);
                }
            }

            ErrorException = new Exception($"Upload {filename}  failed!");
            return string.Empty;
        }

        public string Upload(FileStream fileStream)
        {
            if (!_settings.Validate())
            {
                return string.Empty;
            }

            var filename = Path.GetFileName(fileStream.Name);
            Debug.Write("start upload....");
            var remoteFile = CombineRemotePath(_settings.RemoteFolder, filename);
            if (UploadStream(remoteFile, fileStream, GetMimeType(fileStream.Name)))
            {
                Debug.WriteLine("ok");
                return ShareWithLink(remoteFile);
            }

            Debug.WriteLine("error");
            return string.Empty;
        }

        public bool Upload(string remoteFolder, FileStream fileStream)
        {
            if (!_settings.Validate())
            {
                return false;
            }

            var filename = Path.GetFileName(fileStream.Name);
            Debug.Write("start upload....");
            var remoteFile = CombineRemotePath(_settings.RemoteFolder, remoteFolder, filename);
            return UploadStream(remoteFile, fileStream, GetMimeType(fileStream.Name));
        }

        public bool CreateFolder(string folder)
        {
            if (!_settings.Validate())
            {
                return false;
            }

            return SendAsync(new HttpRequestMessage(new HttpMethod("MKCOL"), GetDavUri(CombineRemotePath(_settings.RemoteFolder, folder))))
                .GetAwaiter()
                .GetResult()
                .IsSuccessStatusCode;
        }

        public string GetShareFolderLink(string remoteFolder)
        {
            if (!_settings.Validate())
            {
                return string.Empty;
            }

            return ShareWithLink(CombineRemotePath(_settings.RemoteFolder, remoteFolder));
        }

        private bool UploadStream(string remoteFile, Stream stream, string contentType)
        {
            using (var content = new StreamContent(stream))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                using (var request = new HttpRequestMessage(HttpMethod.Put, GetDavUri(remoteFile)) { Content = content })
                {
                    return SendAsync(request).GetAwaiter().GetResult().IsSuccessStatusCode;
                }
            }
        }

        private string ShareWithLink(string remoteFile)
        {
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("path", "/" + remoteFile.Trim('/')),
                new KeyValuePair<string, string>("shareType", "3"),
                new KeyValuePair<string, string>("permissions", "1")
            });

            using (var request = new HttpRequestMessage(HttpMethod.Post, GetOcsUri("/ocs/v2.php/apps/files_sharing/api/v1/shares")) { Content = form })
            {
                request.Headers.Add("OCS-APIRequest", "true");
                var response = SendAsync(request).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                var xml = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return XDocument.Parse(xml).Descendants("url").FirstOrDefault()?.Value ?? string.Empty;
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(_settings.User, _settings.Password),
                PreAuthenticate = true
            };

            using (handler)
            using (var client = new HttpClient(handler))
            {
                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.User}:{_settings.Password}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
                return await client.SendAsync(request).ConfigureAwait(false);
            }
        }

        private Uri GetDavUri(string remoteFile)
        {
            return GetOcsUri("/remote.php/dav/files/" + EscapePath(_settings.User) + "/" + EscapePath(remoteFile));
        }

        private Uri GetOcsUri(string path)
        {
            return new Uri(new Uri(_settings.Server.TrimEnd('/') + "/"), path.TrimStart('/'));
        }

        private static string CombineRemotePath(params string[] parts)
        {
            return string.Join("/", parts.Where(part => !string.IsNullOrWhiteSpace(part)).Select(part => part.Trim('/')));
        }

        private static string EscapePath(string path)
        {
            return string.Join("/", path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(Uri.EscapeDataString));
        }

        private static string GetMimeType(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLowerInvariant())
            {
                case ".bmp": return "image/bmp";
                case ".csv": return "text/csv";
                case ".gif": return "image/gif";
                case ".htm":
                case ".html": return "text/html";
                case ".jpeg":
                case ".jpg": return "image/jpeg";
                case ".json": return "application/json";
                case ".pdf": return "application/pdf";
                case ".png": return "image/png";
                case ".txt": return "text/plain";
                case ".xml": return "application/xml";
                case ".zip": return "application/zip";
                default: return "application/octet-stream";
            }
        }
    }
}