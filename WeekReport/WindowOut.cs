using System;
using System.Linq;
using System.Windows.Forms;
using Interfaces;
using JobSpace;
using JobSpace.Profiles;
using JobManager = JobSpace.Fasades.JobManager;

namespace WeekReport
{
    public partial class WindowOut : UserControl, Interfaces.IPluginInfo
    {
        private Profile _profile;

        #region WeekReport
        DateTime dateForWeekReport = DateTime.Now;
        private int totalMaxPlate;

        #endregion

        public WindowOut()
        {
            InitializeComponent();

            InitReportWeekTree();
        }

        public void SetUserProfile(IUserProfile profile)
        {
            _profile = profile as Profile;
        }

        public IUserProfile UserProfile { get; set; }

        public UserControl GetUserControl()
        {
            return this;
        }

        public void Start()
        {
            //throw new NotImplementedException();
        }

        public string GetPluginName()
        {
            return "на цьому тижні";
        }

        public void SetCurJobCallBack(object curJob)
        {
            //throw new NotImplementedException();
        }

        public void SetCurJobPathCallBack(object curJobPath)
        {
            //throw new NotImplementedException();
        }

        public void SetCurJob(IJob curJob)
        {
            //throw new NotImplementedException();
        }

        public void BeforeJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }

        public void AfterJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }

        private void InitReportWeekTree()
        {
            treeListView_Week.CanExpandGetter = delegate (object x)
            {
                if (x is ReportCustomerTree)
                {
                    return true;
                }
                return x is int;
            };

            treeListView_Week.ChildrenGetter = delegate (object x)
            {
                var tree = x as ReportCustomerTree;
                return tree?.PlateFormat;
            };

            // понедельник
            olvColumnMo.AspectGetter = o =>
            {
                if (o is ReportPlateFormatTree tree)
                {
                    return GetValue(tree.Montag);
                }
                if (o is ReportCustomerTree rowObject)
                {
                    return GetValue(rowObject.PlateFormat.Sum(x => x.Montag));
                }
                return null;
            };
            // вторник
            olvColumnDi.AspectGetter = o =>
            {
                if (o is ReportPlateFormatTree tree)
                {
                    return GetValue(tree.Dienstag);
                }
                if (o is ReportCustomerTree rowObject)
                {
                    return GetValue(rowObject.PlateFormat.Sum(x => x.Dienstag));
                }
                return null;
            };
            // среда
            olvColumnMit.AspectGetter = o =>
            {
                if (o is ReportPlateFormatTree tree) { return GetValue(tree.Mittwoch); }
                if (o is ReportCustomerTree customerTree) { return GetValue(customerTree.PlateFormat.Sum(x => x.Mittwoch)); }
                return null;
            };
            // четверг
            olvColumnDon.AspectGetter = o =>
            {
                if (o is ReportPlateFormatTree tree) { return GetValue(tree.Donnerstag); }
                if (o is ReportCustomerTree customerTree) { return GetValue(customerTree.PlateFormat.Sum(x => x.Donnerstag)); }
                return null;
            };
            // пятница
            olvColumnFr.AspectGetter = o =>
            {
                if (o is ReportPlateFormatTree tree) { return GetValue(tree.Freitag); }
                if (o is ReportCustomerTree customerTree) { return GetValue(customerTree.PlateFormat.Sum(x => x.Freitag)); }
                return null;
            };
            // суббота
            olvColumnSam.AspectGetter = o =>
            {
                if (o is ReportPlateFormatTree tree) { return GetValue(tree.Samstag); }
                if (o is ReportCustomerTree customerTree) { return GetValue(customerTree.PlateFormat.Sum(x => x.Samstag)); }
                return null;
            };
            // воскресенье
            olvColumnSonn.AspectGetter = o =>
            {
                if (o is ReportPlateFormatTree tree) { return GetValue(tree.Sonntag); }
                if (o is ReportCustomerTree customerTree) { return GetValue(customerTree.PlateFormat.Sum(x => x.Sonntag)); }
                return null;
            };

            olvColumnCompare.AspectGetter += o =>
            {
                if (o is ReportCustomerTree tree && totalMaxPlate != 0)
                {

                    var cur = 100 * tree.PlateFormat.Sum(x => x.TotalSum()) / totalMaxPlate;
                    return cur;
                }
                return 0;
            };

        }

        private void ToolStripButtonPreviousWeek_Click(object sender, EventArgs e)
        {
            treeListView_Week.ClearObjects();
            dateForWeekReport = dateForWeekReport.AddDays(-7);

            SetWeekBar();
        }

        private void ToolStripButton_CurrentWeek_Click(object sender, EventArgs e)
        {
            treeListView_Week.ClearObjects();

            dateForWeekReport = DateTime.Now;
            SetWeekBar();

        }

        private void ToolStripButton_NextWeek_Click(object sender, EventArgs e)
        {
            treeListView_Week.ClearObjects();
            dateForWeekReport = dateForWeekReport.AddDays(7);

            SetWeekBar();

        }

        void SetWeekBar()
        {
            //var root = new  ReportGenerator().GetReportCustomerTreeByCurrentWeek(dateForWeekReport);
            //totalMaxPlate = root.Max(x => x.TotalPlateCount());
            //treeListView_Week.Roots = root;



            //toolStripLabelCurWeek.Text = $"{JobManager.GetWeekDate(dateForWeekReport, DayOfWeek.Monday).ToShortDateString()} - {JobManager.GetWeekDate(dateForWeekReport, DayOfWeek.Sunday).ToShortDateString()}";
        }

        private void LoadWeekJobsReport()
        {
            treeListView_Week.ClearObjects();

            SetWeekBar();
        }

        private void РазвернутьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeListView_Week.ExpandAll();
        }

        private void СвернутьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeListView_Week.CollapseAll();
        }

        static string GetValue(int s)
        {
            return s == 0 ? "-" : s.ToString();
        }

        public string PluginName => GetPluginName();
        public string PluginDescription => "report";
        public void ShowSettingsDlg()
        {
            throw new NotImplementedException();
        }
    }
}
