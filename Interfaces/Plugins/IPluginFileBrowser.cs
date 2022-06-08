namespace Interfaces
{
    public interface IPluginFileBrowser : IPluginBase
    {
        IUserProfile UserProfile { get; set; }

        void FileBrowserFormatRow(IFileBrowser browser, object row);
        void FileBrowserSelectObject(IFileSystemInfoExt file);
    }
}