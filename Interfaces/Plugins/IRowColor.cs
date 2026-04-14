using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public interface IRowColor
    {
        Color Fore { get;set; }
        Color Back { get;set; }
    }
}
