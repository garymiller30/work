using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins
{
    public class PluginSettings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string FilePath { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsLoaded { get; set; } = false;
        public List<string> Profiles { get; set; } = new List<string>();
    }
}
