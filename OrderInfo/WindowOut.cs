using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interfaces;
using Job.Profiles;

namespace OrderInfo
{
    public sealed partial class WindowOut : UserControl, IPluginInfo
    {
        private IJob _curjob;
        private bool _isSubscribed;
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
                if (ucNote1.GetText().Length > 0)
                {
                    SaveChanges();
                }
        }

        void SaveChanges()
        {
            if (_curjob != null)
            {
                _curjob.Note = ucNote1.GetRtf();
                UserProfile.Jobs.UpdateJob(_curjob);
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
