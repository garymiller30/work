using Interfaces;
using Krypton.Docking;
using Krypton.Toolkit;
using System.Windows.Forms;

namespace ActiveWorks.UserControls
{
    public sealed partial class FormProfile : KryptonForm, IFormProfile
    {
        private IProfileTab _profileTab;
        public bool IsInitialized { get; private set; }

        public FormProfile()
        {
            InitializeComponent();
        }

        public void InitProfile()
        {
            if (IsInitialized)
            {
                return;
            }

            _profileTab = new UcTabProfile3(kryptonDockingManager) { Tag = Tag, Dock = DockStyle.Fill };
            
            kryptonPanel.Controls.Add((Control)_profileTab);

            // Setup docking functionality
            KryptonDockingWorkspace w = kryptonDockingManager.ManageWorkspace((KryptonDockableWorkspace)_profileTab);
            kryptonDockingManager.ManageControl(kryptonPanel, w);
            kryptonDockingManager.ManageFloating(this);

            _profileTab.Init();
            IsInitialized = true;

        }

        public void ResetLayout()
        {
            InitProfile();
            _profileTab.ResetLayout();
        }

        private void FormProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
            else if (IsInitialized)
            {
                _profileTab.CloseProgram();
            }

        }

        public void SaveLayout()
        {
            InitProfile();
            _profileTab.SaveLayout();
        }
    }
}
