using Interfaces.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.FileBrowser
{
    public sealed class ToolInfo
    {
        public Type ToolType;
        public PdfToolAttribute Meta;
        public IPdfTool Create()
        {
            return (IPdfTool)Activator.CreateInstance(ToolType);
        }
    }
}
