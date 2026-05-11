using System;

namespace Interfaces
{
    public interface IServiceStateEvents
    {
        EventHandler<IServiceState> AddServiceState { get; set; }
        EventHandler<IServiceState> UpdateServiceState { get; set; }

        void RaiseAddServiceState(object sender, IServiceState state);
        void RaiseUpdateServiceState(object sender, IServiceState state);
    }
}
