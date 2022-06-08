using ComponentFactory.Krypton.Docking;
using ComponentFactory.Krypton.Toolkit;
using Interfaces;
using System.Windows.Forms;

namespace ActiveWorks.UserControls
{
    public partial class FormProfile : KryptonForm, IFormProfile
    {
        private IProfileTab _profileTab;

        public FormProfile()
        {
            InitializeComponent();
        }

        public void InitProfile()
        {
            _profileTab = new UcTabProfile3(kryptonDockingManager) { Tag = Tag, Dock = DockStyle.Fill };
            kryptonPanel.Controls.Add((Control)_profileTab);

            // Setup docking functionality
            KryptonDockingWorkspace w = kryptonDockingManager.ManageWorkspace((KryptonDockableWorkspace)_profileTab);
            kryptonDockingManager.ManageControl(kryptonPanel, w);
            kryptonDockingManager.ManageFloating(this);

            _profileTab.Init();

        }

        public void ResetLayout()
        {
            _profileTab.ResetLayout();
        }

        private void FormProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
            else
            {
                _profileTab.CloseProgram();
            }

        }

        public void SaveLayout()
        {
            _profileTab.SaveLayout();
        }
    }
}
