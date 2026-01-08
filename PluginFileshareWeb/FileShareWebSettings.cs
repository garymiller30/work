using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFileshareWeb
{
    public class FileShareWebSettings
    {
        public List<LinkInfo> Links = new List<LinkInfo>();
        public HashSet<LinkInfo> OpenOnStart = new HashSet<LinkInfo>();
        public void Normalize()
        {
            foreach (var link in Links)
            {
                if (link.ZoomFactor <= 0)
                {
                    link.ZoomFactor = 1.0f;
                }
            }
            foreach (var link in OpenOnStart) {
                if (link.ZoomFactor <= 0)
                {
                    link.ZoomFactor = 1.0f;
                }
            }
        }
    }
}
