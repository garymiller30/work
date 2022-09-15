using System.Collections.Generic;

namespace FtpClient
{
    public sealed class DownloadFilesSettings
    {
        public bool DownloadLikeDescription { get; set; }
        public bool SeparateDownload { get; set; }
        public IEnumerable<FtpFileExt> Files { get; set; } = new List<FtpFileExt>();
        public string OrderNumber { get; set; }
        public dynamic OnDownloaded {get;set;}
    }
}