using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public interface IPluginJobList : IPluginBase
    {
        IUserProfile UserProfile { get; set; }
        void SetRow(object row);
    }
}
