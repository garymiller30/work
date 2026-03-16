using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.FileBrowser
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PdfToolAttribute : Attribute
    {
        public string MenuPath { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SeparatorBefore { get; set; }
        public bool SeparatorAfter { get; set; }
        public string Icon { get; set; }
        public bool IsBackgroundTask { get; set; }
        public int Order { get; set; } = 9999;
        public PdfToolAttribute(string menuPath, string name)
        {
            MenuPath = menuPath;
            Name = name;
        }
    }
}
