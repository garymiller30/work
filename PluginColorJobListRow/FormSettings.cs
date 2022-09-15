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

            olvColumnColor.AspectGetter += rowObject => _settings.GetColor(((IJobStatus) rowObject).Code);

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
                e.SubItem.BackColor = _settings.GetColor(status.Code);
            }
        }

        private void objectListView1_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.Column == olvColumnColor)
            {
                var status = (IJobStatus)e.RowObject;


                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    _settings.SetColor(status.Code,colorDialog1.Color);
                }

                e.Cancel = true;
            }
        }
    }
}
