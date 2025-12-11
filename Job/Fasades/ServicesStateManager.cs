using Interfaces;
using JobSpace.Models;
using JobSpace.Profiles;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Image GetImage(IServiceState state)
        {
            if (state.Image != null) return state.Image;

            switch (state.State)
            {
                case Interfaces.Enums.ServiceStateEnum.ACTIVE:
                    return Properties.Resources.Hopstarter_Soft_Scraps_Button_Blank_Green_16;
                case Interfaces.Enums.ServiceStateEnum.INACTIVE:
                    return Properties.Resources.Hopstarter_Soft_Scraps_Button_Blank_Red_16;
                default:
                    return Properties.Resources.Hopstarter_Soft_Scraps_Button_Blank_Gray_16;
            }
        }
    }
}
