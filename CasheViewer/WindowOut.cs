// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using System;
using System.Linq;
using System.Windows.Forms;
using CasheViewer.Reports;
using CasheViewer.UC;
using Interfaces;
using JobSpace.Dlg;
using JobSpace.Profiles;
using JobSpace.UserForms;

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
            if (job is JobSpace.Job j)
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
                if (!((JobSpace.Job)job).IsCashePayed)
                {
                    var cntForms = ((JobSpace.Job)job).Parts?.Sum(x => x.Form.Count) ?? 0;

                    ((JobSpace.Job)job).CachePayedSum += (cntForms - _savedCntForms) * _settings.PriceForPlate;

                    if (((JobSpace.Job)job).CachePayedSum != 0)
                    {
                        ((JobSpace.Job)job).IsCashe = true;
                    }
                    else
                    {
                        ((JobSpace.Job)job).IsCashe = false;
                    }
                }

            }
        }



        private void RefreshReport()
        {
            _curReportControl.ShowReport( _report);
            ShowTotal();
        }

        private void ShowTotal()
        {
            toolStripStatusLabel_TotalDecimal.Text = _report.Total.ToString("N0");
            tssl_PriceWithCPI.Text = $"{_report.TotalWithConsumerPrice.ToString("N0")} ({(_report.TotalWithConsumerPrice - _report.Total).ToString("N0")})"  ;
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
            _curReportControl.PaySelected();
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

        private void tsb_pay_custom_Click(object sender, EventArgs e)
        {
            using (var form = new FormTirag())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _curReportControl.PayCustomSum(form.Tirag);
                    RefreshReport();
                }
            }
        }

        private void tsb_load_consumer_price_indices_Click(object sender, EventArgs e)
        {
            _curReportControl.ApplyConsumerPriceIndices();
            ShowTotal();
        }
    }
}
