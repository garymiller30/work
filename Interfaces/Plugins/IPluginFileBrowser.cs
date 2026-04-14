using Interfaces.Profile;

namespace Interfaces.FileBrowser
{
    public interface IPluginFileBrowser : IPluginBase
    {
        IUserProfile UserProfile { get; set; }

        void FileBrowserFormatRow(IFileBrowser browser, object row);
        void FileBrowserSelectObject(IFileBrowser fileBrowser, IFileSystemInfoExt file);
    }
}