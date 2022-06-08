using System.Collections.Generic;
using Interfaces;

namespace FtpClient
{
    public class TreeNode
    {
        public IFtpFileExt Folder { get; set; }
        public List<TreeNode> ChildFolders { get; set; }
        public string Name => Folder?.Name;
    }
}
