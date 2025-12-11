namespace Interfaces
{
    public interface IProfileEvents
    {
        IJobEvents Jobs { get; set; }
        IFtpEvents Ftp { get; set; }
        IBrowserEvents Browsers { get; set; }
        IServiceStateEvents ServiceStateEvents { get; set; }
    }
}
