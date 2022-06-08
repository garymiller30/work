// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interfaces;

namespace Job.Menus
{
    public class FileBrowserContextMenuUtils : FileBrowserContextMenuSendTo
    {
        public FileBrowserContextMenuUtils(IUserProfile profile, string fileName) : base(profile, fileName)
        {

        }

        public override IMenuSendTo Add(string name, string path, string commandLine)
        {
            var m = new MenuSendTo { Name = name, Path = path, CommandLine = commandLine};
            Add(m);
            return m;
        }

        public override List<ToolStripMenuItem> Get(EventHandler ttmClick)
        {
            var l = new List<ToolStripMenuItem>();

            var menus = Get();
            foreach (var item in menus)
            {
                var ttm = new ToolStripMenuItem
                {
                    Text = item.Name,
                    Tag = item,
                    Image = item.GetImage()
                };
                ttm.Click += ttmClick;
                l.Add(ttm);
            }
            return l;
        }

        public override List<ToolStripItem> GetToolStripButtons(int explorerIdx, EventHandler ttmClick, bool all = false)
        {
            var l = new List<ToolStripItem>();

            var menus = Get().Cast<MenuSendTo>().ToList();

            IEnumerable<IGrouping<string,MenuSendTo>> group;

            if (all)
            {
                group = menus.GroupBy(x => x.Path,x=>x);
            }
            else
            {

                group = menus
                    .Where(xx=> xx.Enable && 
                                xx.UsedInExplorer[explorerIdx])
                    .GroupBy(x => x.Path,x=>x);
            }
                

            foreach (var g in group)
            {
                if (g.Count() == 1)
                {
                    foreach (MenuSendTo menuSendTo in g)
                    {
                        if (menuSendTo.IsScript())
                        {
                            var button = new ToolStripSplitButton()
                            {
                                DisplayStyle = ToolStripItemDisplayStyle.Text,
                                Text = menuSendTo.Name,
                                Tag = menuSendTo
                            };

                            button.Click += ttmClick;
                            button.ToolTipText = $"{menuSendTo.Name} ({menuSendTo.Path})";

                            var tsmiEdit = new ToolStripMenuItem("редагувати");
                            tsmiEdit.Click+= TsmiEditOnClick;
                            tsmiEdit.Tag = menuSendTo;
                            button.DropDownItems.Add(tsmiEdit);

                            l.Add(button);

                            //var rb = new RoundedButton();
                            //var roundButton = new ToolStripControlHost(rb)
                            //{
                            //    DisplayStyle = ToolStripItemDisplayStyle.Text,
                            //    Text = menuSendTo.Name,
                            //    Tag = menuSendTo
                            //};
                            //roundButton.Click += ttmClick;
                            //roundButton.ToolTipText = $"{menuSendTo.Name} ({menuSendTo.Path})";
                            //l.Add(roundButton);

                        }
                        else
                        {
                            var ttm = new ToolStripButton
                            {
                                Text = menuSendTo.Name,
                                Tag = menuSendTo,
                                Image = menuSendTo.Image
                            };
                            ttm.DisplayStyle = ToolStripItemDisplayStyle.Image;
                            ttm.ToolTipText = menuSendTo.Name;
                            ttm.Click += ttmClick;
                            l.Add(ttm);
                        }

                    }

                }
                else
                {
                    var button = new ToolStripSplitButton {Image = g.First().Image};

                    button.Tag = g.First();
                    foreach (var menuSendTo in g)
                    {
                        var ttm = new ToolStripMenuItem()
                        {
                            
                            Text = menuSendTo.Name,
                            ToolTipText = menuSendTo.Name,
                            Tag = menuSendTo,
                        };
                        ttm.Click += ttmClick;
                        
                        button.DropDownItems.Add(ttm);
                    }

                    l.Add(button);
                    l.Add(new ToolStripSeparator());
                }
            }
            return l;
        }

        private void TsmiEditOnClick(object sender, EventArgs e)
        {
            var menu = (MenuSendTo)((ToolStripMenuItem) sender).Tag;
           ExtensionMethods.Files.ShowOpenWithDialog(menu.Path);
        }

      
    }
}
