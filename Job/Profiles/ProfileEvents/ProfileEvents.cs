using Interfaces;

namespace JobSpace.Profiles.ProfileEvents
{
    public sealed class ProfileEvents : IProfileEvents
    {
        public IJobEvents Jobs { get; set; } = new JobEvents();
        public IBrowserEvents Browsers { get; set; } = new BrowserEvents();
        public IFtpEvents Ftp { get; set; } = new FtpEvents();
    }
}
