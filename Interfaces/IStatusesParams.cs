namespace Interfaces
{
    public interface IStatusesParams
    {
        IUserProfile UserProfile { get; set; }
        IStatusChangeParam GetParam(int statusCode);
        void Run(IJob job);
        void Load();
        void Save();
    }
}
