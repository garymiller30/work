using CasheViewer.Reports;
using Interfaces;
using JobSpace.Profiles;
using System;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        public event EventHandler<decimal> OnChangeSelected = delegate { };

        public void ShowReport(IReport report)
        {

            treeListView1.ClearObjects();
            treeListView1.AddObjects(report.GetNodes());
        }

        public void PayCustomSum(IReport report, int tirag)
        {
            var settings = _profile.Plugins.LoadSettings<CasheSettings>();
            var status = _profile.StatusManager.GetJobStatusByCode(settings.PayStatusCode);
            if (status != null && treeListView1.Objects != null)
            {
                RecurseReport(treeListView1.Objects.Cast<INode>().ToList(),status, tirag);
            }
        }

        decimal RecurseReport(System.Collections.IEnumerable objects, IJobStatus status, decimal tirag)
        {
            if (tirag == 0) return 0;

            decimal recursedTirag = tirag;

            foreach (var o in objects)
            {
                if (recursedTirag == 0) break;

                if (o is INode reportJob)
                {
                    if (reportJob.Children.Any())
                    {
                        recursedTirag = RecurseReport(reportJob.Children, status, recursedTirag);
                    }
                    else
                    {
                        recursedTirag = ApplyPayPlaginsByTirag(_profile,status, reportJob,recursedTirag);
                    }
                }
            }
            return recursedTirag;
        }

        decimal ApplyPayPlaginsByTirag(IUserProfile userProfile, IJobStatus status, INode reportJob,decimal tirag)
        {
            if (tirag == 0) return 0;
            decimal recursedTirag = tirag;

            foreach (var pluginFormAddWork in userProfile.Plugins.GetPluginFormAddWorks())
            {
                if (recursedTirag == 0) break;

                pluginFormAddWork.SetJob(userProfile, (IJob)reportJob.Job);

                var pays = pluginFormAddWork.Price - pluginFormAddWork.Pay;
                
                if (pays > 0)
                {
                    foreach (IProcess process in pluginFormAddWork.GetProcesses())
                    {
                        if (recursedTirag == 0) break;

                        var processPay = process.Price - process.Pay;
                        if (processPay > 0)
                        {
                            if (recursedTirag - processPay >= 0)
                            {
                                recursedTirag -= processPay;
                                pluginFormAddWork.PayProcess(process, processPay);
                            }
                            else
                            {
                                pluginFormAddWork.PayProcess(process, recursedTirag);
                                recursedTirag = 0;
                            }

                             ((JobSpace.Job)reportJob.Job).StatusCode = status.Code;
                            _profile.Jobs.UpdateJob((JobSpace.Job)reportJob.Job, true);

                        }
                    }
                }
            }

            return recursedTirag;
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
                            ((JobSpace.Job)reportJob.Job).IsCashePayed = true;
#pragma warning restore CS0612 // 'Job.IsCashePayed' is obsolete
                        }
                        else
                        {
                            ApplyPayPlugins(_profile, reportJob);
                        }

                        ((JobSpace.Job)reportJob.Job).StatusCode = status.Code;
                        _profile.Jobs.UpdateJob((JobSpace.Job)reportJob.Job, true);
                    }
                }
            }
        }

        private void ApplyPayPlugins(IUserProfile userProfile, INode reportJob)
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

        public void ApplyConsumerPriceIndices(IReport report)
        {
            // знайти в рапорті найменшу дату
           var dateMin = report.DateMin;
            // отрмати індекс інфляції, починаючи з цієї дати і до сьогодні
            var indices = ConsumerPriceIndices.GetConsumerPrices(dateMin.Year,dateMin.Month);
        }
    }
}
