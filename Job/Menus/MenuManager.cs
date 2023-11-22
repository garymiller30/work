using Interfaces;

namespace Job.Menus
{
    public sealed class MenuManager : IMenuManager
    {
        public IFileBrowserContextMenu SendTo { get; set; }
        public IFileBrowserContextMenu Utils { get; set; }
        
        public MenuManager(IUserProfile userProfile)
        {
            InitializeMenus(userProfile);
        }

        void InitializeMenus(IUserProfile userProfile)
        {
            SendTo = new FileBrowserContextMenuSendTo(userProfile, "SendTo.settings");
            Utils = new FileBrowserContextMenuUtils(userProfile, "Utils.settings");
        }
    }
}
