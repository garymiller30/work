using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Ftp
{
    public interface IDownloadResult
    {
        List<IFtpFileExt> Files {get;}
        string TargetDir {get;}
    }
}
