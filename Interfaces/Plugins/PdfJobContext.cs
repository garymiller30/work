using Interfaces.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public class PdfJobContext
    {
        public List<IFileSystemInfoExt> InputFiles {get;set;} = new List<IFileSystemInfoExt>();
        public List<string> ProcessingFiles { get; set; } = new List<string>();
        public IFileManager FileManager { get; set; }
        public IUserProfile UserProfile { get; set; }
    }
}
