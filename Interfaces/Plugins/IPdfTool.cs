using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public interface IPdfTool
    {
        void Execute(PdfJobContext context);
        bool Configure(PdfJobContext context);
    }
}
