using Interfaces.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public interface IMqPlugin
    {
        IUserProfile UserProfile { get;set;}
        void Init(IMqController controller);
        void PublishChanges(MessageEnum me, object id);
        void Disconnect();
    }
}
