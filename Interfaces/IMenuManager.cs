namespace Interfaces
{
    public interface IMenuManager
    {
        IFileBrowserContextMenu SendTo { get; set; }
        IFileBrowserContextMenu Utils { get; set; }
        bool IsInitialized { get; }
    }
}
