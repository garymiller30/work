using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;
using Job.Static;

namespace Job.UC
{
    public sealed partial class UcAddWorkPluginsContainer : UserControl
    {
        public UcAddWorkPluginsContainer()
        {
            InitializeComponent();
            InitTreeListView();
            ThemeController.ThemeChanged += ThemeController_ThemeChanged;
        }

        private void ThemeController_ThemeChanged(object sender, EventArgs e)
        {
            treeListView1.ForeColor = ThemeController.Fore;
            treeListView1.BackColor = ThemeController.Back;
        }

        private void InitTreeListView()
        {
            treeListView1.CanExpandGetter += CanExpandGetter;
            treeListView1.ChildrenGetter += ChildrenGetter;
        }
        
        private IEnumerable ChildrenGetter(object model)
        {
            if (model is IPluginFormAddWork plugin)
            {
                return plugin.GetProcesses();
            }
            return new ArrayList();
        }

        private bool CanExpandGetter(object model)
        {
            if (model is IPluginFormAddWork plugin)
            {
                if (plugin.GetProcesses().Any()) return true;
            }

            return false;
        }

        private void treeListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (treeListView1.SelectedObject is IContextMenu menu)
                {
                    var contextMenu = menu.GetContextMenu();
                    contextMenu.Show(Cursor.Position);

                }
            }
        }

        public void Subscribe(IUserProfile profile, IJob job)
        {
            var plugins = profile.Plugins.GetPluginFormAddWorks();

            if (plugins.Any())
            {
                foreach (var plugin in plugins)
                {
                    plugin.SetJob(profile, job);
                    plugin.PropertyChanged += PluginOnPropertyChanged;
                }

                treeListView1.Roots = plugins;
                treeListView1.ExpandAll();
            }
        }

        private void PluginOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            treeListView1.RefreshObject(sender);
        }

        public void Unsubscribe(IUserProfile profile)
        {
            var plugins = profile.Plugins.GetPluginFormAddWorks();

            foreach (var plugin in plugins)
            {
                plugin.PropertyChanged -= PluginOnPropertyChanged;
            }
        }
    }
}
