using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Interfaces
{
    public interface IFileBrowserContextMenu
    {
        List<ToolStripItem> GetToolStripButtons(int explorerIdx, EventHandler ttmClick, bool all = false);
        List<IMenuSendTo> Get();
        List<ToolStripMenuItem> Get(EventHandler ttmClick);
        IMenuSendTo Add(string name, string path);
        IMenuSendTo Add(string name, string path, string commandLine);
        void Remove(object menuItem);
        void Save();
    }
}
