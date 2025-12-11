using Interfaces;
using System.Diagnostics;

namespace JobSpace.Menus
{
    public sealed class MenuManager : IMenuManager
    {
        public IFileBrowserContextMenu SendTo { get; set; }
        public IFileBrowserContextMenu Utils { get; set; }

        public bool IsInitialized {get;set; }

        public MenuManager(IUserProfile userProfile)
        {
            InitializeMenus(userProfile);
            IsInitialized = true;
            userProfile.Events.Jobs.OnToolsMenuInitialized(this,null);
        }

        void InitializeMenus(IUserProfile userProfile)
        {
            SendTo = new FileBrowserContextMenuSendTo(userProfile, "SendTo.settings");
            Utils = new FileBrowserContextMenuUtils(userProfile, "Utils.settings");
        }
    }
}
