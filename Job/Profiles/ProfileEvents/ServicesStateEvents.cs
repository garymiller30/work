using Interfaces;
using System;

namespace JobSpace.Profiles.ProfileEvents
{
    public sealed class ServicesStateEvents : IServiceStateEvents
    {
        public EventHandler<IServiceState> AddServiceState { get; set; } = delegate { };
        public EventHandler<IServiceState> UpdateServiceState { get; set; } = delegate { };

        public void RaiseAddServiceState(object sender, IServiceState state)
        {
            AddServiceState(sender, state);
        }

        public void RaiseUpdateServiceState(object sender, IServiceState state)
        {
            UpdateServiceState(sender, state);
        }
    }
}
