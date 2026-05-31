using BackgroundTaskServiceLib;
using ExtensionMethods;
using System;
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
            BackgroundTaskService.OnChanged += BackgroundTaskService_OnChanged;
        }

        private void BackgroundTaskService_OnFinish(object sender, BackgroundTaskItem e)
        {
            this.InvokeIfNeeded(() => objectListView1.RemoveObject(e));
        }

        private void BackgroundTaskService_OnAdd(object sender, BackgroundTaskItem e)
        {
            this.InvokeIfNeeded(() => objectListView1.AddObject(e));
        }

        private void BackgroundTaskService_OnChanged(object sender, BackgroundTaskItem e)
        {
            this.InvokeIfNeeded(() => objectListView1.RefreshObject(e));
        }

        private void FormBackgroundTasks_FormClosed(object sender, FormClosedEventArgs e)
        {
            BackgroundTaskService.OnAdd -= BackgroundTaskService_OnAdd;
            BackgroundTaskService.OnFinish -= BackgroundTaskService_OnFinish;
            BackgroundTaskService.OnChanged -= BackgroundTaskService_OnChanged;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is BackgroundTaskItem task)
            {
                task.Cancel();
                objectListView1.RefreshObject(task);
            }
        }
    }
}
