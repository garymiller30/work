using Interfaces;
using Interfaces.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient
{
    public sealed class DownloadResult : IDownloadResult
    {
        public List<IFtpFileExt> Files {get;}

        public string TargetDir {get;}

        public DownloadResult(List<IFtpFileExt> files,string targetDir)
        {
            Files = files;
            TargetDir = targetDir;
        }

    }
}
