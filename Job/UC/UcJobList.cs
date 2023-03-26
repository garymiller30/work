// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using BrightIdeasSoftware;
using ExtensionMethods;
using Interfaces;
using Job.Ext;
using Job.Menus;
using Job.Models;
using Job.Profiles;
using Job.UserForms;
using Logger;
using Ookii.Dialogs.WinForms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.UC
{
    public sealed partial class UcJobList : UserControl, IUcJobList
    {

        //private readonly PythonEngine.PythonEngine _pythonEngine;
        private readonly IUserProfile _profile;
        private readonly RichTextBox _richTextBox = new RichTextBox();
        //private IJob _jobForToolStripActive;

        public EventHandler<int> OnChangeCountJobs { get; set; } = delegate { };

        public UcJobList()
        {
            InitializeComponent();
            var rbd = new RowBorderDecoration
            {
                BorderPen = new Pen(Color.FromArgb(255, Color.DarkBlue), 1),
                BoundsPadding = new Size(0, -1),
                CornerRounding = 3.0F
            };

            objectListView_NewWorks.SelectedRowDecoration = rbd;

            //_pythonEngine = new PythonEngine.PythonEngine();
        }


        public UcJobList(IUserProfile userProfile) : this()
        {
            _profile = userProfile;

            InitOlvColumns();
            RestoreOLVState();
            InitObjectListviewImageList();

            olvColumn_Status.Renderer = new BaseRenderer();
            olvColumn_Status.ImageGetter += ImageGetter;
            olvColumnProcess.Renderer = new BarRenderer(0, 100);


            InitMainToolStrip();
            AddingExtendedSettings();
            //ApplyViewFilter();
        }
        private void ObjectListView_NewWorks_DoubleClick(object sender, EventArgs e)
        {
            EditJob2();
        }

        private void ObjectListView_NewWorks_Click(object sender, EventArgs e)
        {
            _profile.Jobs.SetCurrentJob(objectListView_NewWorks.SelectedObject as IJob);
        }

        private void EditJob2()
        {
            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                _profile.Jobs.LockJob(j);

                _profile.Plugins.BeforeJobChange(j);

                var jobParameters = new JobParameters(j);
                using (var faw = new FormAddWork2(_profile, jobParameters, false))
                {
                    var oldPath = _profile.Jobs.GetFullPathToWorkFolder(j);

                    if (faw.ShowDialog() == DialogResult.OK)
                    {
                        _profile.Plugins.AfterJobChange(jobParameters);
                        if (_profile.Jobs.RenameJobDirectory(oldPath, jobParameters))
                        {

                            jobParameters.ApplyToJob();
                            //_profile.Jobs.UnlockJob(j);
                            _profile.Jobs.UpdateJob(j);
                            objectListView_NewWorks.RefreshObject(j);
                            var newPath = _profile.Jobs.GetFullPathToWorkFolder(j);
                            _profile.FileBrowser.Browsers[0].SetRootFolder(newPath);
                        }

                    }
                    _profile.Jobs.UnlockJob(j);
                    //_profile.Jobs.UpdateJob(j);
                }

            }
        }

        private void objectListView_NewWorks_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = ModifierKeys == Keys.Control ? DragDropEffects.Move : DragDropEffects.Copy;
        }


        private void ObjectListView_NewWorks_CanDrop(object sender, OlvDropEventArgs e)
        {
            e.Effect = ModifierKeys == Keys.Control ? DragDropEffects.Move : DragDropEffects.Copy;
        }

        private void ObjectListView_NewWorks_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column == olvColumn_OrderNumber)
            {
                var str = e.NewValue.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    var job = e.RowObject as IJob;
                    _profile.Jobs.ChangeJobNumber(job, str);
                }
                e.Cancel = true;
            }
        }

        private void RestoreOLVState()
        {
            if (_profile.Settings.OLVState != null)
            {
                var state = Convert.FromBase64String(_profile.Settings.OLVState);
                objectListView_NewWorks.RestoreState(state);
            }
        }

        private void ToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObjects == null) return;

            var menuSendTo = (MenuSendTo)((ToolStripItem)sender).Tag;

            if (string.IsNullOrEmpty(menuSendTo.CommandLine)) return;

            var jobs = objectListView_NewWorks.SelectedObjects.Cast<IJob>().ToArray();

            foreach (var job in jobs)
            {
                //if (menuSendTo.CommandLine.Contains("{0}")) // file
                //{
                //    ProcessSingleFile(job, menuSendTo);
                //}
                //else 
                if (menuSendTo.CommandLine.Contains("{1}")) //folder
                {
                    ProcessFolder(job, menuSendTo);
                }
            }
            return;
        }

        private void ProcessFolder(IJob job, MenuSendTo menuSendTo)
        {
            if (_profile.ScriptEngine.IsScriptFile(menuSendTo.Path))
            {
                var scriptParam = _profile.ScriptEngine.JobList.PrepareScriptToStart(job, menuSendTo);
                _profile.ScriptEngine.JobList.RunScript(scriptParam);
            }
            else
            {
                var pii = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetDirectoryName(menuSendTo.Path),
                    FileName = menuSendTo.Path,
                    Arguments = _profile.ScriptEngine.JobList.PrepareCommandlineArguments(job, menuSendTo)
                };

                var p = Process.Start(pii);
                if (menuSendTo.EventOnFinish)
                {
                    p?.WaitForExit();
                }
            }
        }

        //private void ProcessSingleFile(IJob job, MenuSendTo menuSendTo)
        //{
        //    var fileList = _profile.FileBrowser.Browsers[0].GetFilesFromDirectory(_profile.Jobs.GetFullPathToWorkFolder(job));

        //    foreach (FileSystemInfoExt info in fileList)
        //    {
        //        var pii = new ProcessStartInfo
        //        {
        //            WorkingDirectory = Path.GetDirectoryName(menuSendTo.Path),
        //            FileName = menuSendTo.Path,
        //            Arguments = _profile.ScriptEngine.JobList.PrepareCommandlineArguments(job, menuSendTo, info)
        //        };
        //        var p = Process.Start(pii);
        //        if (menuSendTo.EventOnFinish)
        //        {
        //            p?.WaitForExit();
        //        }
        //    }
        //}

        private void InitMainToolStrip()
        {
            toolStripWorks.Items.Clear();
            toolStripMainScriptPanel.Items.Clear();

            var menus = _profile.MenuManagers.Utils.GetToolStripButtons(0, ToolsStripMenuItem_Click, true);

            //var mainMenus = new List<ToolStripItem>();
            //bool isMenuEmpty = true;

            foreach (var menu in menus)
            {
                if (menu is ToolStripButton || menu is ToolStripSplitButton)
                {
                    if (menu.Tag is MenuSendTo menuSendTo)
                    {
                        if (menuSendTo.UsedInMainWindow && menuSendTo.Enable)
                        {
                            if (menuSendTo.IsScript())
                            {
                                toolStripMainScriptPanel.Items.Add(menu);
                            }
                            else
                            {
                                toolStripWorks.Items.Add(menu);
                            }
                        }
                    }
                }
            }

            toolStripWorks.Visible = toolStripWorks.Items.Count > 0;
            toolStripMainScriptPanel.Visible = toolStripMainScriptPanel.Items.Count > 0;
        }


        private void InitOlvColumns()
        {
            // без цього не малює прогрес бар
            objectListView_NewWorks.OwnerDraw = true;

            olvColumn_Date.AspectGetter += x => ((IJob)x).Date.ToLocalTime();
            olvColumnCategories.AspectGetter += r => _profile.Categories.GetCategoryById(((IJob)r).CategoryId)?.Name;
            olvColumnNote.AspectGetter += AspectGetterNote;
            olvColumn_Status.AspectGetter = rowObject => _profile.StatusManager.GetJobStatusDescriptionByCode(((IJob)rowObject).StatusCode);


        }
        private object AspectGetterNote(object job)
        {
            var note = ((IJob)job).Note;

            if (!string.IsNullOrEmpty(note) && note.TrimStart().StartsWith(@"{\rtf1", StringComparison.Ordinal))
            {
                _richTextBox.Rtf = note;
                return _richTextBox.Text;
            }

            return note;
        }


        private void InitObjectListviewImageList()
        {
            objectListView_NewWorks.SmallImageList = _profile.StatusManager.GetImageList();
        }
        private object ImageGetter(object job)
        {
            int i = _profile.StatusManager.GetIndexImageByStatusCode(((IJob)job).StatusCode);
            return i;
        }

        void _onChangeCountJobs()
        {
            OnChangeCountJobs(this, objectListView_NewWorks.Items.Count);
        }


        #region JobEvents
        public void CreateEvents()
        {
            _profile.Events.Jobs.OnJobAdd += JobEventsOnJobAdd;
            _profile.Events.Jobs.OnJobsAdd += JobEventsOnJobsAdd;
            _profile.Events.Jobs.OnJobBeginEdit += JobEventsOnJobBeginEdit;
            _profile.Events.Jobs.OnJobChange += JobEventsOnJobChange;
            _profile.Events.Jobs.OnJobFinishEdit += JobEventsOnJobFinishEdit;
            _profile.Events.Jobs.OnJobDelete += JobEventsOnJobDelete;

            _profile.Events.Jobs.Init(_profile);
        }
        private void JobEventsOnJobAdd(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.AddObject(job);
                _onChangeCountJobs();
            });
        }
        private void JobEventsOnJobsAdd(object sender, ICollection jobs)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.ClearObjects();
                objectListView_NewWorks.AddObjects(jobs);
                _onChangeCountJobs();
            });
        }
        private void JobEventsOnJobBeginEdit(object sender, IJob job)
        {
            this.InvokeIfNeeded(() => objectListView_NewWorks.DisableObject(job));
        }
        private void JobEventsOnJobChange(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {

                var j = objectListView_NewWorks.Objects.Cast<IJob>().FirstOrDefault(x => x.Id.Equals(job.Id));
                if (j != null)
                {
                    //якщо змінився шлях і робота зараз вибрана
                    var isChangeFolder = _profile.Jobs.GetFullPathToWorkFolder(j)
                        .Equals(_profile.Jobs.GetFullPathToWorkFolder(job)) &&
                                         Equals(objectListView_NewWorks.SelectedObject, j);
                    j.Update(job);
                    objectListView_NewWorks.RefreshObject(j);

                    if (isChangeFolder)
                    {
                        _profile.Jobs.SetCurrentJob(j);
                        //_profile.FileBrowser.Browsers[0].SetRootFolder(_profile.Jobs.GetFullPathToWorkFolder(j));
                        //_profile.FileBrowser.Browsers[0].CurrentJob = j;
                    }

                    ApplyViewFilter(j);
                }
                else
                {
                    if (_profile.StatusManager.IsVisible(job.StatusCode))
                        objectListView_NewWorks.AddObject(job);
                }

                _onChangeCountJobs();
            });
        }
        private void JobEventsOnJobDelete(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.RemoveObject(job);
                _onChangeCountJobs();
            });
        }

        private void JobEventsOnJobFinishEdit(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.EnableObject(job);
            });
        }

        #endregion


        //bool CheckFilter(object job)
        //{
        //    var statusCode = ((IJob)job).StatusCode;
        //    return _profile.StatusManager.IsViewStatusChecked(statusCode);
        //}



        public string GetSelectedJobPath()
        {
            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                return _profile.Jobs.GetFullPathToWorkFolder(j);
            }
            return null;
        }

        //public IJob GetSelectedJob()
        //{
        //    return objectListView_NewWorks.SelectedObject as IJob;
        //}

        public void ChangeSelectedJobsStatus(IJobStatus status)
        {
            if (objectListView_NewWorks.SelectedObjects.Count > 0)
            {
                foreach (IJob job in objectListView_NewWorks.SelectedObjects)
                {
                    job.StatusCode = status.Code;
                    _profile.CustomersNotifyManager.Notify(job);
                    _profile.Jobs.UpdateJob(job);
                    _profile.StatusManager.OnChangeStatusesParams.Run(job);
                }
                objectListView_NewWorks.RefreshObjects(objectListView_NewWorks.SelectedObjects);
            }
        }

        public void CreateJobFromFile(string file)
        {
            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                Factory.CreateJobFromFile(_profile, j, file);
            }
        }

        public IJob ChangeSelectedJobDescription(string description)
        {
            if (objectListView_NewWorks.SelectedObject is IJob job)
            {
                if (_profile.Jobs.ChangeJobDescription(job, description))
                {
                    objectListView_NewWorks.RefreshObject(job);
                    return job;
                }
            }
            return null;
        }
        public void Close() => SaveOLVState();
        public void Search(string text)
        {
            objectListView_NewWorks.ClearObjects();
            _profile.Jobs.Search(text);

        }



        public void RepeatSelectedJob() => RepeatJob();

        #region ApplyViewFilter
        public void ApplyViewFilter()
        {
            objectListView_NewWorks.ClearObjects();
            _profile.Jobs?.ApplyStatusViewFilter();
            _onChangeCountJobs();
            //objectListView_NewWorks.ModelFilter = new ModelFilter(CheckFilter);
        }

        public void ApplyViewFilter(DateTime date)
        {
            //objectListView_NewWorks.ModelFilter = new ModelFilter(o => ((IJob)o).Date.Date.Equals(date.Date) );
            objectListView_NewWorks.ClearObjects();
            _profile.Jobs.ApplyDateFilter(date);
        }

        public void ApplyViewFilter(IJob job)
        {
            var visibleStatuses = _profile.StatusManager.GetEnabledViewStatuses();
            if (!visibleStatuses.Contains(job.StatusCode))
            {
                objectListView_NewWorks.RemoveObject(job);
            }

        }
        #endregion

        public void ApplyJobListFilter(string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
                objectListView_NewWorks.ModelFilter = null;
            else
            {
                objectListView_NewWorks.ModelFilter = new TextMatchFilter(objectListView_NewWorks, filterText);
            }
        }



        private void ContextMenuStrip_NewJob_Opening(object sender, CancelEventArgs e)
        {
            if (objectListView_NewWorks.SelectedObjects.Count > 1)
            {
                toolStripSeparator3.Visible = false;
                обєднатиВОднеЗамовленняToolStripMenuItem.Visible = true;

                создатьSignaJobToolStripMenuItem.Visible = false;
                повторитьЗаказToolStripMenuItem.Visible = false;
                копироватьВБуферНомерЗаказаToolStripMenuItem.Visible = false;
                копироватьВБуферОписаниеЗаказаToolStripMenuItem.Visible = false;
            }
            else if (objectListView_NewWorks.SelectedObject != null)
            {
                toolStripSeparator3.Visible = true;
                обєднатиВОднеЗамовленняToolStripMenuItem.Visible = false;

                создатьSignaJobToolStripMenuItem.Visible = true;
                повторитьЗаказToolStripMenuItem.Visible = true;
                копироватьВБуферНомерЗаказаToolStripMenuItem.Visible = true;
                копироватьВБуферОписаниеЗаказаToolStripMenuItem.Visible = true;

                var job = objectListView_NewWorks.SelectedObject as IJob;
                создатьSignaJobToolStripMenuItem.Text =
                    job.IsSignaJobExist(_profile) ?
                        "відкрити файл Prinect Signa" :
                        "створити файл Prinect Signa";

                доповненняToolStripMenuItem.DropDownItems.Clear();
                доповненняToolStripMenuItem.DropDownItems.AddRange(_profile.MenuManagers.Utils.Get(ToolsStripMenuItem_Click).ToArray());
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void AddingExtendedSettings()
        {
            var s = _profile.Settings as ProfileSettings;
            if (s.ExtendedSettings.CanDeleteJobs)
            {
                var toolstripItem = new ToolStripMenuItem("(A) Видалити роботу");
                toolstripItem.Click += ToolstripItem_Click;
                contextMenuStrip_NewJob.Items.Add(toolstripItem);
            }
        }

        private void ToolstripItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObjects.Count > 0)
            {
                TaskDialog td = new TaskDialog();
                td.Buttons.Add(new TaskDialogButton(ButtonType.Yes));
                td.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
                td.WindowTitle = "Видалення робіт";
                td.Content = "Видалити вибрані роботи?\nВидаляється тільки запис з бази данних, файли залишаються";
                var button = td.ShowDialog();

                if (button.ButtonType == ButtonType.Yes)
                {
                    var jobs = objectListView_NewWorks.SelectedObjects.Cast<IJob>().ToArray();
                    foreach (var job in jobs)
                    {
                        _profile.Jobs.DeleteJob(job);
                    }
                    objectListView_NewWorks.RemoveObjects(jobs);
                }
            }
        }

        private void SaveOLVState()
        {
            var state = objectListView_NewWorks.SaveState();
            _profile.Settings.OLVState = Convert.ToBase64String(state);
            ProfilesController.Save(_profile);
        }

        private void СоздатьSignaJobToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                _profile.Jobs.CreateOrOpenSignaJob(_profile.Settings.GetJobSettings().SignaJobsPath, j);
            }
        }

        private void ПовторитьЗаказToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RepeatJob();
        }

        private void RepeatJob()
        {
            if (objectListView_NewWorks.SelectedObject != null)
            {
                _profile.Jobs.Dublicate((IJob)objectListView_NewWorks.SelectedObject);
            }
        }


        private void КопироватьВБуферНомерЗаказаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObject is IJob o)
            {
                try
                {
                    Clipboard.SetText(o.Number);
                }
                catch (Exception ee)
                {
                    Log.Error(_profile, "Clipboard", ee.Message);

                }

            }
        }

        private void КопироватьВБуферОписаниеЗаказаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObject is IJob o)
            {
                try
                {
                    Clipboard.SetText(o.Description);
                }
                catch (Exception ee)
                {
                    Log.Error(_profile, "Clipboard", ee.Message);
                }

            }
        }

        private void ObjectListView_NewWorks_Dropped(object sender, OlvDropEventArgs e)
        {

            if (!(e.DropTargetItem.RowObject is IJob job))
                return;

            if (e.DataObject is OLVDataObject olvobj)
            {
                var downloadParam = _profile.Ftp.FtpExplorer.GetDownloadFileParam(olvobj.ModelObjects);
                if (downloadParam != null)
                {
                    //todo: подумати над реалізацією закачки файлів з ftp через drag`n`drop
                    //StartDownloadFilesFromFtp(job, downloadParam);
                }
            }
            else if (((IDataObject)e.DataObject).GetDataPresent(DataFormats.FileDrop))
            {
                FileManager.CopyFiles((string[])((IDataObject)e.DataObject).GetData(DataFormats.FileDrop),
                    _profile.Jobs.GetFullPathToWorkFolder(job));
            }
            else if (((IDataObject)e.DataObject).GetDataPresent(DataFormats.StringFormat))
            {
                var link = ((IDataObject)e.DataObject).GetData(typeof(string));

                if (link != null)
                {
                    if (link.ToString().StartsWith("http"))
                    {
                        DownloadFromHttpLinkAsync(job, link.ToString());
                    }
                }

            }
        }

        public void UnlockJob(IJob job) => objectListView_NewWorks.EnableObject(job);
        public void LockJob(IJob job) => objectListView_NewWorks.DisableObject(job);

        private async void DownloadFromHttpLinkAsync(IJob job, string link)
        {
            LockJob(job);
            try
            {
                await Task.Run(() => _profile.Jobs.DownloadFromHttp(link, job)).ConfigureAwait(false);
            }
            catch (Exception)
            {
            }

            UnlockJob(job);

        }

        private void ОбєднатиВОднеЗамовленняToolStripMenuItem_Click(object sender, EventArgs e) =>
            _profile.Jobs.CombineOrdersInOne(objectListView_NewWorks.SelectedObjects);


        private void objectListView_NewWorks_FormatRow(object sender, FormatRowEventArgs e) =>
            _profile.Plugins?.JobListFormatRow(e.Item);
    }
}
