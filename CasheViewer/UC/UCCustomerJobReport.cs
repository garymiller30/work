using System;
using System.Linq;
using System.Windows.Forms;
using CasheViewer.Reports;
using Interfaces;
using Job.Profiles;

namespace CasheViewer.UC
{
    public partial class UCCustomerJobReport : UserControl, IReportControl
    {

        IUserProfile _profile;

        public UCCustomerJobReport(IUserProfile profile)
        {
            _profile = profile;

            InitializeComponent();

            treeListView1.CanExpandGetter = x => (x as INode).Children.Any();
            treeListView1.ChildrenGetter = o => (o as INode).Children;

            olvColumn_Customer.AspectGetter = x => x is INode customer ? customer.Name : string.Empty;
            olvColumn_Description.AspectGetter = x => x is INode job ? job.Description : string.Empty;
            olvColumn_OrderNumber.AspectGetter = x => x is INode job ? job.Number : string.Empty;
            olvColumnCategory.AspectGetter = x => x is INode job ? job.Category : string.Empty;

            olvColumn_Price.AspectGetter = x => (x as INode).Sum;
            olvColumnDate.AspectGetter = x => (x as INode).Children.Any() ? string.Empty : $"{((INode)x).Date.Year}.{((INode)x).Date.Month:D2}.{((INode)x).Date.Day:D2}";

        }
        

        public event EventHandler<decimal> OnChangeSelected = delegate{};

        public void ShowReport(IReport report)
        {
            
            treeListView1.ClearObjects();
            treeListView1.AddObjects(report.GetNodes());
        }

        public void PaySelected(IReport report)
        {
            var settings = _profile.Plugins.LoadSettings<CasheSettings>();
            var status = _profile.StatusManager.GetJobStatusByCode(settings.PayStatusCode);
            if (status != null)
            {
                foreach (var o in treeListView1.SelectedObjects)
                {
                    if (o is INode reportJob && !reportJob.Children.Any())
                    {
                        if (reportJob.ReportVersion == ReportVersionEnum.Version1)
                        {
#pragma warning disable CS0612 // 'Job.IsCashePayed' is obsolete
                            ((Job.Job)reportJob.Job).IsCashePayed = true;
#pragma warning restore CS0612 // 'Job.IsCashePayed' is obsolete
                        }
                        else
                        {
                            ApplyPayPlugins(_profile, reportJob);
                        }

                        ((Job.Job)reportJob.Job).StatusCode = status.Code;
                        _profile.Jobs.UpdateJob((Job.Job)reportJob.Job, true);
                    }
                }
            }
        }

        private void ApplyPayPlugins(IUserProfile userProfile,INode reportJob)
        {
            foreach (var pluginFormAddWork in userProfile.Plugins.GetPluginFormAddWorks())
            {
                pluginFormAddWork.SetJob(userProfile, (IJob)reportJob.Job);
                var pays = pluginFormAddWork.Price - pluginFormAddWork.Pay;
                if (pays > 0)
                {
                    foreach (IProcess process in pluginFormAddWork.GetProcesses())
                    {
                        var processPay = process.Price - process.Pay;
                        if (processPay > 0)
                        {
                            pluginFormAddWork.PayProcess(process, processPay);
                        }
                    }
                }
            }
        }

        private void treeListView1_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            if (e.Model is INode reportJob)
            {
                e.Item.ForeColor = reportJob.ForegroundColor;
            }
        }

        private void treeListView1_SelectionChanged(object sender, EventArgs e)
        {
            decimal total = 0;

            foreach (var o in treeListView1.SelectedObjects)
            {
                if (o is JobNode job) total += job.Sum;
            }

            OnChangeSelected(this, total);

            //_profile.FileBrowser.Browsers[0].SetRootFolder

        }
    }
}
