using System;
using System.Windows.Forms;
using Interfaces;
using JobSpace.Statuses;

namespace CasheViewer
{
    public partial class FormSettings : Form
    {
        public IUserProfile UserProfile { get; set; }

        private CasheSettings _settings;
        public decimal PriceForPlate
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public FormSettings(IUserProfile profile,CasheSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            UserProfile = profile;
            DialogResult = DialogResult.Cancel;
            
            numericUpDown1.Value = _settings.PriceForPlate;

            //var statuses = UserProfile.StatusManager.GetJobStatuses();

            comboBoxStatuses.DataSource = UserProfile.StatusManager.GetJobStatuses();
            comboBoxStatuses.DisplayMember = "Name";

            JobStatus selStatus = (JobStatus)UserProfile.StatusManager.GetJobStatusByCode(_settings.PayStatusCode);
            if (selStatus != null)
            {
                comboBoxStatuses.SelectedItem = selStatus;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _settings.PriceForPlate = numericUpDown1.Value;

            if (comboBoxStatuses.SelectedItem is JobStatus status)
            {
                _settings.PayStatusCode = status.Code;
            }
            Close();
        }
    }
}
