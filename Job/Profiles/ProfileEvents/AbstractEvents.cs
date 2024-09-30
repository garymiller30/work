using Interfaces;

namespace JobSpace.Profiles.ProfileEvents
{
    public abstract class AbstractEvents
    {
        public abstract void Init(IUserProfile profile);
    }
}
