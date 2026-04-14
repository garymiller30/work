using Interfaces;
using Interfaces.Profile;

namespace JobSpace.Profiles.ProfileEvents
{
    public abstract class AbstractEvents
    {
        public abstract void Init(IUserProfile profile);
    }
}
