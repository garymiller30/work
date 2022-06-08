using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginAddWorkFromPolymix
{
    public interface IFilter
    {
        string TypeName { get; set; }
        string Name { get; set; }

        Image Img { get;set; }

    }
}
