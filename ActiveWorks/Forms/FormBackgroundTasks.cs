using BackgroundTaskServiceLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActiveWorks.Forms
{
    public sealed partial class FormBackgroundTasks : Form
    {
        

        public FormBackgroundTasks()
        {
            InitializeComponent();
        }

        private void FormBackgroundTasks_Shown(object sender, EventArgs e)
        {
            objectListView1.AddObjects(BackgroundTaskService.GetAll());

            BackgroundTaskService.OnAdd += BackgroundTaskService_OnAdd;
            BackgroundTaskService.OnFinish += BackgroundTaskService_OnFinish;
        }

        private void BackgroundTaskService_OnFinish(object sender, BackgroundTaskItem e)
        {
            objectListView1.RemoveObject(e);
        }

        private void BackgroundTaskService_OnAdd(object sender, BackgroundTaskItem e)
        {
            objectListView1.AddObject(e);
        }

        private void FormBackgroundTasks_FormClosed(object sender, FormClosedEventArgs e)
        {
            BackgroundTaskService.OnAdd -= BackgroundTaskService_OnAdd;
            BackgroundTaskService.OnFinish -= BackgroundTaskService_OnFinish;
        }
    }
}
