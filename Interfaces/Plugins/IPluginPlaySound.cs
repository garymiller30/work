using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public interface IPluginPlaySound : IPluginBase
    {
        IUserProfile UserProfile { get; set; }
        void PlaySound(AvailableSound soundType,object param);
    }
}
