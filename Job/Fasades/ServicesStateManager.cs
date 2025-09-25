using Interfaces;
using JobSpace.Models;
using JobSpace.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Fasades
{
    public sealed class ServicesStateManager : IServicesStateManager
    {
        Profile _profile;
        List<IServiceState> services {get;set; } = new List<IServiceState>();

        public IServiceState Create()
        {
            var state = new ServiceState();
            services.Add(state);
            return state;
        }

        public ServicesStateManager(Profile profile)
        {
            _profile = profile;
        }

        public void Add(IServiceState state)
        {
            services.Add(state);
        }

        public IEnumerable<IServiceState> GetAll()
        {
            return services;
        }
    }
}
