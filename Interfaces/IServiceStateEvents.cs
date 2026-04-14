using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IServiceStateEvents
    {
        EventHandler<IServiceState> AddServiceState { get; set; }
        EventHandler<IServiceState> UpdateServiceState { get; set; }
    }
}
