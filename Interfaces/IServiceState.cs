using Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IServiceState
    {
        string Id { get; set; }
        string Name { get; set; }
        string Tooltip { get; set; }
        string Description { get; set; }
        ServiceStateEnum State { get; set; }
        Image Image { get; set; }
    }
}
