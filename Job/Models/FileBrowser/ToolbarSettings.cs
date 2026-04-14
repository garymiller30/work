using Interfaces.FileBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.FileBrowser
{
    public class ToolbarSettings : IToolbarSettings
    {
        public List<string> Tools { get; set; } = new List<string>();
    }
}
