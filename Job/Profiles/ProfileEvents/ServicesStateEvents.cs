using Interfaces;
using JobSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Profiles.ProfileEvents
{
    public class ServicesStateEvents : IServiceStateEvents
    {
        public EventHandler<IServiceState> AddServiceState { get; set; } = delegate { };
        public EventHandler<IServiceState> UpdateServiceState { get; set; } = delegate { };
    }
}
