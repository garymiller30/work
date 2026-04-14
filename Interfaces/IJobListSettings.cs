using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IJobListSettings
    {
        Font UserFont { get; set; }
        string FontName { get; set; }
        float FontSize { get; set; }
        FontStyle FontStyle { get; set; }
    }
}
