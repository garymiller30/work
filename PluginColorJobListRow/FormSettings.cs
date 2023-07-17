using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;

namespace PluginColorJobListRow
{
    public sealed partial class FormSettings : Form
    {
        private ColorJobListRowSettings _settings;
        private IUserProfile _userProfile;
        private IJobStatus[] _statuses;
        
        public FormSettings()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormSettings(ColorJobListRowSettings settings, IUserProfile userProfile):this()
        {
            _settings = settings;
            _userProfile = userProfile;
            _statuses = _userProfile.StatusManager.GetJobStatuses();

            InitColumns();

            objectListView1.AddObjects(_statuses);
        }

        private void InitColumns()
        {
            olvColumnStatus.AspectGetter += rowObject => ((IJobStatus) rowObject).Name;

            olvColumnColor.AspectGetter += row => _settings.GetColor(((IJobStatus) row).Code).Back;
            olvColumnForeColor.AspectGetter += row => _settings.GetColor(((IJobStatus)row).Code).Fore;

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void objectListView1_FormatCell(object sender, BrightIdeasSoftware.FormatCellEventArgs e)
        {
            if (e.Column == olvColumnColor)
            {
                var status = (IJobStatus) e.Item.RowObject;
                var color = _settings.GetColor(status.Code);
                e.SubItem.BackColor = color.Back;
               
            }
            else if (e.Column == olvColumnForeColor)
            {
                var status = (IJobStatus)e.Item.RowObject;
                var color = _settings.GetColor(status.Code);
                e.SubItem.BackColor = color.Fore;
                
            }
        }

        private void objectListView1_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            var status = (IJobStatus)e.RowObject;

            if (e.Column == olvColumnColor)
            {

                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    _settings.SetBack(status.Code,colorDialog1.Color);
                }

                e.Cancel = true;
            }
            else if (e.Column == olvColumnForeColor)
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    _settings.SetFore(status.Code, colorDialog1.Color);
                }

                e.Cancel = true;
            }
        }

        private void заЗамовчуваннямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                foreach (var item in objectListView1.SelectedObjects)
                {
                    _settings.SetColor(((IJobStatus)item).Code,new RowColor());
                }
            }
        }
    }
}
