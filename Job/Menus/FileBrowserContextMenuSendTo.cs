using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interfaces;

namespace JobSpace.Menus
{
	public class FileBrowserContextMenuSendTo : IFileBrowserContextMenu
	{
        private IUserProfile _userProfile { get; set; }
		protected  List<MenuSendTo> _menus;
		protected readonly string _fn;
		public FileBrowserContextMenuSendTo(IUserProfile profile, string fileName)
        {
            _userProfile = profile;
			_fn =  Path.Combine(profile.ProfilePath,fileName);
			Load();
		}

        public virtual void Load()
		{
			_menus = Commons.DeserializeXML<List<MenuSendTo>>(_fn) ?? new List<MenuSendTo>();
		}

		protected void GetImage(MenuSendTo menuSendTo)
		{
			if (File.Exists(menuSendTo.Path))
			{
				var icon = Icon.ExtractAssociatedIcon(menuSendTo.Path);
				menuSendTo.Image = icon?.ToBitmap();

			}
		}


		public virtual IMenuSendTo Add(string name, string path)
		{
			var m = new MenuSendTo {Name = name, Path = path};
			_menus.Add(m);
			return m;
		}

        public virtual IMenuSendTo Add(string name, string path, string commandLine)
        {
            var m = new MenuSendTo { Name = name, Path = path, CommandLine = commandLine};
            Add(m);
            return m;
        }

        public virtual void Add(MenuSendTo menuItem)
		{
			_menus.Add(menuItem);
		}

		public void Remove(object menuItem)
		{
            if (menuItem is MenuSendTo m)
                _menus.Remove(m);
		}

		public  void Clear()
		{
			_menus.Clear();
		}

		public  void Save()
		{
			Commons.SerializeXML(_menus, _fn);
		}

		public virtual List<ToolStripMenuItem> Get(EventHandler ttmClick)
		{
			var l = new List<ToolStripMenuItem>();
			if (_userProfile.Customers.CurrentCustomer != null)
            {
				foreach (var item in _menus)
				{
					if (item.Name.ToUpper(CultureInfo.InvariantCulture).Contains(_userProfile.Customers.CurrentCustomer.Name.ToUpper(CultureInfo.InvariantCulture)) || item.Name.StartsWith("*"))
					{
						var ttm = new ToolStripMenuItem { Text = item.Name, Tag = item };

						ttm.Click += ttmClick;
						l.Add(ttm);
					}
				}

			}
			
			return l;
		}


		public List<IMenuSendTo> Get()
		{
		  
			return _menus.Cast<IMenuSendTo>().ToList();
		}

        public virtual List<ToolStripItem> GetToolStripButtons(int explorerIdx, EventHandler ttmClick, bool all = false)
        {
			//заглушка
            return new List<ToolStripItem>();
        }
    }
}
