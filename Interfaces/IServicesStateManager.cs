using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IServicesStateManager
    {
        IServiceState Create();
        void Add(IServiceState state);
        IEnumerable<IServiceState> GetAll();
        Image GetImage(IServiceState state);
    }
}
