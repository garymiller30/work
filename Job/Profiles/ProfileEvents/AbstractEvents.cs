using Interfaces;

namespace Job.Profiles.ProfileEvents
{
    public abstract class AbstractEvents
    {
        public abstract void Init(IUserProfile profile);
    }
}
