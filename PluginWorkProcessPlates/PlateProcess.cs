using System;
using System.Windows.Forms;
using Interfaces;
using Interfaces.Plugins;
using PluginWorkProcessPlates.Forms;

namespace PluginWorkProcessPlates
{
    public sealed class PlateProcess : AbstractProcess<Pay>

    {
        public int CountPlates { get; set; }
        public decimal PriceForPlate { get; set; }
        public PlateFormat PlateFormat { get; set; } = new PlateFormat();


        public override string Name
        {
            get => $"{PlateFormat.Width} x {PlateFormat.Height}, {CountPlates} шт.";
            set { }
        }

        public override decimal Price
        {
            get => CountPlates * PriceForPlate;
            set { }
        }

        protected override void CreateContextMenu(IPluginFormAddWork parent)
        {

            if (_contextMenu.Items.Count == 0)
            {
                // create context menu
            
                _contextMenu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripMenuItem("редагувати",null,OnEdit),
                    new ToolStripMenuItem("видалити",null,OnDelete),
                });

            }

            foreach (ToolStripMenuItem item in _contextMenu.Items)
            {
                item.Tag = parent;
            }
        }

        private void OnDelete(object sender, EventArgs e)
        {
            ((IPluginFormAddWork)((dynamic)sender).Tag).RemoveProcess(this);
        }

        private void OnEdit(object sender, EventArgs e)
        {
            EditProcess();
            ((IPluginFormAddWork)((dynamic)sender).Tag).Update(this);
        }

        public override bool EditProcess()
        {
            using (var form = new FormEdit(this))
            {
                return form.ShowDialog() == DialogResult.OK;
            }
        }

        public override bool EditProcess(IUserProfile profile)
        {
            using (var form = new FormEdit(this,profile))
            {
                return form.ShowDialog() == DialogResult.OK;
            }
        }
    }
}
