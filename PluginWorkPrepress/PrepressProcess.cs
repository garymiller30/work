using System;
using System.Windows.Forms;
using Interfaces;
using Interfaces.Plugins;
using PluginWorkPrepress.Forms;

namespace PluginWorkPrepress
{
    public class PrepressProcess : AbstractProcess<PrepressPay>
    {
        public override string Name { get; set; }
        public override decimal Price { get; set; }

        protected override void CreateContextMenu(IPluginFormAddWork parent)
        {
            if (_contextMenu.Items.Count == 0)
            {
                _contextMenu.Items.AddRange(new ToolStripItem[]
                {
                    new ToolStripMenuItem("редагувати",null,OnEdit),
                    new ToolStripMenuItem("видалити",null,OnDelete),
                    new ToolStripMenuItem("сплачено",null,OnPay),

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

        private void OnPay(object sender, EventArgs e)
        {
            PayProcess();
            ((IPluginFormAddWork)((dynamic)sender).Tag).Update(this);
        }

        private void PayProcess()
        {
            using (var form = new FormPay(this))
            {
                form.ShowDialog();
            }
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
            using (var form = new FormEdit(this))
            {
                return form.ShowDialog() == DialogResult.OK;
            }
        }
    }
}
