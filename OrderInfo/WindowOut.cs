using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interfaces;
using JobSpace.Profiles;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderInfo.Models;

namespace OrderInfo
{
    public sealed partial class WindowOut : UserControl, IPluginInfo
    {
        private IJob _curjob;
        private bool _isSubscribed;
        JobInfo _jobInfo;
        // private Profile _profile;

        public IUserProfile UserProfile { get; set; }

        public WindowOut()
        {
            InitializeComponent();
        }

        /*
                public void SetUserProfile(object profile)
                {
                    _profile = profile as Profile;
                }
        */

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
            return "Job Info";
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
            try
            {
                if (curJob is IJob job)
                {
                    _curjob = job;

                    var col = UserProfile.Base.GetRawCollection<JobInfo>();
                    _jobInfo = ((IMongoCollection<JobInfo>)col).AsQueryable().FirstOrDefault(x => x.JobId == (ObjectId)job.Id);

                    if (_jobInfo != null)
                    {
                        cb_cut.Checked = _jobInfo.Cut;
                        cb_uv_lak.Checked = _jobInfo.UVLak;
                        cb_protected_lak.Checked = _jobInfo.ProtectedLak;
                    }
                    else
                    {
                        cb_cut.Checked = false;
                        cb_uv_lak.Checked = false;
                        cb_protected_lak.Checked = false;
                    }

                    ucNote1.SetText(job.Note ?? string.Empty);


                    if (_isSubscribed) ucAddWorkPluginsContainer1.Unsubscribe(UserProfile);

                    ucAddWorkPluginsContainer1.Subscribe(UserProfile, _curjob);
                    _isSubscribed = true;

                    if (UserProfile.Plugins.GetPluginFormAddWorks().Count() == 0)
                    {
                        splitContainer1.Panel2Collapsed = true;
                    }

                }
                else
                {
                    _curjob = null;
                    _jobInfo = null;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }



        }

        public void BeforeJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }

        public void AfterJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }



        private void RichTextBox1_Leave(object sender, EventArgs e)
        {

            SaveChanges();

        }

        void SaveChanges()
        {
            if (_curjob != null)
            {
                _curjob.Note = ucNote1.GetRtf();
                UserProfile.Jobs.UpdateJob(_curjob);

                if (_jobInfo == null)
                {
                    _jobInfo = new JobInfo
                    {
                        JobId = (ObjectId)_curjob.Id,
                        Cut = cb_cut.Checked,
                        UVLak = cb_uv_lak.Checked,
                        ProtectedLak = cb_protected_lak.Checked
                    };
                    var col = UserProfile.Base.GetRawCollection<JobInfo>();
                    ((IMongoCollection<JobInfo>)col).InsertOne(_jobInfo);
                }
                else
                {
                    _jobInfo.Cut = cb_cut.Checked;
                    _jobInfo.UVLak = cb_uv_lak.Checked;
                    _jobInfo.ProtectedLak = cb_protected_lak.Checked;
                    var col = UserProfile.Base.GetRawCollection<JobInfo>();
                    ((IMongoCollection<JobInfo>)col).ReplaceOne(x => x.JobId == _jobInfo.JobId, _jobInfo);
                }

            }
        }

        public string PluginName => GetPluginName();
        public string PluginDescription => "інформація про замовлення";

        public void ShowSettingsDlg()
        {
            MessageBox.Show("Налаштування відсутні");
        }
    }
}
