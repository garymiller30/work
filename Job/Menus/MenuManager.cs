using Interfaces;
using System.Diagnostics;

namespace JobSpace.Menus
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
            Stopwatch _sw = new Stopwatch();
            _sw.Start();
            Logger.Log.Info(this, "Loading SendTo menu: ", "start");

            SendTo = new FileBrowserContextMenuSendTo(userProfile, "SendTo.settings");
            _sw.Stop();
            Logger.Log.Info(this, "Loading SendTo menu: ", _sw.ElapsedMilliseconds);
            _sw.Reset();
            _sw.Start();
            Logger.Log.Info(this, "Loading Utils menu: ", "start");
            Utils = new FileBrowserContextMenuUtils(userProfile, "Utils.settings");
            _sw.Stop();
            Logger.Log.Info(this, "Loading Utils menu: ", _sw.ElapsedMilliseconds);
        }
    }
}
