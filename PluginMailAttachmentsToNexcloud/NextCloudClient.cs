using owncloudsharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

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
            if (_settings.Validate())
            {
                var client = new Client(_settings.Server, _settings.User, _settings.Password);
                var filename = Path.GetFileName(localFile);
                Debug.Write("start upload....");
                var remoteFile = $"{_settings.RemoteFolder}/{filename}";
                var contentType = MimeMapping.GetMimeMapping(localFile);
                var res = client.Upload(remoteFile, new FileStream(localFile, FileMode.Open), contentType);
                if (res)
                {
                    Debug.WriteLine("ok");
                    var link = client.ShareWithLink(remoteFile, Convert.ToInt32(OcsPermission.All));

                    return link.Url;
                }

                ErrorException = new Exception($"Upload {filename}  failed!");
            }
            else
            {
                ErrorException = new Exception("Check Nextcloud settings");
            }

            return string.Empty;
        }

        public string Upload(FileStream fileStream)
        {
            if (_settings.Validate())
            {
                var client = new Client(_settings.Server, _settings.User, _settings.Password);
                var filename = Path.GetFileName(fileStream.Name);
                Debug.Write("start upload....");
                var remoteFile = $"{_settings.RemoteFolder}/{filename}";
                var contentType = MimeMapping.GetMimeMapping(fileStream.Name);
                var res = client.Upload(remoteFile, fileStream, contentType);
                if (res)
                {
                    Debug.WriteLine("ok");

                    var link = client.ShareWithLink(remoteFile, Convert.ToInt32(OcsPermission.All));

                    return link.Url;
                }
                else
                {
                    Debug.WriteLine("error");
                }
            }
            else
            {

            }

            return string.Empty;
        }


        public bool Upload(string remoteFolder, FileStream fileStream)
        {
            if (_settings.Validate())
            {
                var client = new Client(_settings.Server, _settings.User, _settings.Password);
                var filename = Path.GetFileName(fileStream.Name);
                Debug.Write("start upload....");
                var remoteFile = $"{_settings.RemoteFolder}/{remoteFolder}/{filename}";
                var contentType = MimeMapping.GetMimeMapping(fileStream.Name);
                var res = client.Upload(remoteFile, fileStream, contentType);

                return res;
            }

            return false;
        }

        public bool CreateFolder(string folder)
        {
            if (_settings.Validate())
            {
                var client = new Client(_settings.Server, _settings.User, _settings.Password);

                return client.CreateDirectory($"{_settings.RemoteFolder}/{folder}");
            }
            return false;
        }

        public string GetShareFolderLink(string remoteFolder)
        {
            if (_settings.Validate())
            {
                var client = new Client(_settings.Server, _settings.User, _settings.Password);
                var remoteFile = $"{_settings.RemoteFolder}/{remoteFolder}";
                var link = client.ShareWithLink(remoteFile, Convert.ToInt32(OcsPermission.All));

                return link.Url;
            }

            return string.Empty;

        }
    }
}
