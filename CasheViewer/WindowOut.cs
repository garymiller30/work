// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using System;
using System.Linq;
using System.Windows.Forms;
using CasheViewer.Reports;
using CasheViewer.UC;
using Interfaces;
using Job.Profiles;

namespace CasheViewer
{
    public partial class WindowOut : UserControl, IPluginInfo
    {
        private int _savedCntForms;
        private object _savedJob;
        public IUserProfile UserProfile { get; set; }
        private IReport _report = new ReportJob();
        private IReportControl _curReportControl;

        private CasheSettings _settings;

        public WindowOut()
        {
            InitializeComponent();
        }

        private void CurReportControlOnOnChangeSelected(object sender, decimal e)
        {
            toolStripStatusLabel_Selected.Text = e.ToString("N0");
        }

        public UserControl GetUserControl()
        {
            return this;
        }

        public void Start()
        {
            _settings = UserProfile.Plugins.LoadSettings<CasheSettings>();
            _report.UserProfile = UserProfile;

            _curReportControl = new UCCustomerJobReport(UserProfile) { Dock = DockStyle.Fill };
            _curReportControl.OnChangeSelected += CurReportControlOnOnChangeSelected;
            panelControlReport.Controls.Add((UserControl)_curReportControl);
            //_pricePlate = Properties.Settings.Default.PriceForPlate;
        }

        public string GetPluginName()
        {
            return "Готівка";
        }

        public void SetCurJob(IJob curJob)
        {
            //throw new NotImplementedException();
        }

        public void BeforeJobChange(IJob job)
        {
            if (job is Job.Job j)
            {
                _savedJob = job;
#pragma warning disable CS0612 // 'Job.Parts' is obsolete
                _savedCntForms = j.Parts?.Sum(x => x.Form.Count) ?? 0;
#pragma warning restore CS0612 // 'Job.Parts' is obsolete
            }
        }

        public void AfterJobChange(IJob job)
        {
            if (_savedJob == job)
            {
#pragma warning disable CS0612 // 'Job.IsCashePayed' is obsolete
                if (!((Job.Job)job).IsCashePayed)
                {
#pragma warning disable CS0612 // 'Job.Parts' is obsolete
                    var cntForms = ((Job.Job)job).Parts?.Sum(x => x.Form.Count) ?? 0;
#pragma warning restore CS0612 // 'Job.Parts' is obsolete

#pragma warning disable CS0612 // 'Job.CachePayedSum' is obsolete
                    ((Job.Job)job).CachePayedSum += (cntForms - _savedCntForms) * _settings.PriceForPlate;
#pragma warning restore CS0612 // 'Job.CachePayedSum' is obsolete

#pragma warning disable CS0612 // 'Job.CachePayedSum' is obsolete
                    if (((Job.Job)job).CachePayedSum != 0)
                    {
#pragma warning disable CS0612 // 'Job.IsCashe' is obsolete
                        ((Job.Job)job).IsCashe = true;
#pragma warning restore CS0612 // 'Job.IsCashe' is obsolete
                    }
                    else
                    {
#pragma warning disable CS0612 // 'Job.IsCashe' is obsolete
                        ((Job.Job)job).IsCashe = false;
#pragma warning restore CS0612 // 'Job.IsCashe' is obsolete
                    }
#pragma warning restore CS0612 // 'Job.CachePayedSum' is obsolete
                }
#pragma warning restore CS0612 // 'Job.IsCashePayed' is obsolete

            }
        }



        private void RefreshReport()
        {
            _curReportControl.ShowReport( _report);
            toolStripStatusLabel_TotalDecimal.Text = _report.Total.ToString("N0");
        }

        private void ShowTotal()
        {
            toolStripStatusLabel_TotalDecimal.Text = _report.Total.ToString("N0");
        }

        private void ShowSettings()
        {
            using (var d = new FormSettings(UserProfile,_settings))
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    UserProfile.Plugins.SaveSettings(_settings);
                }
            }
        }

        public string PluginName => GetPluginName();
        public string PluginDescription => "контроль розрахунків";

        public void ShowSettingsDlg()
        {
            ShowSettings();
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void toolStripButtonPayed_Click(object sender, EventArgs e)
        {
            _curReportControl.PaySelected(_report);
            RefreshReport();

        }

        private void toolStripButtonJobs_Click(object sender, EventArgs e)
        {
            _report = new ReportJob {UserProfile = UserProfile};
            _curReportControl.ShowReport(_report);
            ShowTotal();
        }

        private void toolStripButtonReportYears_Click(object sender, EventArgs e)
        {
            _report = new ReportCustomerYear{UserProfile = UserProfile};
            _curReportControl.ShowReport(_report);
            ShowTotal();
        }

        private void toolStripButtonTotalPayedByCustomer_Click(object sender, EventArgs e)
        {
            _report = new ReportCustomerPayByYear() { UserProfile = UserProfile };
            _curReportControl.ShowReport( _report);
            ShowTotal();
        }
    }
}
