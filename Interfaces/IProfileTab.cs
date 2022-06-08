namespace Interfaces
{
    public interface IProfileTab
    {
        object Tag { get; set; }
        void ResetLayout();
        void CloseProgram();
        IUserProfile GetUserProfile();
        bool IsInitializedControl { get; set; }
        void Init();
        void SaveLayout();
    }
}
